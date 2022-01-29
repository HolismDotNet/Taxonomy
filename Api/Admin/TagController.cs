namespace Taxonomy;

public class TagController : Controller<Tag, Tag>
{
    public override ReadBusiness<Tag> ReadBusiness => new TagBusiness();

    public override Business<Tag, Tag> Business => new TagBusiness();

    [BindProperty]
    public string EntityType { get; set; }

    public override Action<ListParameters> ListParametersAugmenter => listParameters => 
    {
        if (EntityType.IsNothing())
        {
            throw new ClientException("EntityType is not provided");
        }
        var entityTypeGuid = new EntityTypeBusiness().GetGuid(EntityType);
        listParameters.AddFilter<Tag>(i => i.EntityTypeGuid, entityTypeGuid);
    };

    public override Action<Tag, UpsertMode> PreUpsertion => (tag, upsertMode) =>
    {
        var entityType = HttpContext.ExtractProperty("entityType");
        tag.EntityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
    };
}