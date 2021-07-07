namespace Holism.Taxonomy.DataAccess
{
    public class RepositoryFactory
    {
        public static Repositories.HierarchyRepository Hierarchy
        {
            get
            {
                return new Repositories.HierarchyRepository();
            }
        }

        public static Repositories.HierarchyRepository HierarchyFrom(string databaseName = null)
        {
            return new Repositories.HierarchyRepository(databaseName);
        }

        public static Repositories.HierarchyItemRepository HierarchyItem
        {
            get
            {
                return new Repositories.HierarchyItemRepository();
            }
        }

        public static Repositories.HierarchyItemRepository HierarchyItemFrom(string databaseName = null)
        {
            return new Repositories.HierarchyItemRepository(databaseName);
        }

        public static Repositories.Views.HierarchyItemViewRepository HierarchyItemView
        {
            get
            {
                return new Repositories.Views.HierarchyItemViewRepository();
            }
        }

        public static Repositories.Views.HierarchyItemViewRepository HierarchyItemViewFrom(string databaseName = null)
        {
            return new Repositories.Views.HierarchyItemViewRepository(databaseName);
        }

        public static Repositories.TagRepository Tag
        {
            get
            {
                return new Repositories.TagRepository();
            }
        }

        public static Repositories.TagRepository TagFrom(string databaseName = null)
        {
            return new Repositories.TagRepository(databaseName);
        }

        public static Repositories.TagItemRepository TagItem
        {
            get
            {
                return new Repositories.TagItemRepository();
            }
        }

        public static Repositories.TagItemRepository TagItemFrom(string databaseName = null)
        {
            return new Repositories.TagItemRepository(databaseName);
        }

        public static Repositories.Views.TagItemViewRepository TagItemView
        {
            get
            {
                return new Repositories.Views.TagItemViewRepository();
            }
        }

        public static Repositories.Views.TagItemViewRepository TagItemViewFrom(string databaseName = null)
        {
            return new Repositories.Views.TagItemViewRepository(databaseName);
        }
    }
}
