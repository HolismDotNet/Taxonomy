namespace Taxonomy;

public class HierarchyBusiness : TreeBusiness<HierarchyView, Hierarchy>
{
    protected override Read<HierarchyView> Read => Repository.HierarchyView;

    protected override Write<Hierarchy> Write => Repository.Hierarchy;

    public HierarchyView Create(string entityType, string title, long? parentId)
    {
        var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
        var hierarchy = new Hierarchy();
        hierarchy.EntityTypeGuid = entityTypeGuid;
        hierarchy.Title = title;
        hierarchy.ParentId = parentId;
        Create(hierarchy);
        return Get(hierarchy.Id);
    }

    public HierarchyView UpdateTitle(Guid hierarchyGuid, string title)
    {
        var hierarchy = Write.Get(i => i.Guid == hierarchyGuid);
        hierarchy.Title = title;
        Update(hierarchy);
        return Get(hierarchy.Id);
    }
}