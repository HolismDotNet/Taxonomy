using Holism.Taxonomy.Models;
using Holism.DataAccess;

namespace Holism.Taxonomy.DataAccess
{
    public class Repository
    {
        public static Repository<Hierarchy> Hierarchy
        {
            get
            {
                return new Holism.DataAccess.Repository<Hierarchy
                >(new TaxonomyContext());
            }
        }

        public static Repository<HierarchyItem> HierarchyItem
        {
            get
            {
                return new Holism.DataAccess.Repository<HierarchyItem
                >(new TaxonomyContext());
            }
        }

        public static Repository<Tag> Tag
        {
            get
            {
                return new Holism.DataAccess.Repository<Tag
                >(new TaxonomyContext());
            }
        }

        public static Repository<TagItem> TagItem
        {
            get
            {
                return new Holism.DataAccess.Repository<TagItem
                >(new TaxonomyContext());
            }
        }

        public static Repository<TagItemView> TagItemView
        {
            get
            {
                return new Holism.DataAccess.Repository<TagItemView
                >(new TaxonomyContext());
            }
        }

        public static Repository<HierarchyItemView> HierarchyItemView
        {
            get
            {
                return new Holism.DataAccess.Repository<HierarchyItemView
                >(new TaxonomyContext());
            }
        }
    }
}
