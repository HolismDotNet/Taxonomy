namespace Taxonomy;

public class SimpleHierarchyNode
{
    public long Id { get; set; }

    public string Title { get; set; }

    public long? ParentId { get; set; }

    public List<SimpleHierarchyNode> Children { get; set; }
}
