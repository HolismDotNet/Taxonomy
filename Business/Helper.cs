namespace Taxonomy;

public class Helper
{
    public void RemoveEntity(string entityType, Guid entityGuid)
    {
        new HierarchyItemBusiness().RemoveEntity(entityType, entityGuid);
        new TagItemBusiness().RemoveEntity(entityType, entityGuid);
    }

    public void RemoveOrphanEntities(string entityType, List<Guid> entityGuids)
    {
        new HierarchyItemBusiness().RemoveOrphanEntities(entityType, entityGuids);
        new TagItemBusiness().RemoveOrphanEntities(entityType, entityGuids);
    }
}
