namespace Taxonomy;

public class HierarchyItemBusiness : Business<HierarchyItemView, HierarchyItem>
{
    protected override Read<HierarchyItemView> Read => Repository.HierarchyItemView;

    protected override Write<HierarchyItem> Write => Repository.HierarchyItem;
}