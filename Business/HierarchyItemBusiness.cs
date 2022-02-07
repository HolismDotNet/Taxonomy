namespace Taxonomy;

public class HierarchyItemBusiness : Business<HierarchyItemView, HierarchyItem>
{
    protected override ReadRepository<HierarchyItemView> ReadRepository => Repository.HierarchyItemView;

    protected override Repository<HierarchyItem> WriteRepository => Repository.HierarchyItem;
}