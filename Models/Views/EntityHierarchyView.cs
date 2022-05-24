namespace Taxonomy;

public class EntityHierarchyView : IEntity
{
    public EntityHierarchyView()
    {
        RelatedItems = new ExpandoObject();
    }

    public long Id { get; set; }

    public Guid EntityGuid { get; set; }

    public long HierarchyId { get; set; }

    public dynamic RelatedItems { get; set; }
}
