namespace Taxonomy;

public class TagItemView : IEntity, IOrder
{
    public TagItemView()
    {
        RelatedItems = new ExpandoObject();
    }

    public long Id { get; set; }

    public long TagId { get; set; }

    public Guid EntityGuid { get; set; }

    public long? Order { get; set; }

    public string TagName { get; set; }

    public Guid TagGuid { get; set; }

    public dynamic RelatedItems { get; set; }
}
