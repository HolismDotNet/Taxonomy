namespace Taxonomy;

public class Repository
{
    public static Write<Taxonomy.Hierarchy> Hierarchy
    {
        get
        {
            return new Write<Taxonomy.Hierarchy>(new TaxonomyContext());
        }
    }

    public static Write<Taxonomy.HierarchyItem> HierarchyItem
    {
        get
        {
            return new Write<Taxonomy.HierarchyItem>(new TaxonomyContext());
        }
    }

    public static Write<Taxonomy.HierarchyItemView> HierarchyItemView
    {
        get
        {
            return new Write<Taxonomy.HierarchyItemView>(new TaxonomyContext());
        }
    }

    public static Write<Taxonomy.HierarchyView> HierarchyView
    {
        get
        {
            return new Write<Taxonomy.HierarchyView>(new TaxonomyContext());
        }
    }

    public static Write<Taxonomy.TagItem> TagItem
    {
        get
        {
            return new Write<Taxonomy.TagItem>(new TaxonomyContext());
        }
    }

    public static Write<Taxonomy.TagItemView> TagItemView
    {
        get
        {
            return new Write<Taxonomy.TagItemView>(new TaxonomyContext());
        }
    }

    public static Write<Taxonomy.Tag> Tag
    {
        get
        {
            return new Write<Taxonomy.Tag>(new TaxonomyContext());
        }
    }
}
