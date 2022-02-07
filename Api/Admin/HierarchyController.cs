namespace Taxonomy;

public class HierarchyController : TreeController<HierarchyView, Hierarchy>
{
    public override TreeBusiness<HierarchyView, Hierarchy> Business => new HierarchyBusiness();
}