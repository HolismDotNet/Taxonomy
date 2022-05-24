namespace Taxonomy;

public class EntityTag : IEntity
{
    public EntityTag()
    {
        RelatedItems = new ExpandoObject();
    }

    public long Id { get; set; }

    public Guid EntityGuid { get; set; }

    public long TagId { get; set; }

    public dynamic RelatedItems { get; set; }
}
