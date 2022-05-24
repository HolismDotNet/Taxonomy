namespace Taxonomy;

public class Repository
{
    public static Write<Taxonomy.EntityHierarchy> EntityHierarchy
    {
        get
        {
            return new Write<Taxonomy.EntityHierarchy>(new TaxonomyContext());
        }
    }

    public static Write<Taxonomy.EntityHierarchyView> EntityHierarchyView
    {
        get
        {
            return new Write<Taxonomy.EntityHierarchyView>(new TaxonomyContext());
        }
    }

    public static Write<Taxonomy.EntityTag> EntityTag
    {
        get
        {
            return new Write<Taxonomy.EntityTag>(new TaxonomyContext());
        }
    }

    public static Write<Taxonomy.EntityTagView> EntityTagView
    {
        get
        {
            return new Write<Taxonomy.EntityTagView>(new TaxonomyContext());
        }
    }

    public static Write<Taxonomy.Hierarchy> Hierarchy
    {
        get
        {
            return new Write<Taxonomy.Hierarchy>(new TaxonomyContext());
        }
    }

    public static Write<Taxonomy.HierarchyView> HierarchyView
    {
        get
        {
            return new Write<Taxonomy.HierarchyView>(new TaxonomyContext());
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
