namespace Holism.Taxonomy.Models;

public class HierarchyItem : IEntity
{
    public HierarchyItem()
    {
        RelatedItems = new ExpandoObject();
    }

    public long Id { get; set; }

    public long HierarchyId { get; set; }

    public Guid EntityGuid { get; set; }

    public int? Order { get; set; }

    public dynamic RelatedItems { get; set; }
}
