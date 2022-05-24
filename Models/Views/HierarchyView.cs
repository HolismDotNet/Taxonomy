namespace Taxonomy;

public class HierarchyView : IEntity, IGuid, ISlug, IKey, IOrder, IParent
{
    public HierarchyView()
    {
        RelatedItems = new ExpandoObject();
    }

    public long Id { get; set; }

    public Guid Guid { get; set; }

    public Guid EntityTypeGuid { get; set; }

    public string Title { get; set; }

    public Guid? ImageGuid { get; set; }

    public Guid? IconGuid { get; set; }

    public string IconSvg { get; set; }

    public long? ParentId { get; set; }

    public string Description { get; set; }

    public bool? IsActive { get; set; }

    public int? ItemsCount { get; set; }

    public int? Level { get; set; }

    public bool? IsLeaf { get; set; }

    public string Key { get; set; }

    public long Order { get; set; }

    public string Slug { get; set; }

    public string Path { get; set; }

    public dynamic RelatedItems { get; set; }
}
