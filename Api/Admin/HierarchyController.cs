namespace Taxonomy;

public class HierarchyController : TreeController<HierarchyView, Hierarchy>
{
    public override ReadBusiness<HierarchyView> ReadBusiness => new HierarchyBusiness();

    public override TreeBusiness<HierarchyView, Hierarchy> Business => new HierarchyBusiness();

    [BindProperty(SupportsGet = true)]
    public string EntityType { get; set; }

    public override Action<ListParameters> ListParametersAugmenter => listParameters =>
    {
        if (EntityType.IsNothing())
        {
            throw new ClientException("EntityType is not provided");
        }
        var entityTypeGuid = new EntityTypeBusiness().GetGuid(EntityType);
        listParameters.AddFilter<HierarchyView>(i => i.EntityTypeGuid, entityTypeGuid);
    };

    public override Action<Hierarchy> PreCreation => model =>
    {
        if (EntityType.IsNothing())
        {
            throw new ClientException("EntityType is not provided");
        }
        var entityTypeGuid = new EntityTypeBusiness().GetGuid(EntityType);
        model.EntityTypeGuid = entityTypeGuid;
    };
}