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
        var tags = new TagBusiness().GetEntityTypeTags(entityType);
        return tags;
    }

    public override Action<Tag, UpsertMode> PreUpsertion => (tag, upsertMode) =>
    {
        var entityType = HttpContext.ExtractProperty("entityType");
        tag.EntityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
    };

    [FileUploadChecker]
    [HttpPost]
    public Tag SetImage(IFormFile file)
    {
        var tagId = Request.Query["tagId"];
        if (tagId.Count == 0)
        {
            throw new ClientException("Please provide tagId");
        }
        var bytes = file.OpenReadStream().GetBytes();
        var tag = new TagBusiness().ChangeIcon(tagId[0].ToLong(), bytes);
        return tag;
    }
}