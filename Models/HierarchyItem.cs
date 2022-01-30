namespace Taxonomy;

public class HierarchyItem : IEntity, IOrder
{
    public HierarchyItem()
    {
        RelatedItems = new ExpandoObject();
    }

    public long Id { get; set; }

    public long HierarchyId { get; set; }

    public Guid EntityGuid { get; set; }

    public long? Order { get; set; }

    public dynamic RelatedItems { get; set; }
}
