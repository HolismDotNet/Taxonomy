namespace Taxonomy;

public class HierarchyBusiness : TreeBusiness<HierarchyView, Hierarchy>
{
    protected override Read<HierarchyView> Read => Repository.HierarchyView;

    protected override Write<Hierarchy> Write => Repository.Hierarchy;
}