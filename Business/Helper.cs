using System;
using System.Collections.Generic;

namespace Holism.Taxonomy.Business
{
    public class Helper
    {
        string taxonomyDatabaseName;

        string entityDatabaseName;

        public Helper(string taxonomyDatabaseName = null, string entityDatabaseName = null)
        {
            this.taxonomyDatabaseName = taxonomyDatabaseName;
            this.entityDatabaseName = entityDatabaseName;
        }

        public void RemoveEntity(string entityTypeName, Guid entityGuid)
        {
            new HierarchyItemBusiness(taxonomyDatabaseName, entityDatabaseName).RemoveEntity(entityTypeName, entityGuid);
            new TagItemBusiness(taxonomyDatabaseName, entityDatabaseName).RemoveEntity(entityTypeName, entityGuid);
        }

        public void RemoveOrphanEntities(string entityTypeName, List<Guid> entityGuids)
        {
            new HierarchyItemBusiness(taxonomyDatabaseName, entityDatabaseName).RemoveOrphanEntities(entityTypeName, entityGuids);
            new TagItemBusiness(taxonomyDatabaseName, entityDatabaseName).RemoveOrphanEntities(entityTypeName, entityGuids);
        }
    }
}
