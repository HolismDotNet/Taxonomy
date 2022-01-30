namespace Taxonomy;

public class TagItemController : Controller<TagItemView, TagItem>
{
    public override ReadBusiness<TagItemView> ReadBusiness => new TagItemBusiness();

    public override Business<TagItemView, TagItem> Business => new TagItemBusiness();

    [BindProperty(SupportsGet = true)]
    public Guid? EntityGuid { get; set; }

    public override Action<ListParameters> ListParametersAugmenter => listParameters => 
    {
        if (!EntityGuid.HasValue)
        {
            throw new ClientException("EntityGuid is not provided");
        }
        listParameters.AddFilter<TagItemView>(i => i.EntityGuid, EntityGuid);
    };

    [HttpPost]
    public IActionResult PutInTags(List<Guid> tagGuids)
    {
        if (!EntityGuid.HasValue)
        {
            throw new ClientException("EntityGuid is not provided");
        }
        new TagItemBusiness().UpsertTags(EntityGuid.Value, tagGuids);
        return OkJson();
    }
}