namespace Taxonomy;

public class HierarchyView : IEntity, IParent
{
    public HierarchyView()
    {
        RelatedItems = new ExpandoObject();
    }

    public long Id { get; set; }

    public long? ParentId { get; set; }

    public string Title { get; set; }

    public string Path { get; set; }

    public dynamic RelatedItems { get; set; }
}
