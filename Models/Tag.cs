namespace Taxonomy;

public class Tag : IGuidEntity
{
    public Tag()
    {
        RelatedItems = new ExpandoObject();
    }

    public long Id { get; set; }

    public Guid Guid { get; set; }

    public Guid EntityTypeGuid { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public Guid? IconGuid { get; set; }

    public string IconSvg { get; set; }

    public string Description { get; set; }

    public int? Order { get; set; }

    public bool? Show { get; set; }

    public int? ItemsCount { get; set; }

    public string Slug { get; set; }

    public dynamic RelatedItems { get; set; }
}
