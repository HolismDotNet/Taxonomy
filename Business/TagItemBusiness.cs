namespace Taxonomy;

public class TagItemBusiness : Business<TagItemView, TagItem>
{
    protected override Repository<TagItem> WriteRepository => Repository.TagItem;

    protected override ReadRepository<TagItemView> ReadRepository => Repository.TagItemView;

    private static Dictionary<Guid, Func<List<Guid>, Dictionary<Guid, object>>> entitiesInfoAugmenter = new Dictionary<Guid, Func<List<Guid>, Dictionary<Guid, object>>>();

    public static Action<TagItem> OnTagToggled;

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

    public void UpdateOrder(long tagItemId, int newOrder)
    {
        var tagItem = WriteRepository.Get(tagItemId);
        tagItem.Order = newOrder;
        Update(tagItem);
    }

    public List<TagItemView> GetItemTags(string entityType, Guid entityGuid)
    {
       var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
       var tagItems = ReadRepository.All
        .Where(i => i.EntityGuid == entityGuid)
        .ToList();
        return tagItems;
    }

    public int GetCountOfItemsInTag(Tag tag)
    {
        var count = ReadRepository.All.Count(i => i.TagId == tag.Id);
        return count;
    }

    public List<TagItemView> GetAllItems(long tagId)
    {
        var tag = new TagBusiness().Get(tagId);
        var allItems = ReadRepository.All.Where(i => i.TagId == tagId).OrderBy(i => i.Order).ThenBy(i => i.Id).ToList();
        if (entitiesInfoAugmenter.ContainsKey(tag.EntityTypeGuid))
        {
            var entityGuids = allItems.Select(i => i.EntityGuid).ToList();
            var entityInfoList = entitiesInfoAugmenter[tag.EntityTypeGuid](entityGuids);
            var tagItemsWithEntityInfo = allItems.Where(i => entityInfoList.ContainsKey(i.EntityGuid)).ToList();
            foreach (var tagItem in tagItemsWithEntityInfo)
            {
                ExpandoObjectExtensions.AddProperty(tagItem.RelatedItems, new EntityTypeBusiness().GetName(tag.EntityTypeGuid), entityInfoList[tagItem.EntityGuid]);
            }
        }
        return allItems;
    }

    public ListResult<Guid> GetEntityGuids(long tagId, int pageNumber, List<Guid> excludedEntityGuids)
    {
        CheckExcludedEntitiesCount(excludedEntityGuids);
        var listParameters = ListParameters.Create();
        listParameters.PageNumber = pageNumber;
        listParameters.AddFilter<TagItem>(i => i.TagId, tagId.ToString());
        listParameters.AddSort<TagItem>(i => i.Order, SortDirection.Ascending);
        listParameters.AddSort<TagItem>(i => i.Id, SortDirection.Ascending);
        var entityGuids = WriteRepository
            .All
            .Where(i => !excludedEntityGuids.Contains(i.EntityGuid))
            .ApplyListParametersAndGetTotalCount(listParameters)
            .Convert<TagItem, Guid>(i => i.EntityGuid);
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
        var entityGuids = WriteRepository
            .All
            .Where(i => !excludedEntityGuids.Contains(i.EntityGuid))
            .ApplyListParametersAndGetTotalCount(listParameters)
            .Convert<TagItem, Guid>(i => i.EntityGuid);
        return entityGuids;
    }

    public void UpsertTags(Guid entityGuid, List<Guid> tagGuids)
    {
        foreach (var tagGuid in tagGuids)
        {
            ToggleTag(entityGuid, tagGuid);
        }
    }

    public void ToggleTag(Guid entityGuid, Guid tagGuid)
    {
        tagGuid.Ensure().IsNotEmpty();
        var tag = new TagBusiness().GetByGuid(tagGuid);
        var tagItem = WriteRepository.All.FirstOrDefault(i => i.EntityGuid == entityGuid && i.TagId == tag.Id);
        if (tagItem == null)
        {
            tagItem = new TagItem();
            tagItem.EntityGuid = entityGuid;
            tagItem.TagId = tag.Id;
            tagItem.Order = 1;
            WriteRepository.Create(tagItem);
        }
        else
        {
            WriteRepository.Delete(tagItem);
        }
        new TagBusiness().CountItemsInTag(tag.Id);
        OnTagToggled?.Invoke(tagItem);
    }

    public void PutInTag(Guid tagGuid, Guid entityGuid)
    {
        if (IsInTag(entityGuid, tagGuid))
        {
            return;
        }
        ToggleTag(tagGuid, entityGuid);
    }

    public void RemoveEntity(string entityType, Guid entityGuid)
    {
        var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
    }

    public void RemoveOrphanEntities(string entityType, List<Guid> entityGuids)
    {
        var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
        var orphanTagItems = WriteRepository.All.Where(i => !entityGuids.Contains(i.EntityGuid)).ToList();
        foreach (var orphanTagItem in orphanTagItems)
        {
            WriteRepository.Delete(orphanTagItem);
        }
    }

    public bool IsInTag(Guid entityGuid, Guid tagGuid)
    {
        var tag = new TagBusiness().GetByGuid(tagGuid);
        var tagItem = ReadRepository.All.Any(i => i.EntityGuid == entityGuid && i.TagId == tag.Id);
        return tagItem;
    }
}
