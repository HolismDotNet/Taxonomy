namespace Taxonomy;

public class HierarchyController : TreeController<HierarchyView, Hierarchy>
{
    public override ReadBusiness<HierarchyView> ReadBusiness => new HierarchyBusiness();

    public override TreeBusiness<HierarchyView, Hierarchy> Business => new HierarchyBusiness();

    [BindProperty(SupportsGet = true)]
    public string EntityType { get; set; }

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