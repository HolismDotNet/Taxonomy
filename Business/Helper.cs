using System;
using System.Collections.Generic;

namespace Holism.Taxonomy.Business
{
    public class Helper
    {
        public void RemoveEntity(string entityTypeName, Guid entityGuid)
        {
            new HierarchyItemBusiness().RemoveEntity(entityTypeName, entityGuid);
            new TagItemBusiness().RemoveEntity(entityTypeName, entityGuid);
        }

        public void RemoveOrphanEntities(string entityTypeName, List<Guid> entityGuids)
        {
            new HierarchyItemBusiness().RemoveOrphanEntities(entityTypeName, entityGuids);
            new TagItemBusiness().RemoveOrphanEntities(entityTypeName, entityGuids);
        }
    }
}
