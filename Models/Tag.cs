namespace Taxonomy;

public class Tag : IEntity, IGuid, ISlug, IKey, IOrder
{
    public Tag()
    {
        RelatedItems = new ExpandoObject();
    }

    public long Id { get; set; }

    public Guid Guid { get; set; }

    public Guid EntityTypeGuid { get; set; }

    public string Name { get; set; }

    public Guid? ImageGuid { get; set; }

    public Guid? IconGuid { get; set; }

    public string IconSvg { get; set; }

    public string Description { get; set; }

    public bool? IsActive { get; set; }

    public int? ItemsCount { get; set; }

    public string Key { get; set; }

    public long Order { get; set; }

    public string Slug { get; set; }

    public dynamic RelatedItems { get; set; }
}
