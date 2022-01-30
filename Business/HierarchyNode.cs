namespace Taxonomy;

public class HierarchyNode
{
    public HierarchyNode()
    {
        RelatedItems = new System.Dynamic.ExpandoObject();
    }

    public long Id { get; set; }

    public Guid Guid { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string IconUrl { get; set; }

    public string IconSvg { get; set; }

    public bool HasDefaultIcon { get; set; }

    public bool HasChildren
    {
        get
        {
            return Children.Count > 0;
        }
    }

    public long? ParentId { get; set; }

    public int? ItemsCount { get; set; }

    public string Slug { get; set; }

    public int Level { get; set; }

    public List<HierarchyNode> Children { get; set; }

    public dynamic RelatedItems { get; set; }
}
