using Holism.EntityFramework;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq.Expressions;
using Holism.Taxonomy.DataAccess.DbContexts;

namespace Holism.Taxonomy.DataAccess.Repositories
{
    public partial class HierarchyItemRepository : Repository<Holism.Taxonomy.DataAccess.Models.HierarchyItem>
    {
        public HierarchyItemRepository(string databaseName = null)
            : base(new HierarchyItemDbContext(databaseName))
        {
        }

        public override DataTable ConfigureDataTable()
        {
            var table = new DataTable();

			table.Columns.Add("Id", typeof(long));
			table.Columns.Add("HierarchyId", typeof(long));
			table.Columns.Add("EntityTypeGuid", typeof(Guid));
			table.Columns.Add("EntityGuid", typeof(Guid));
			table.Columns.Add("Order", typeof(int));

            return table;
        }

        public override void AddRecord(DataTable table, Holism.Taxonomy.DataAccess.Models.HierarchyItem hierarchyItem)
        {
            var row = table.NewRow();

			row["Id"] = (object)hierarchyItem.Id ?? DBNull.Value;
			row["HierarchyId"] = (object)hierarchyItem.HierarchyId ?? DBNull.Value;
			row["EntityTypeGuid"] = (object)hierarchyItem.EntityTypeGuid ?? DBNull.Value;
			row["EntityGuid"] = (object)hierarchyItem.EntityGuid ?? DBNull.Value;
			row["Order"] = (object)hierarchyItem.Order ?? DBNull.Value;

            table.Rows.Add(row);
        }

        public override void AddColumnMappings(SqlBulkCopy bulkOperator)
        {
			bulkOperator.ColumnMappings.Add("Id", "[Id]");
			bulkOperator.ColumnMappings.Add("HierarchyId", "[HierarchyId]");
			bulkOperator.ColumnMappings.Add("EntityTypeGuid", "[EntityTypeGuid]");
			bulkOperator.ColumnMappings.Add("EntityGuid", "[EntityGuid]");
			bulkOperator.ColumnMappings.Add("Order", "[Order]");
        }

        public override string BulkUpdateComparisonKey
        {
            get
            {
                return "(t.[Id] = s.[Id]) or (t.[HierarchyId] = s.[HierarchyId] and t.[EntityGuid] = s.[EntityGuid] and t.[EntityTypeGuid] = s.[EntityTypeGuid])";;
            }
        }

        public override string BulkUpdateInsertClause
        {
            get
            {
                return "([HierarchyId], [EntityTypeGuid], [EntityGuid], [Order]) values (s.[HierarchyId], s.[EntityTypeGuid], s.[EntityGuid], s.[Order])";
            }
        }

        public override string BulkUpdateUpdateClause
        {
            get
            {
                return "t.[HierarchyId] = s.[HierarchyId], t.[EntityTypeGuid] = s.[EntityTypeGuid], t.[EntityGuid] = s.[EntityGuid], t.[Order] = s.[Order]";
            }
        }

        public override string TableName
        {
            get
            {
                return "[HierarchyItems]";
            }
        }

        public override Expression<Func<Holism.Taxonomy.DataAccess.Models.HierarchyItem, bool>> ExistenceFilter(Holism.Taxonomy.DataAccess.Models.HierarchyItem t)
        {
            Expression<Func<Holism.Taxonomy.DataAccess.Models.HierarchyItem, bool>> result = null;
            if (t.Id > 0)
            {
                result = i => i.Id == t.Id;
            }
            else
            {
                result = i => (i.HierarchyId == t.HierarchyId && i.HierarchyId != null) && (i.EntityGuid == t.EntityGuid && i.EntityGuid != null) && (i.EntityTypeGuid == t.EntityTypeGuid && i.EntityTypeGuid != null);
            }
            return result;
        }

        public override string TempTableCreationScript(string tempTableName)
        {
            var tempTableScript =  $@"
                    create table {tempTableName}
                    (
						[Id] bigint not null,
						[HierarchyId] bigint not null,
						[EntityTypeGuid] uniqueidentifier not null,
						[EntityGuid] uniqueidentifier not null,
						[Order] int not null,    
                    )
                    ";
            return tempTableScript;
        }
    }
}
