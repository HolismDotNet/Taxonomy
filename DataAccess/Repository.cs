namespace Taxonomy;

public class Repository
{
    public static Repository<Taxonomy.Hierarchy> Hierarchy
    {
        get
        {
            return new Repository<Taxonomy.Hierarchy>(new TaxonomyContext());
        }
    }

    public static Repository<Taxonomy.HierarchyItem> HierarchyItem
    {
        get
        {
            return new Repository<Taxonomy.HierarchyItem>(new TaxonomyContext());
        }
    }

    public static Repository<Taxonomy.HierarchyItemView> HierarchyItemView
    {
        get
        {
            return new Repository<Taxonomy.HierarchyItemView>(new TaxonomyContext());
        }
    }

    public static Repository<Taxonomy.TagItem> TagItem
    {
        get
        {
            return new Repository<Taxonomy.TagItem>(new TaxonomyContext());
        }
    }

    public static Repository<Taxonomy.TagItemView> TagItemView
    {
        get
        {
            return new Repository<Taxonomy.TagItemView>(new TaxonomyContext());
        }
    }

    public static Repository<Taxonomy.Tag> Tag
    {
        get
        {
            return new Repository<Taxonomy.Tag>(new TaxonomyContext());
        }
    }
}
