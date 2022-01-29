namespace Taxonomy;

public class TagController : Controller<Tag, Tag>
{
    public override ReadBusiness<Tag> ReadBusiness => new TagBusiness();

    public override Business<Tag, Tag> Business => new TagBusiness();
}