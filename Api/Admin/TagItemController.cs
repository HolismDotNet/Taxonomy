namespace Taxonomy;

public class EntityTagController : Controller<EntityTagView, EntityTag>
{
    public override ReadBusiness<EntityTagView> ReadBusiness => new EntityTagBusiness();

    public override Business<EntityTagView, EntityTag> Business => new EntityTagBusiness();

    [BindProperty(SupportsGet = true)]
    public Guid? EntityGuid { get; set; }

    public override Action<ListParameters> ListParametersAugmenter => listParameters => 
    {
        if (!EntityGuid.HasValue)
        {
            throw new ClientException("EntityGuid is not provided");
        }
        listParameters.AddFilter<EntityTagView>(i => i.EntityGuid, EntityGuid);
    };

    [HttpPost]
    public IActionResult PutInTags(List<Guid> tagGuids)
    {
        if (!EntityGuid.HasValue)
        {
            throw new ClientException("EntityGuid is not provided");
        }
        new EntityTagBusiness().UpsertTags(EntityGuid.Value, tagGuids);
        return OkJson();
    }
}