namespace Taxonomy;

public class TagItem : IEntity, IOrder
{
    public TagItem()
    {
        RelatedItems = new ExpandoObject();
    }

    public long Id { get; set; }

    public long TagId { get; set; }

    public Guid EntityGuid { get; set; }

    public long? Order { get; set; }

    public dynamic RelatedItems { get; set; }
}
