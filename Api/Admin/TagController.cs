namespace Taxonomy;

public class TagController : Controller<Tag, Tag>
{
    public override ReadBusiness<Tag> ReadBusiness => new TagBusiness();

    public override Business<Tag, Tag> Business => new TagBusiness();

    public override Action<Tag, UpsertMode> PreUpsertion => (tag, upsertMode) =>
    {
        var entityType = HttpContext.ExtractProperty("entityType");
        tag.EntityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
    };
}