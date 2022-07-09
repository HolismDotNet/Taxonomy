namespace Taxonomy;

public class TagController : Controller<Tag, Tag>
{
    public override ReadBusiness<Tag> ReadBusiness => new TagBusiness();

    public override Business<Tag, Tag> Business => new TagBusiness();

    [BindProperty(SupportsGet = true)]
    public string EntityType { get; set; }

    [HttpGet]
    public List<Tag> EntityTypeTags(string entityType)
    {
        var tags = new TagBusiness().GetTags(entityType);
        return tags;
    }

    public override Action<Tag, UpsertMode> PreUpsertion => (tag, upsertMode) =>
    {
        var entityType = HttpContext.ExtractProperty("entityType").ToString();
        tag.EntityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
    };
}