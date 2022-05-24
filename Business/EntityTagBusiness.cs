namespace Taxonomy;

public class EntityTagBusiness : Business<EntityTagView, EntityTag>
{
    protected override Write<EntityTag> Write => Repository.EntityTag;

    protected override Read<EntityTagView> Read => Repository.EntityTagView;

    private static Dictionary<Guid, Func<List<Guid>, Dictionary<Guid, object>>> entitiesInfoAugmenter = new Dictionary<Guid, Func<List<Guid>, Dictionary<Guid, object>>>();

    public static Action<EntityTag> OnTagToggled;

    public void RegisterEntityInfoAugmenter(string entityType, Func<List<Guid>, Dictionary<Guid, object>> augmenter)
    {
        var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
        if (entitiesInfoAugmenter.ContainsKey(entityTypeGuid))
        {
            entitiesInfoAugmenter[entityTypeGuid] = augmenter;
        }
        else
        {
            entitiesInfoAugmenter.Add(entityTypeGuid, augmenter);
        }
    }

    public List<EntityTagView> GetItemTags(string entityType, Guid entityGuid)
    {
       var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
       var entityTags = Read.All
        .Where(i => i.EntityGuid == entityGuid)
        .ToList();
        return entityTags;
    }

    public int GetCountOfItemsInTag(Tag tag)
    {
        var count = Read.All.Count(i => i.TagId == tag.Id);
        return count;
    }

    public List<EntityTagView> GetAllItems(long tagId)
    {
        var tag = new TagBusiness().Get(tagId);
        var allItems = Read.All.Where(i => i.TagId == tagId).ToList();
        if (entitiesInfoAugmenter.ContainsKey(tag.EntityTypeGuid))
        {
            var entityGuids = allItems.Select(i => i.EntityGuid).ToList();
            var entityInfoList = entitiesInfoAugmenter[tag.EntityTypeGuid](entityGuids);
            var entityTagsWithEntityInfo = allItems.Where(i => entityInfoList.ContainsKey(i.EntityGuid)).ToList();
            foreach (var entityTag in entityTagsWithEntityInfo)
            {
                ExpandoObjectExtensions.AddProperty(entityTag.RelatedItems, new EntityTypeBusiness().GetName(tag.EntityTypeGuid), entityInfoList[entityTag.EntityGuid]);
            }
        }
        return allItems;
    }

    public ListResult<Guid> GetEntityGuids(long tagId, int pageNumber, List<Guid> excludedEntityGuids)
    {
        CheckExcludedEntitiesCount(excludedEntityGuids);
        var listParameters = ListParameters.Create();
        listParameters.PageNumber = pageNumber;
        listParameters.AddFilter<EntityTag>(i => i.TagId, tagId.ToString());
        listParameters.AddSort<EntityTag>(i => i.Id, SortDirection.Ascending);
        var entityGuids = Write
            .All
            .Where(i => !excludedEntityGuids.Contains(i.EntityGuid))
            .ApplyListParametersAndGetTotalCount(listParameters)
            .Convert<EntityTag, Guid>(i => i.EntityGuid);
        return entityGuids;
    }

    private void CheckExcludedEntitiesCount(List<Guid> excludedEntityGuids)
    {
        if (excludedEntityGuids.Count > 100)
        {
            throw new ClientException("Excluding more than 100 items will slow down the system logarithmically. Please solve this problem.");
        }
    }

    public ListResult<Guid> GetEntityGuids(ListParameters listParameters, List<Guid> excludedEntityGuids)
    {
        CheckExcludedEntitiesCount(excludedEntityGuids);
        var entityGuids = Write
            .All
            .Where(i => !excludedEntityGuids.Contains(i.EntityGuid))
            .ApplyListParametersAndGetTotalCount(listParameters)
            .Convert<EntityTag, Guid>(i => i.EntityGuid);
        return entityGuids;
    }

    public void UpsertTags(Guid entityGuid, List<Guid> tagGuids)
    {
        Database.Open(Repository.Tag.ConnectionString).Run($"delete from EntityTags where EntityGuid = '{entityGuid}'");
        foreach (var tagGuid in tagGuids)
        {
            PutInTag(entityGuid, tagGuid);
        }
    }

    public void ToggleTag(Guid entityGuid, Guid tagGuid)
    {
        tagGuid.Ensure().IsNotEmpty();
        var tag = new TagBusiness().GetByGuid(tagGuid);
        var entityTag = Write.All.FirstOrDefault(i => i.EntityGuid == entityGuid && i.TagId == tag.Id);
        if (entityTag == null)
        {
            entityTag = new EntityTag();
            entityTag.EntityGuid = entityGuid;
            entityTag.TagId = tag.Id;
            Write.Create(entityTag);
        }
        else
        {
            Write.Delete(entityTag);
        }
        new TagBusiness().CountItemsInTag(tag.Id);
        OnTagToggled?.Invoke(entityTag);
    }

    public void PutInTag(Guid entityGuid, Guid tagGuid)
    {
        if (IsInTag(entityGuid, tagGuid))
        {
            return;
        }
        ToggleTag(entityGuid, tagGuid);
    }

    public void RemoveEntity(string entityType, Guid entityGuid)
    {
        var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
    }

    public void RemoveOrphanEntities(string entityType, List<Guid> entityGuids)
    {
        var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
        var orphanEntityTags = Write.All.Where(i => !entityGuids.Contains(i.EntityGuid)).ToList();
        foreach (var orphanEntityTag in orphanEntityTags)
        {
            Write.Delete(orphanEntityTag);
        }
    }

    public bool IsInTag(Guid entityGuid, Guid tagGuid)
    {
        var tag = new TagBusiness().GetByGuid(tagGuid);
        var entityTag = Read.All.Any(i => i.EntityGuid == entityGuid && i.TagId == tag.Id);
        return entityTag;
    }
}
