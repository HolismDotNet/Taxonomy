namespace Taxonomy;

public class HierarchyBusiness : TreeBusiness<HierarchyView, Hierarchy>
{
    protected override Read<HierarchyView> Read => Repository.HierarchyView;

    protected override Write<Hierarchy> Write => Repository.Hierarchy;

    public List<HierarchyView> GetTree(string entityType)
    {
        var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
        var hierarchyViews = GetList(i => i.EntityTypeGuid == entityTypeGuid);
        return GetTree(hierarchyViews);
    }

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

    public void SetImage(long hierarchyId, Guid imageGuid)
    {
        var hierarchy = Write.Get(hierarchyId);
        hierarchy.ImageGuid = imageGuid;
        Update(hierarchy);
    }
}