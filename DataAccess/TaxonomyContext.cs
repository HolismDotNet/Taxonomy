namespace Taxonomy;

public class TaxonomyContext : DatabaseContext
{
    public override string ConnectionStringName => "Taxonomy";

    public DbSet<Taxonomy.EntityHierarchy> EntityHierarchies { get; set; }

    public DbSet<Taxonomy.EntityHierarchyView> EntityHierarchyViews { get; set; }

    public DbSet<Taxonomy.EntityTag> EntityTags { get; set; }

    public DbSet<Taxonomy.EntityTagView> EntityTagViews { get; set; }

    public DbSet<Taxonomy.Hierarchy> Hierarchies { get; set; }

    public DbSet<Taxonomy.HierarchyView> HierarchyViews { get; set; }

    public DbSet<Taxonomy.Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
