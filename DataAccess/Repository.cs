namespace Holism.Taxonomy.DataAccess;

public class Repository
{
    public static Repository<Hierarchy> Hierarchy
    {
        get
        {
            return new Repository<Hierarchy>(new TaxonomyContext());
        }
    }

    public static Repository<HierarchyItem> HierarchyItem
    {
        get
        {
            return new Repository<HierarchyItem>(new TaxonomyContext());
        }
    }

    public static Repository<HierarchyItemView> HierarchyItemView
    {
        get
        {
            return new Repository<HierarchyItemView>(new TaxonomyContext());
        }
    }

    public static Repository<TagItem> TagItem
    {
        get
        {
            return new Repository<TagItem>(new TaxonomyContext());
        }
    }

    public static Repository<TagItemView> TagItemView
    {
        get
        {
            return new Repository<TagItemView>(new TaxonomyContext());
        }
    }    public static Repository<Tag> Tag
    {
        get
        {
            return new Repository<Tag>(new TaxonomyContext());
        }
    }


}
