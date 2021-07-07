namespace Holism.Taxonomy.DataAccess
{
    public class RepositoryFactory
    {
        public static Repositories.CategoryRepository Category
        {
            get
            {
                return new Repositories.CategoryRepository();
            }
        }

        public static Repositories.CategoryRepository CategoryFrom(string databaseName = null)
        {
            return new Repositories.CategoryRepository(databaseName);
        }

        public static Repositories.CategoryItemRepository CategoryItem
        {
            get
            {
                return new Repositories.CategoryItemRepository();
            }
        }

        public static Repositories.CategoryItemRepository CategoryItemFrom(string databaseName = null)
        {
            return new Repositories.CategoryItemRepository(databaseName);
        }

        public static Repositories.Views.CategoryItemViewRepository CategoryItemView
        {
            get
            {
                return new Repositories.Views.CategoryItemViewRepository();
            }
        }

        public static Repositories.Views.CategoryItemViewRepository CategoryItemViewFrom(string databaseName = null)
        {
            return new Repositories.Views.CategoryItemViewRepository(databaseName);
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
