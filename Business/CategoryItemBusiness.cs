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

namespace Holism.Taxonomy.Business
{
    public class CategoryItemBusiness : Business<CategoryItemView, CategoryItem>
    {
        protected override Repository<CategoryItem> ModelRepository => RepositoryFactory.CategoryItemFrom(taxonomyDatabaseName);

        protected override ViewRepository<CategoryItemView> ViewRepository => RepositoryFactory.CategoryItemViewFrom(taxonomyDatabaseName);

        string taxonomyDatabaseName;

        string entityDatabaseName;

        public CategoryItemBusiness(string taxonomyDatabaseName = null, string entityDatabaseName = null)
        {
            this.taxonomyDatabaseName = taxonomyDatabaseName;
            this.entityDatabaseName = entityDatabaseName;
        }

        private static Dictionary<Guid, Func<List<Guid>, Dictionary<Guid, object>>> entitiesInfoAugmenter = new Dictionary<Guid, Func<List<Guid>, Dictionary<Guid, object>>>();

        public static Action<CategoryItem> OnCategoryToggled;

        public void RegisterEnittyInfoAugmenter(string entityTypeName, Func<List<Guid>, Dictionary<Guid, object>> augmenter)
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

        public void UpdateOrder(long categoryItemId, int newOrder)
        {
            var categoryItem = ModelRepository.Get(categoryItemId);
            categoryItem.Order = newOrder;
            Update(categoryItem);
        }

        public List<CategoryItemNode> GetItemCategories(string entityTypeName, Guid entityGuid)
        {
            var entityTypeGuid = new EntityTypeBusiness(entityDatabaseName).GetGuid(entityTypeName);
            var categoryIds = ViewRepository.All.Where(i => i.EntityTypeGuid == entityTypeGuid && i.EntityGuid == entityGuid).Select(i => i.CategoryId).ToList();
            var categories = new CategoryBusiness(taxonomyDatabaseName, entityDatabaseName).GetHierarchy(entityTypeName);
            var categoryItems = new List<CategoryItemNode>();
            foreach (var category in categories)
            {
                var categoryItem = new CategoryItemNode();
                categoryItem.Children = new List<CategoryItemNode>();
                categoryItem.IconUrl = category.IconUrl;
                categoryItem.IconSvg = category.IconSvg;
                categoryItem.CategoryId = category.Id;
                categoryItem.Title = category.Title;
                if (categoryIds.Contains(categoryItem.CategoryId))
                {
                    categoryItem.IsInThisCategory = true;
                }
                categoryItems.Add(categoryItem);
                GetChildrenCategoryItems(categoryIds, category, categoryItem);
            }
            return categoryItems;
        }

        public int GetCountOfItemsInCategory(Category category)
        {
            var count = ViewRepository.All.Count(i => i.EntityTypeGuid == category.EntityTypeGuid && i.CategoryId == category.Id);
            return count;
        }

        public List<CategoryItemView> GetAllItems(long categoryId)
        {
            var category = new CategoryBusiness(taxonomyDatabaseName, entityDatabaseName).Get(categoryId);
            var allItems = ViewRepository.All.Where(i => i.CategoryId == categoryId).OrderBy(i => i.Order).ThenBy(i => i.Id).ToList();
            if (entitiesInfoAugmenter.ContainsKey(category.EntityTypeGuid))
            {
                var entityGuids = allItems.Select(i => i.EntityGuid).ToList();
                var entityInfoList = entitiesInfoAugmenter[category.EntityTypeGuid](entityGuids);
                var categoryItemsWithEntityInfo = allItems.Where(i => entityInfoList.ContainsKey(i.EntityGuid)).ToList();
                foreach (var categoryItem in categoryItemsWithEntityInfo)
                {
                    ExpandoObjectExtensions.AddProperty(categoryItem.RelatedItems, new EntityTypeBusiness(entityDatabaseName).GetName(category.EntityTypeGuid), entityInfoList[categoryItem.EntityGuid]);
                }
            }
            return allItems;
        }

        private void GetChildrenCategoryItems(List<long> categoryIds, CategoryNode category, CategoryItemNode categoryItem)
        {
            var categoryItemNodes = new List<CategoryItemNode>();
            foreach (var child in category.Children)
            {
                var childCategoryItem = new CategoryItemNode();
                childCategoryItem.Children = new List<CategoryItemNode>();
                childCategoryItem.IconUrl = child.IconUrl;
                childCategoryItem.IconSvg = category.IconSvg;
                childCategoryItem.CategoryId = child.Id;
                childCategoryItem.Title = child.Title;
                if (categoryIds.Contains(childCategoryItem.CategoryId))
                {
                    childCategoryItem.IsInThisCategory = true;
                }
                categoryItem.Children.Add(childCategoryItem);
                GetChildrenCategoryItems(categoryIds, child, childCategoryItem);
            }
        }

        public ListResult<Guid> GetEntityGuids(long categoryId, int pageNumber, List<Guid> excludedEntityGuids)
        {
            CheckExcludedEntitiesCount(excludedEntityGuids);
            var listOptions = ListOptions.Create();
            listOptions.PageNumber = pageNumber;
            listOptions.AddFilter<CategoryItem>(i => i.CategoryId, categoryId.ToString());
            listOptions.AddSort<CategoryItem>(i => i.Order, SortDirection.Ascending);
            listOptions.AddSort<CategoryItem>(i => i.Id, SortDirection.Ascending);
            var entityGuids = ModelRepository
                .All
                .Where(i => !excludedEntityGuids.Contains(i.EntityGuid))
                .ApplyListOptionsAndGetTotalCount(listOptions)
                .Convert<CategoryItem, Guid>(i => i.EntityGuid);
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
                .Convert<CategoryItem, Guid>(i => i.EntityGuid);
            return entityGuids;
        }

        public void ToggleCategory(string entityTypeName, long categoryId, Guid entityGuid)
        {
            var entityTypeGuid = new EntityTypeBusiness(entityDatabaseName).GetGuid(entityTypeName);
            entityGuid.Ensure().IsNotNull().And().AsString().IsNotEmptyGuid();
            categoryId.Ensure().IsNumeric().And().IsGreaterThanZero();
            var categoryItem = ModelRepository.All.FirstOrDefault(i => i.EntityTypeGuid == entityTypeGuid && i.EntityGuid == entityGuid && i.CategoryId == categoryId);
            if (categoryItem.IsNull())
            {
                categoryItem = new CategoryItem();
                categoryItem.EntityTypeGuid = entityTypeGuid;
                categoryItem.EntityGuid = entityGuid;
                categoryItem.CategoryId = categoryId;
                categoryItem.Order = 1;
                ModelRepository.Create(categoryItem);
            }
            else
            {
                ModelRepository.Delete(categoryItem);
            }
            new CategoryBusiness(taxonomyDatabaseName).CountItemsInCategory(categoryId);
            OnCategoryToggled?.Invoke(categoryItem);
        }

        public void PutInCategory(string entityTypeName, long categoryId, Guid entityGuid)
        {
            if (IsInCategory(entityTypeName, entityGuid, categoryId))
            {
                return;
            }
            ToggleCategory(entityTypeName, categoryId, entityGuid);
        }

        public void RemoveEntity(string entityTypeName, Guid entityGuid)
        {
            var entityTypeGuid = new EntityTypeBusiness(entityDatabaseName).GetGuid(entityTypeName);
            var query = $@"
                delete
                from {ModelRepository.TableName}
                where EntityGuid = '{entityGuid}'
                ";
            ModelRepository.Run(query);
        }

        public void RemoveOrphanEntities(string entityTypeName, List<Guid> entityGuids)
        {
            var entityTypeGuid = new EntityTypeBusiness(entityDatabaseName).GetGuid(entityTypeName);
            var orphanCategoryItems = ModelRepository.All.Where(i => !entityGuids.Contains(i.EntityGuid)).ToList();
            foreach (var orphanCategoryItem in orphanCategoryItems)
            {
                ModelRepository.Delete(orphanCategoryItem);
            }
        }

        public List<long> GetItemCategoryIds(string entityTypeName, Guid entityGuid)
        {
            var entityTypeGuid = new EntityTypeBusiness(entityDatabaseName).GetGuid(entityTypeName);
            var categoryItems = ViewRepository.All.Where(i => i.EntityTypeGuid == entityTypeGuid && i.EntityGuid == entityGuid).Select(i => i.CategoryId).Distinct().ToList();
            return categoryItems;
        }

        public bool IsInCategory(string entityTypeName, Guid entityGuid, long categoryId)
        {
            var entityTypeGuid = new EntityTypeBusiness(entityDatabaseName).GetGuid(entityTypeName);
            var categoryItem = ViewRepository.All.Any(i => i.EntityTypeGuid == entityTypeGuid && i.EntityGuid == entityGuid && i.CategoryId == categoryId);
            return categoryItem;
        }
    }
}