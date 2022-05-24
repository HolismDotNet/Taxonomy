namespace Taxonomy;

public class EntityHierarchy : IEntity
{
    public EntityHierarchy()
    {
        RelatedItems = new ExpandoObject();
    }

    public long Id { get; set; }

    public Guid EntityGuid { get; set; }

    public long HierarchyId { get; set; }

    public dynamic RelatedItems { get; set; }
}
