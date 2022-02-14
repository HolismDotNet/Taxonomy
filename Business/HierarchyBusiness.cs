namespace Taxonomy;

public class HierarchyBusiness : TreeBusiness<HierarchyView, Hierarchy>
{
    protected override Read<HierarchyView> Read => Repository.HierarchyView;

    protected override Write<Hierarchy> Write => Repository.Hierarchy;

    public HierarchyView Create(string entityType, string title)
    {
        var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
        var hierarchy = new Hierarchy();
        hierarchy.EntityTypeGuid = entityTypeGuid;
        hierarchy.Title = title;
        Create(hierarchy);
        return Get(hierarchy.Id);
    }
}