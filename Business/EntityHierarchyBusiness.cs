namespace Taxonomy;

public class EntityHierarchyBusiness : Business<EntityHierarchyView, EntityHierarchy>
{
    protected override Read<EntityHierarchyView> Read => Repository.EntityHierarchyView;

    protected override Write<EntityHierarchy> Write => Repository.EntityHierarchy;
}