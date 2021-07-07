using Holism.Business;
using Holism.Entity.Business;
using Holism.DataAccess;
using Holism.Framework;
using Holism.Taxonomy.DataAccess;
using Holism.Taxonomy.Models;
using Holism.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Holism.Taxonomy.Business
{
    public class HierarchyItemBusiness : Business<HierarchyItem, HierarchyItem>
    {
        protected override Repository<HierarchyItem> WriteRepository => RepositoryFactory.HierarchyItem;

        protected override ReadRepository<HierarchyItem> ReadRepository => RepositoryFactory.HierarchyItem;

        private static Dictionary<Guid, Func<List<Guid>, Dictionary<Guid, object>>> entitiesInfoAugmenter = new Dictionary<Guid, Func<List<Guid>, Dictionary<Guid, object>>>();

        public static Action<HierarchyItem> OnHierarchyToggled;

        public void RegisterEnittyInfoAugmenter(string entityTypeName, Func<List<Guid>, Dictionary<Guid, object>> augmenter)
        {
            var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityTypeName);
            if (entitiesInfoAugmenter.ContainsKey(entityTypeGuid))
            {
                entitiesInfoAugmenter[entityTypeGuid] = augmenter;
            }
            else
            {
                entitiesInfoAugmenter.Add(entityTypeGuid, augmenter);
            }
        }

        public void UpdateOrder(long hierarchyItemId, int newOrder)
        {
            var hierarchyItem = WriteRepository.Get(hierarchyItemId);
            hierarchyItem.Order = newOrder;
            Update(hierarchyItem);
        }

        public List<HierarchyItemNode> GetItemHierarchies(string entityTypeName, Guid entityGuid)
        {
            var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityTypeName);
            var hierarchyIds = ReadRepository.All.Where(i => i.EntityTypeGuid == entityTypeGuid && i.EntityGuid == entityGuid).Select(i => i.HierarchyId).ToList();
            var hierarchies = new HierarchyBusiness().GetHierarchy(entityTypeName);
            var hierarchyItems = new List<HierarchyItemNode>();
            foreach (var hierarchy in hierarchies)
            {
                var hierarchyItem = new HierarchyItemNode();
                hierarchyItem.Children = new List<HierarchyItemNode>();
                hierarchyItem.IconUrl = hierarchy.IconUrl;
                hierarchyItem.IconSvg = hierarchy.IconSvg;
                hierarchyItem.HierarchyId = hierarchy.Id;
                hierarchyItem.Title = hierarchy.Title;
                if (hierarchyIds.Contains(hierarchyItem.HierarchyId))
                {
                    hierarchyItem.IsInThisHierarchy = true;
                }
                hierarchyItems.Add(hierarchyItem);
                GetChildrenHierarchyItems(hierarchyIds, hierarchy, hierarchyItem);
            }
            return hierarchyItems;
        }

        public int GetCountOfItemsInHierarchy(Hierarchy hierarchy)
        {
            var count = ReadRepository.All.Count(i => i.EntityTypeGuid == hierarchy.EntityTypeGuid && i.HierarchyId == hierarchy.Id);
            return count;
        }

        public List<HierarchyItem> GetAllItems(long hierarchyId)
        {
            var hierarchy = new HierarchyBusiness().Get(hierarchyId);
            var allItems = ReadRepository.All.Where(i => i.HierarchyId == hierarchyId).OrderBy(i => i.Order).ThenBy(i => i.Id).ToList();
            if (entitiesInfoAugmenter.ContainsKey(hierarchy.EntityTypeGuid))
            {
                var entityGuids = allItems.Select(i => i.EntityGuid).ToList();
                var entityInfoList = entitiesInfoAugmenter[hierarchy.EntityTypeGuid](entityGuids);
                var hierarchyItemsWithEntityInfo = allItems.Where(i => entityInfoList.ContainsKey(i.EntityGuid)).ToList();
                foreach (var hierarchyItem in hierarchyItemsWithEntityInfo)
                {
                    ExpandoObjectExtensions.AddProperty(hierarchyItem.RelatedItems, new EntityTypeBusiness().GetName(hierarchy.EntityTypeGuid), entityInfoList[hierarchyItem.EntityGuid]);
                }
            }
            return allItems;
        }

        private void GetChildrenHierarchyItems(List<long> hierarchyIds, HierarchyNode hierarchy, HierarchyItemNode hierarchyItem)
        {
            var hierarchyItemNodes = new List<HierarchyItemNode>();
            foreach (var child in hierarchy.Children)
            {
                var childHierarchyItem = new HierarchyItemNode();
                childHierarchyItem.Children = new List<HierarchyItemNode>();
                childHierarchyItem.IconUrl = child.IconUrl;
                childHierarchyItem.IconSvg = hierarchy.IconSvg;
                childHierarchyItem.HierarchyId = child.Id;
                childHierarchyItem.Title = child.Title;
                if (hierarchyIds.Contains(childHierarchyItem.HierarchyId))
                {
                    childHierarchyItem.IsInThisHierarchy = true;
                }
                hierarchyItem.Children.Add(childHierarchyItem);
                GetChildrenHierarchyItems(hierarchyIds, child, childHierarchyItem);
            }
        }

        public ListResult<Guid> GetEntityGuids(long hierarchyId, int pageNumber, List<Guid> excludedEntityGuids)
        {
            CheckExcludedEntitiesCount(excludedEntityGuids);
            var listParameters = ListParameters.Create();
            listParameters.PageNumber = pageNumber;
            listParameters.AddFilter<HierarchyItem>(i => i.HierarchyId, hierarchyId.ToString());
            listParameters.AddSort<HierarchyItem>(i => i.Order, SortDirection.Ascending);
            listParameters.AddSort<HierarchyItem>(i => i.Id, SortDirection.Ascending);
            var entityGuids = WriteRepository
                .All
                .Where(i => !excludedEntityGuids.Contains(i.EntityGuid))
                .ApplyListParametersAndGetTotalCount(listParameters)
                .Convert<HierarchyItem, Guid>(i => i.EntityGuid);
            return entityGuids;
        }

        private void CheckExcludedEntitiesCount(List<Guid> excludedEntityGuids)
        {
            if (excludedEntityGuids.Count > 100)
            {
                throw new BusinessException("Excluding more than 100 items will slow down the system logarithmically. Please solve this problem.");
            }
        }

        public ListResult<Guid> GetEntityGuids(ListParameters listParameters, List<Guid> excludedEntityGuids)
        {
            CheckExcludedEntitiesCount(excludedEntityGuids);
            var entityGuids = WriteRepository
                .All
                .Where(i => !excludedEntityGuids.Contains(i.EntityGuid))
                .ApplyListParametersAndGetTotalCount(listParameters)
                .Convert<HierarchyItem, Guid>(i => i.EntityGuid);
            return entityGuids;
        }

        public void ToggleHierarchy(string entityTypeName, long hierarchyId, Guid entityGuid)
        {
            var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityTypeName);
            entityGuid.Ensure().IsNotNull().And().AsString().IsNotEmptyGuid();
            hierarchyId.Ensure().IsNumeric().And().IsGreaterThanZero();
            var hierarchyItem = WriteRepository.All.FirstOrDefault(i => i.EntityTypeGuid == entityTypeGuid && i.EntityGuid == entityGuid && i.HierarchyId == hierarchyId);
            if (hierarchyItem.IsNull())
            {
                hierarchyItem = new HierarchyItem();
                hierarchyItem.EntityTypeGuid = entityTypeGuid;
                hierarchyItem.EntityGuid = entityGuid;
                hierarchyItem.HierarchyId = hierarchyId;
                hierarchyItem.Order = 1;
                WriteRepository.Create(hierarchyItem);
            }
            else
            {
                WriteRepository.Delete(hierarchyItem);
            }
            new HierarchyBusiness().CountItemsInHierarchy(hierarchyId);
            OnHierarchyToggled?.Invoke(hierarchyItem);
        }

        public void PutInHierarchy(string entityTypeName, long hierarchyId, Guid entityGuid)
        {
            if (IsInHierarchy(entityTypeName, entityGuid, hierarchyId))
            {
                return;
            }
            ToggleHierarchy(entityTypeName, hierarchyId, entityGuid);
        }

        public void RemoveEntity(string entityTypeName, Guid entityGuid)
        {
            var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityTypeName);
            var query = $@"
                delete
                from {WriteRepository.TableName}
                where EntityGuid = '{entityGuid}'
                ";
            WriteRepository.Run(query);
        }

        public void RemoveOrphanEntities(string entityTypeName, List<Guid> entityGuids)
        {
            var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityTypeName);
            var orphanHierarchyItems = WriteRepository.All.Where(i => !entityGuids.Contains(i.EntityGuid)).ToList();
            foreach (var orphanHierarchyItem in orphanHierarchyItems)
            {
                WriteRepository.Delete(orphanHierarchyItem);
            }
        }

        public List<long> GetItemHierarchyIds(string entityTypeName, Guid entityGuid)
        {
            var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityTypeName);
            var hierarchyItems = ReadRepository.All.Where(i => i.EntityTypeGuid == entityTypeGuid && i.EntityGuid == entityGuid).Select(i => i.HierarchyId).Distinct().ToList();
            return hierarchyItems;
        }

        public bool IsInHierarchy(string entityTypeName, Guid entityGuid, long hierarchyId)
        {
            var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityTypeName);
            var hierarchyItem = ReadRepository.All.Any(i => i.EntityTypeGuid == entityTypeGuid && i.EntityGuid == entityGuid && i.HierarchyId == hierarchyId);
            return hierarchyItem;
        }
    }
}