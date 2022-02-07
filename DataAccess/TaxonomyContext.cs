namespace Taxonomy;

public class TaxonomyContext : DatabaseContext
{
    public override string ConnectionStringName => "Taxonomy";

    public DbSet<Taxonomy.Hierarchy> Hierarchies { get; set; }

    public DbSet<Taxonomy.HierarchyItem> HierarchyItems { get; set; }

    public DbSet<Taxonomy.HierarchyItemView> HierarchyItemViews { get; set; }

    public DbSet<Taxonomy.HierarchyView> HierarchyViews { get; set; }

    public DbSet<Taxonomy.TagItem> TagItems { get; set; }

    public DbSet<Taxonomy.TagItemView> TagItemViews { get; set; }

    public DbSet<Taxonomy.Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
