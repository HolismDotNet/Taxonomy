using Holism.Azure;
using Holism.Business;
using Holism.EntityFramework;
using Holism.Framework;
using Holism.Framework.Extensions;
using Holism.Image;
using Holism.Validation;
using Holism.Taxonomy.DataAccess;
using Holism.Taxonomy.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Holism.Entity.Business;
using System.Linq.Expressions;
using Holism.Sql;

namespace Holism.Taxonomy.Business
{
    public class CategoryBusiness : Business<Category, Category>
    {
        public const string CategoryIconsContainerName = "categoryicons";

        protected override Repository<Category> ModelRepository => RepositoryFactory.CategoryFrom(taxonomyDatabaseName);

        protected override ViewRepository<Category> ViewRepository => RepositoryFactory.CategoryFrom(taxonomyDatabaseName);

        string taxonomyDatabaseName;

        string entityDatabaseName;

        public CategoryBusiness(string taxonomyDatabaseName = null, string entityDatabaseName = null)
        {
            this.taxonomyDatabaseName = taxonomyDatabaseName;
            this.entityDatabaseName = entityDatabaseName;
        }

        public static List<Action<CategoryNode>> CategoryNodeAugmenters = new List<Action<CategoryNode>>();

        public static List<Action<List<CategoryNode>>> CategoryNodesAugmenters = new List<Action<List<CategoryNode>>>();

        private static Dictionary<string, List<CategoryNode>> entityTypeCategoriesHierarchy = new Dictionary<string, List<CategoryNode>>();

        private static List<SimpleCategoryNode> simpleCategoriesHierarchy;

        private static List<Category> categories;

        private static Dictionary<string, Category> categoriesTitleDictionary;

        public Category Get(Guid guid)
        {
            var category = GetOrNull(i => i.Guid == guid);
            return category;
        }

        public List<Category> GetList(List<Guid> guids)
        {
            var categories = GetList(i => guids.Contains(i.Guid));
            return categories;
        }

        public Category Create(string entityTypeName, Category category)
        {
            var entityTypeGuid = new EntityTypeBusiness(entityDatabaseName).GetGuid(entityTypeName);
            category.EntityTypeGuid = entityTypeGuid;
            return Create(category);
        }

        public static List<Category> Categories
        {
            get
            {
                CacheHelper.InitiliazeData(categories, () =>
                {
                    categories = RepositoryFactory.Category.All.Where(i => i.Show == true).ToList();
                    return categories;
                });
                return categories;
            }
        }

        public static Dictionary<string, Category> CategoriesTitleDictionary
        {
            get
            {
                if (categoriesTitleDictionary.IsNull())
                {
                    categoriesTitleDictionary = Categories.ToDictionary(i => i.Title, i => i);
                }
                return categoriesTitleDictionary;
            }
        }

        public static List<Category> RootCategories
        {
            get
            {
                var rootCategories = Categories.Where(i => i.ParentCategoryId.IsNull()).ToList();
                return rootCategories;
            }
        }

        public void CountItemsInCategories()
        {
            var categories = GetAll();
            foreach (var category in categories)
            {
                category.ItemsCount = new CategoryItemBusiness(taxonomyDatabaseName, entityDatabaseName).GetCountOfItemsInCategory(category);
            }
            ModelRepository.BulkUpdate(categories);
        }

        public int GetTotalCategorizedItemsCount(string entityModuleDatabaseName, string entityTypeName)
        {
            CountItemsInCategories();
            if (entityTypeName.IsSomething())
            {
                var entityTypeGuid = new EntityTypeBusiness(entityModuleDatabaseName ?? entityDatabaseName).GetGuid(entityTypeName);
                var count = ViewRepository.All.Where(i => i.EntityTypeGuid == entityTypeGuid).Sum(i => i.ItemsCount) ?? 0;
                return count;
            }
            else
            {
                var count = ViewRepository.All.Sum(i => i.ItemsCount) ?? 0;
                return count;
            }
        }

        public void CountItemsInCategory(long categoryId)
        {
            var category = Get(categoryId);
            category.ItemsCount = new CategoryItemBusiness(taxonomyDatabaseName, entityDatabaseName).GetCountOfItemsInCategory(category);
            Update(category);
        }

        public static Category GetCategory(string name)
        {
            if (CategoriesTitleDictionary.ContainsKey(name))
            {
                return CategoriesTitleDictionary[name];
            }
            return null;
        }

        public static bool HasSubcategories(long categoryId)
        {
            var hasSubcategories = Categories.Any(i => i.ParentCategoryId == categoryId);
            return hasSubcategories;
        }

        public static List<Category> GetSubcategories(long parentCategoryId)
        {
            var subcategories = Categories.Where(i => i.ParentCategoryId == parentCategoryId).ToList();
            return subcategories;
        }

        protected override void ModifyItemBeforeReturning(Category item)
        {
            item.RelatedItems.IconUrl = GetIconUrl(item);
            item.RelatedItems.HasDefaultIcon = !item.IconGuid.HasValue;
            item.RelatedItems.HasChildren = ViewRepository.All.Any(i => i.ParentCategoryId == item.Id);
            base.ModifyItemBeforeReturning(item);
        }

        private dynamic GetIconUrl(Category category)
        {
            var iconUrl = Storage.GetImageUrl(CategoryIconsContainerName, category.IconGuid.HasValue ? category.IconGuid.Value : Guid.Empty);
            return iconUrl;
        }

        public List<Category> GetList(long? parentCategoryId = null)
        {
            var categories = ViewRepository.All.Where(i => i.ParentCategoryId == parentCategoryId).ToList();
            ModifyListBeforeReturning(categories);
            return categories;
        }

        public void ChangeTitle(long id, string title)
        {
            var category = Get(id);
            category.Title = title;
            ModelRepository.Update(category);
        }

        public void ChangeDescription(long id, string description)
        {
            var category = Get(id);
            category.Description = description;
            ModelRepository.Update(category);
        }

        public List<CategoryNode> CacheAndGetHierarchy(List<Category> categories, string entityTypeName = null)
        {
            var tempEntityTypeName = entityTypeName;
            if (tempEntityTypeName.IsNull())
            {
                tempEntityTypeName = "null";
            }
            if (entityTypeCategoriesHierarchy.ContainsKey(tempEntityTypeName))
            {
                return entityTypeCategoriesHierarchy[tempEntityTypeName];
            }
            entityTypeCategoriesHierarchy.Add(tempEntityTypeName, GetHierarchy(entityTypeName, categories));
            return entityTypeCategoriesHierarchy[tempEntityTypeName];
        }

        public List<CategoryNode> CacheAndGetHierarchy(string entityTypeName = null)
        {
            var tempEntityTypeName = entityTypeName;
            if (tempEntityTypeName.IsNull())
            {
                tempEntityTypeName = "null";
            }
            if (entityTypeCategoriesHierarchy.ContainsKey(tempEntityTypeName))
            {
                return entityTypeCategoriesHierarchy[tempEntityTypeName];
            }
            entityTypeCategoriesHierarchy.Add(tempEntityTypeName, GetHierarchy(entityTypeName));
            return entityTypeCategoriesHierarchy[tempEntityTypeName];
        }

        public List<CategoryNode> GetRootCategories(string entityTypeName = null)
        {
            var allCategories = ViewRepository.All.Where(i => i.ParentCategoryId == null).ToList();
            if (entityTypeName.IsSomething())
            {
                var entityTypeGuid = new EntityTypeBusiness(entityDatabaseName).GetGuid(entityTypeName);
                allCategories = allCategories.Where(i => i.EntityTypeGuid == entityTypeGuid).ToList();
            }
            return ConvertToCategoryNodes(allCategories, null);
        }

        public List<CategoryNode> GetCategories(long? parentCategoryId = null)
        {
            var allCategories = ViewRepository.All.Where(i => i.ParentCategoryId == parentCategoryId).ToList();
            CategoryNode parentCategory = null;
            if (parentCategoryId.HasValue)
            {
                parentCategory = ConvertToCategoryNode(GetOrNull(parentCategoryId.Value), null);
            }
            return ConvertToCategoryNodes(allCategories, parentCategory);
        }

        private List<CategoryNode> ConvertToCategoryNodes(List<Category> allCategories, CategoryNode parentCategory)
        {
            var categoryNodes = allCategories.Select(i => ConvertToCategoryNode(i, parentCategory)).ToList();
            foreach (var categoryNode in categoryNodes)
            {
                foreach (var augmenter in CategoryNodeAugmenters)
                {
                    augmenter.Invoke(categoryNode);
                }
            }
            foreach (var augmenter in CategoryNodesAugmenters)
            {
                augmenter.Invoke(categoryNodes);
            }
            return categoryNodes;
        }

        public List<CategoryNode> GetHierarchy(string entityTypeName = null)
        {
            var query = ViewRepository.All;
            var allCategories = query.ToList();
            return GetHierarchy(entityTypeName, allCategories);
        }

        private List<CategoryNode> GetHierarchy(string entityTypeName, List<Category> categories)
        {
            var allCategories = categories;
            if (entityTypeName.IsSomething())
            {
                var entityTypeGuid = new EntityTypeBusiness(entityDatabaseName).GetGuid(entityTypeName);
                allCategories = allCategories.Where(i => i.EntityTypeGuid == entityTypeGuid).ToList();
            }
            var rootCategories = allCategories.Where(i => i.ParentCategoryId == null).Select(i => ConvertToCategoryNode(i, null)).ToList();
            foreach (var rootCategory in rootCategories)
            {
                foreach (var augmenter in CategoryNodeAugmenters)
                {
                    augmenter.Invoke(rootCategory);
                }
                rootCategory.Children = GetChildrenCategories(allCategories, rootCategory);
            }
            foreach (var augmenter in CategoryNodesAugmenters)
            {
                augmenter.Invoke(rootCategories);
            }
            return rootCategories;
        }

        private CategoryNode ConvertToCategoryNode(Category category, CategoryNode parentCategory)
        {
            var categoryNode = new CategoryNode();
            categoryNode.Id = category.Id;
            categoryNode.Guid = category.Guid;
            categoryNode.Title = category.Title;
            categoryNode.Description = category.Description;
            categoryNode.IconUrl = GetIconUrl(category);
            categoryNode.IconSvg = category.IconSvg;
            categoryNode.HasDefaultIcon = !category.IconGuid.HasValue;
            categoryNode.ParentCategoryId = parentCategory?.Id;
            categoryNode.ItemsCount = category.ItemsCount;
            categoryNode.UrlKey = category.UrlKey;
            categoryNode.Level = parentCategory.IsNull() ? 1 : (parentCategory.Level + 1);
            categoryNode.Children = new List<CategoryNode>();
            return categoryNode;
        }

        private List<CategoryNode> GetChildrenCategories(List<Category> allCategories, CategoryNode parentCategory)
        {
            var childrenCategories = allCategories.Where(i => i.ParentCategoryId == parentCategory.Id).Select(i => ConvertToCategoryNode(i, parentCategory)).ToList();
            foreach (var childCategory in childrenCategories)
            {
                foreach (var augmenter in CategoryNodeAugmenters)
                {
                    augmenter.Invoke(childCategory);
                }
                childCategory.Children = GetChildrenCategories(allCategories, childCategory);
            }
            return childrenCategories;
        }

        public List<SimpleCategoryNode> GetSimpleHierarchy()
        {
            if (simpleCategoriesHierarchy.IsNotNull())
            {
                return simpleCategoriesHierarchy;
            }
            var allCategories = ViewRepository.All.ToList();
            var rootCategories = allCategories.Where(i => i.ParentCategoryId == null).Select(i => new SimpleCategoryNode
            {
                Id = i.Id,
                Title = i.Title,
                ParentCategoryId = null,
                Children = new List<SimpleCategoryNode>()
            }).ToList();
            foreach (var rootCategory in rootCategories)
            {
                rootCategory.Children = GetSimpleChildrenCategories(allCategories, rootCategory);
            }
            simpleCategoriesHierarchy = rootCategories;
            return rootCategories;
        }

        private List<SimpleCategoryNode> GetSimpleChildrenCategories(List<Category> allCategories, SimpleCategoryNode parentCategory)
        {
            var childrenCategories = allCategories.Where(i => i.ParentCategoryId == parentCategory.Id).Select(i => new SimpleCategoryNode
            {
                Id = i.Id,
                Title = i.Title,
                ParentCategoryId = parentCategory.Id,
                Children = new List<SimpleCategoryNode>()
            }).ToList(); ;
            foreach (var childCategory in childrenCategories)
            {
                childCategory.Children = GetSimpleChildrenCategories(allCategories, childCategory);
            }
            return childrenCategories;
        }

        public string ChangeIcon(long categoryId, byte[] bytes)
        {
            var category = Get(categoryId);
            if (category.IconGuid.HasValue)
            {
                Storage.DeleteImage(CategoryIconsContainerName, category.IconGuid.Value);
            }
            var thumbnail = ImageHelper.MakeImageThumbnail(TaxonomyConfig.CategoryThumbnailWidth, null, bytes);
            category.IconGuid = Guid.NewGuid();
            Storage.UploadImage(thumbnail.GetBytes(), category.IconGuid.Value, CategoryIconsContainerName);
            ModelRepository.Update(category);
            return Storage.GetImageUrl(CategoryIconsContainerName, category.IconGuid.Value);
        }

        public void RemoveIcon(long categoryId)
        {
            var category = Get(categoryId);
            if (category.IconGuid.HasValue)
            {
                Storage.DeleteImage(CategoryIconsContainerName, category.IconGuid.Value);
            }
            category.IconGuid = null;
            ModelRepository.Update(category);
        }

        public override void Validate(Category model)
        {
            model.Title.Ensure().AsString().IsSomething("عنوان دسته بندی فراهم نشده است");
            if (model.ParentCategoryId.HasValue)
            {
                var parentCategory = Get(model.ParentCategoryId.Value);
                if (parentCategory.IsNull())
                {
                    throw new BusinessException("دسته بندی والد وجود ندارد");
                }
            }
            model.EntityTypeGuid.Ensure().IsNotNull().And().AsString().IsNotEmptyGuid();
        }

        protected override void BeforeCreation(Category model, object extraParameters = null)
        {
            model.Guid = Guid.NewGuid();
            model.Show = true;
        }

        public Category GetByTitle(string title)
        {
            return GetByTitle(title, null);
        }

        public Category GetByTitle(string title, long? parentCategoryId = null)
        {
            var categories = GetList(i => i.Title == title);
            if (parentCategoryId.HasValue)
            {
                categories = categories.Where(i => i.ParentCategoryId == parentCategoryId.Value).ToList();
            }
            if (categories.Count > 1)
            {
                throw new BusinessException($"بیش از یک دسته بندی با عنوان {title} یافت شد");
            }
            return categories.FirstOrDefault();
        }

        public Category GetByCode(string code)
        {
            var categories = GetList(i => i.Code == code);
            if (categories.Count > 1)
            {
                throw new BusinessException($"بیش از یک دسته بندی با کد {code} یافت شد");
            }
            return categories.FirstOrDefault();
        }

        public Category GetByUrlKey(string urlKey)
        {
            var categories = GetList(i => i.UrlKey == urlKey);
            if (categories.Count > 1)
            {
                throw new BusinessException($"بیش از یک دسته بندی با کلید URL {urlKey} یافت شد");
            }
            return categories.FirstOrDefault();
        }

        public void UpdateIsLeaf()
        {
            var categories = GetAll();
            foreach (var category in categories)
            {
                category.IsLeaf = GetList(i => i.ParentCategoryId == category.Id).Count == 0;
                Update(category);
            }
        }

        public List<Guid> OrderGuids(List<Guid> guids)
        {
            var categories = GetCategoriesInOrder(guids);
            var orderedGuids = categories.Select(i => i.Guid).ToList();
            return orderedGuids;
        }

        public List<Category> GetCategoriesInOrder(List<Guid> guids)
        {
            var categories = GetList(guids);
            var rootCategories = categories.Where(i => i.ParentCategoryId == null || !categories.Any(x => i.ParentCategoryId == x.Id)).ToList();
            var orderedIds = new List<long>();
            orderedIds.AddRange(rootCategories.Select(i => i.Id).OrderBy(i => i).ToList());
            foreach (var rootCategory in rootCategories)
            {
                AddChildCategories(rootCategory, categories, orderedIds);
            }
            categories = categories.OrderBy(i => orderedIds.IndexOf(i.Id)).ToList();
            return categories;
        }

        private void AddChildCategories(Category rootCategory, List<Category> categories, List<long> orderedIds)
        {
            var childCategories = categories.Where(i => i.ParentCategoryId == rootCategory.Id).OrderBy(i => i.Id).ToList();
            orderedIds.AddRange(childCategories.Select(i => i.Id).OrderBy(i => i).ToList());
            foreach (var childCategory in childCategories)
            {
                AddChildCategories(childCategory, categories, orderedIds);
            }
        }
    }
}
