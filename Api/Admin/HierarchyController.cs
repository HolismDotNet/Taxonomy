namespace Taxonomy;

public class HierarchyController : TreeController<HierarchyView, Hierarchy>
{
    public override ReadBusiness<HierarchyView> ReadBusiness => new HierarchyBusiness();

    public override TreeBusiness<HierarchyView, Hierarchy> Business => new HierarchyBusiness();
}