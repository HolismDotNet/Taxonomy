namespace Taxonomy;

public class TagItemController : Controller<TagItemView, TagItem>
{
    public override ReadBusiness<TagItemView> ReadBusiness => new TagItemBusiness();

    public override Business<TagItemView, TagItem> Business => new TagItemBusiness();

    [BindProperty]
    public string EntityType { get; set; }

    [BindProperty]
    public Guid? EntityGuid { get; set; }

    public override Action<ListParameters> ListParametersAugmenter => listParameters => 
    {
        if (EntityType.IsNothing() || !EntityGuid.HasValue)
        {
            throw new ClientException("Invalid parameters");
        }
        listParameters.AddFilter<TagItemView>(i => i.EntityGuid, EntityGuid);
    };
}