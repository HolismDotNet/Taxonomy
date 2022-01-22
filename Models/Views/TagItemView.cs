namespace Taxonomy;

public class TagItemView : IEntity
{
    public TagItemView()
    {
        RelatedItems = new ExpandoObject();
    }

    public long Id { get; set; }

    public long TagId { get; set; }

    public Guid EntityGuid { get; set; }

    public int? Order { get; set; }

    public dynamic RelatedItems { get; set; }
}
