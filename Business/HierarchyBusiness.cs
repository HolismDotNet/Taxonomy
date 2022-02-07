namespace Taxonomy;

public class HierarchyBusiness : TreeBusiness<HierarchyView, Hierarchy>
{
    protected override ReadRepository<HierarchyView> ReadRepository => Repository.HierarchyView;

    protected override Repository<Hierarchy> WriteRepository => Repository.Hierarchy;
}