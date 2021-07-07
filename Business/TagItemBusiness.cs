using Holism.Business;
using Holism.Entity.Business;
using Holism.EntityFramework;
using Holism.Framework;
using Holism.Framework.Extensions;
using Holism.Taxonomy.DataAccess;
using Holism.Taxonomy.DataAccess.Models;
using Holism.Taxonomy.DataAccess.Models.Views;
using Holism.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holism.Taxonomy.Business
{
    public class TagItemBusiness : Business<TagItemView, TagItem>
    {
        protected override Repository<TagItem> ModelRepository => RepositoryFactory.TagItemFrom(taxonomyDatabaseName);

        protected override ViewRepository<TagItemView> ViewRepository => RepositoryFactory.TagItemViewFrom(taxonomyDatabaseName);

        string taxonomyDatabaseName;

        string entityDatabaseName;

        public TagItemBusiness(string taxonomyDatabaseName = null, string entityDatabaseName = null)
        {
            this.taxonomyDatabaseName = taxonomyDatabaseName;
            this.entityDatabaseName = entityDatabaseName;
        }

        private static Dictionary<Guid, Func<List<Guid>, Dictionary<Guid, object>>> entitiesInfoAugmenter = new Dictionary<Guid, Func<List<Guid>, Dictionary<Guid, object>>>();

        public static Action<TagItem> OnTagToggled;

        public void RegisterEntityInfoAugmenter(string entityTypeName, Func<List<Guid>, Dictionary<Guid, object>> augmenter)
        {
            var entityTypeGuid = new EntityTypeBusiness(entityDatabaseName).GetGuid(entityTypeName);
            if (entitiesInfoAugmenter.ContainsKey(entityTypeGuid))
            {
                entitiesInfoAugmenter[entityTypeGuid] = augmenter;
            }
            else
            {
                entitiesInfoAugmenter.Add(entityTypeGuid, augmenter);
            }
        }

        public void UpdateOrder(long tagItemId, int newOrder)
        {
            var tagItem = ModelRepository.Get(tagItemId);
            tagItem.Order = newOrder;
            Update(tagItem);
        }

        //public List<TagItem> GetItemTags(string entityTypeName, long entityGuid)
        //{
        //    var entityTypeGuid = new EntityTypeBusiness(entityDatabaseName).GetGuid(entityTypeName);
        //    var tagIds = ViewRepository.All.Where(i => i.EntityTypeGuid == entityTypeGuid && i.EntityGuid == entityGuid).Select(i => i.TagId).ToList();
        //    var tags = new TagBusiness(taxonomyDatabaseName, entityDatabaseName).GetHierarchy(entityTypeName);
        //    var tagItems = new List<TagItem>();
        //    foreach (var tag in tags)
        //    {
        //        var tagItem = new TagItem();
        //        tagItem.Children = new List<TagItem>();
        //        tagItem.IconUrl = tag.IconUrl;
        //        tagItem.IconSvg = tag.IconSvg;
        //        tagItem.TagId = tag.Id;
        //        tagItem.Title = tag.Title;
        //        if (tagIds.Contains(tagItem.TagId))
        //        {
        //            tagItem.IsInThisTag = true;
        //        }
        //        tagItems.Add(tagItem);
        //        GetChildrenTagItems(tagIds, tag, tagItem);
        //    }
        //    return tagItems;
        //}

        public int GetCountOfItemsInTag(Tag tag)
        {
            var count = ViewRepository.All.Count(i => i.EntityTypeGuid == tag.EntityTypeGuid && i.TagId == tag.Id);
            return count;
        }

        public List<TagItemView> GetAllItems(long tagId)
        {
            var tag = new TagBusiness(taxonomyDatabaseName, entityDatabaseName).Get(tagId);
            var allItems = ViewRepository.All.Where(i => i.TagId == tagId).OrderBy(i => i.Order).ThenBy(i => i.Id).ToList();
            if (entitiesInfoAugmenter.ContainsKey(tag.EntityTypeGuid))
            {
                var entityGuids = allItems.Select(i => i.EntityGuid).ToList();
                var entityInfoList = entitiesInfoAugmenter[tag.EntityTypeGuid](entityGuids);
                var tagItemsWithEntityInfo = allItems.Where(i => entityInfoList.ContainsKey(i.EntityGuid)).ToList();
                foreach (var tagItem in tagItemsWithEntityInfo)
                {
                    ExpandoObjectExtensions.AddProperty(tagItem.RelatedItems, new EntityTypeBusiness(entityDatabaseName).GetName(tag.EntityTypeGuid), entityInfoList[tagItem.EntityGuid]);
                }
            }
            return allItems;
        }

        public ListResult<Guid> GetEntityGuids(long tagId, int pageNumber, List<Guid> excludedEntityGuids)
        {
            CheckExcludedEntitiesCount(excludedEntityGuids);
            var listOptions = ListOptions.Create();
            listOptions.PageNumber = pageNumber;
            listOptions.AddFilter<TagItem>(i => i.TagId, tagId.ToString());
            listOptions.AddSort<TagItem>(i => i.Order, SortDirection.Ascending);
            listOptions.AddSort<TagItem>(i => i.Id, SortDirection.Ascending);
            var entityGuids = ModelRepository
                .All
                .Where(i => !excludedEntityGuids.Contains(i.EntityGuid))
                .ApplyListOptionsAndGetTotalCount(listOptions)
                .Convert<TagItem, Guid>(i => i.EntityGuid);
            return entityGuids;
        }

        private void CheckExcludedEntitiesCount(List<Guid> excludedEntityGuids)
        {
            if (excludedEntityGuids.Count > 100)
            {
                throw new BusinessException("Excluding more than 100 items will slow down the system logarithmically. Please solve this problem.");
            }
        }

        public ListResult<Guid> GetEntityGuids(ListOptions listOptions, List<Guid> excludedEntityGuids)
        {
            CheckExcludedEntitiesCount(excludedEntityGuids);
            var entityGuids = ModelRepository
                .All
                .Where(i => !excludedEntityGuids.Contains(i.EntityGuid))
                .ApplyListOptionsAndGetTotalCount(listOptions)
                .Convert<TagItem, Guid>(i => i.EntityGuid);
            return entityGuids;
        }

        public void ToggleTag(string entityTypeName, long tagId, Guid entityGuid)
        {
            var entityTypeGuid = new EntityTypeBusiness(entityDatabaseName).GetGuid(entityTypeName);
            entityGuid.Ensure().IsNotNull().And().AsString().IsNotEmptyGuid();
            tagId.Ensure().IsNumeric().And().IsGreaterThanZero();
            var tagItem = ModelRepository.All.FirstOrDefault(i => i.EntityTypeGuid == entityTypeGuid && i.EntityGuid == entityGuid && i.TagId == tagId);
            if (tagItem.IsNull())
            {
                tagItem = new TagItem();
                tagItem.EntityTypeGuid = entityTypeGuid;
                tagItem.EntityGuid = entityGuid;
                tagItem.TagId = tagId;
                tagItem.Order = 1;
                ModelRepository.Create(tagItem);
            }
            else
            {
                ModelRepository.Delete(tagItem);
            }
            new TagBusiness(taxonomyDatabaseName, entityDatabaseName).CountItemsInTag(tagId);
            OnTagToggled?.Invoke(tagItem);
        }

        public void PutInTag(string entityTypeName, long tagId, Guid entityGuid)
        {
            if (IsInTag(entityTypeName, entityGuid, tagId))
            {
                return;
            }
            ToggleTag(entityTypeName, tagId, entityGuid);
        }

        public void RemoveEntity(string entityTypeName, Guid entityGuid)
        {
            var entityTypeGuid = new EntityTypeBusiness(entityDatabaseName).GetGuid(entityTypeName);
        }

        public void RemoveOrphanEntities(string entityTypeName, List<Guid> entityGuids)
        {
            var entityTypeGuid = new EntityTypeBusiness(entityDatabaseName).GetGuid(entityTypeName);
            var orphanTagItems = ModelRepository.All.Where(i => !entityGuids.Contains(i.EntityGuid)).ToList();
            foreach (var orphanTagItem in orphanTagItems)
            {
                ModelRepository.Delete(orphanTagItem);
            }
        }

        public bool IsInTag(string entityTypeName, Guid entityGuid, long tagId)
        {
            var entityTypeGuid = new EntityTypeBusiness(entityDatabaseName).GetGuid(entityTypeName);
            var tagItem = ViewRepository.All.Any(i => i.EntityTypeGuid == entityTypeGuid && i.EntityGuid == entityGuid && i.TagId == tagId);
            return tagItem;
        }
    }
}
