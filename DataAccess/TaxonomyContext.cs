namespace Taxonomy;

public class TaxonomyContext : DatabaseContext
{
    public override string ConnectionStringName => "Taxonomy";

    public DbSet<Hierarchy> Hierarchies { get; set; }

    public DbSet<HierarchyItem> HierarchyItems { get; set; }

    public DbSet<HierarchyItemView> HierarchyItemViews { get; set; }

    public DbSet<TagItem> TagItems { get; set; }

    public DbSet<TagItemView> TagItemViews { get; set; }

    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
