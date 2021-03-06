namespace Taxonomy;

public class TagBusiness : Business<Tag, Tag>
{
    public const string TagIconsContainerName = "tagicons";

    protected override Read<Tag> Read => Repository.Tag;

    protected override Write<Tag> Write => Repository.Tag;

    public Tag Create(string entityType, Tag tag)
    {
        var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
        tag.EntityTypeGuid = entityTypeGuid;
        tag.Guid = Guid.NewGuid();
        return Create(tag);
    }

    public List<Tag> GetTags(string entityType)
    {
        if (entityType.IsNothing())
        {
            throw new ClientException("EntityType is not provided");
        }
        var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
        var tags = GetList(i => i.EntityTypeGuid == entityTypeGuid);
        return tags;
    }

    public void CountItemsInTags()
    {
        var tags = GetAll();
        foreach (var tag in tags)
        {
            tag.ItemsCount = new EntityTagBusiness().GetCountOfItemsInTag(tag);
        }
        Write.BulkUpdate(tags);
    }

    public int GetTotalTaggedItemsCount(string entityType)
    {
        var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
        CountItemsInTags();
        var count = Read.All.Where(i => i.EntityTypeGuid == entityTypeGuid).Sum(i => i.ItemsCount) ?? 0;
        return count;
    }

    public void CountItemsInTag(long tagId)
    {
        var tag = Get(tagId);
        tag.ItemsCount = new EntityTagBusiness().GetCountOfItemsInTag(tag);
        Update(tag);
    }

    public Tag GetTag(string name)
    {
        var tag = Get(i => i.Name == name);
        return tag;
    }

    protected override void ModifyItemBeforeReturning(Tag item)
    {
        item.RelatedItems.IconUrl = GetIconUrl(item);
        item.RelatedItems.HasDefaultIcon = !item.IconGuid.HasValue;
        base.ModifyItemBeforeReturning(item);
    }

    private dynamic GetIconUrl(Tag tag)
    {
        var iconUrl = Storage.GetImageUrl(TagIconsContainerName, tag.IconGuid.HasValue ? tag.IconGuid.Value : Guid.Empty);
        return iconUrl;
    }

    public void ChangeName(long id, string name)
    {
        var tag = Get(id);
        tag.Name = name;
        Write.Update(tag);
    }

    public void ChangeDescription(long id, string description)
    {
        var tag = Get(id);
        tag.Description = description;
        Write.Update(tag);
    }

    public override void Validate(Tag model)
    {
        model.Name.Ensure().IsSomething("Name is not provided");
        model.EntityTypeGuid.Ensure().IsNotEmpty();
    }

    protected override void PreCreation(Tag model)
    {
        model.IsActive = true;
    }

    public Tag GetByName(string name)
    {
        var tags = GetList(i => i.Name == name);
        if (tags.Count > 1)
        {
            throw new ClientException($"No tag is found with name {name}");
        }
        return tags.FirstOrDefault();
    }

    public Tag GetOrCreateAndGet(string entityType, string name)
    {
        var tags = GetList(i => i.Name == name);
        if (tags.Count > 1)
        {
            throw new ClientException($"No tag is found with name {name}");
        }
        if (tags.Count == 1)
        {
            return tags.FirstOrDefault();
        }
        var tag = new Tag();
        tag.Name = name;
        tag.Slug = name.Kebaberize();
        Create(entityType, tag);
        return tag;
    }
}
