namespace Holism.Taxonomy.Models;

public class TagItem : IEntity
{
    public TagItem()
    {
        RelatedItems = new ExpandoObject();
    }

    public long Id { get; set; }

    public long TagId { get; set; }

    public Guid EntityGuid { get; set; }

    public int? Order { get; set; }

    public dynamic RelatedItems { get; set; }
}
