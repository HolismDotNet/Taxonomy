using Holism.EntityFramework;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq.Expressions;
using Holism.Taxonomy.DataAccess.DbContexts;

namespace Holism.Taxonomy.DataAccess.Repositories
{
    public partial class TagItemRepository : Repository<Holism.Taxonomy.DataAccess.Models.TagItem>
    {
        public TagItemRepository(string databaseName = null)
            : base(new TagItemDbContext(databaseName))
        {
        }

        public override DataTable ConfigureDataTable()
        {
            var table = new DataTable();

			table.Columns.Add("Id", typeof(long));
			table.Columns.Add("TagId", typeof(long));
			table.Columns.Add("EntityTypeGuid", typeof(Guid));
			table.Columns.Add("EntityGuid", typeof(Guid));
			table.Columns.Add("Order", typeof(int));

            return table;
        }

        public override void AddRecord(DataTable table, Holism.Taxonomy.DataAccess.Models.TagItem tagItem)
        {
            var row = table.NewRow();

			row["Id"] = (object)tagItem.Id ?? DBNull.Value;
			row["TagId"] = (object)tagItem.TagId ?? DBNull.Value;
			row["EntityTypeGuid"] = (object)tagItem.EntityTypeGuid ?? DBNull.Value;
			row["EntityGuid"] = (object)tagItem.EntityGuid ?? DBNull.Value;
			row["Order"] = (object)tagItem.Order ?? DBNull.Value;

            table.Rows.Add(row);
        }

        public override void AddColumnMappings(SqlBulkCopy bulkOperator)
        {
			bulkOperator.ColumnMappings.Add("Id", "[Id]");
			bulkOperator.ColumnMappings.Add("TagId", "[TagId]");
			bulkOperator.ColumnMappings.Add("EntityTypeGuid", "[EntityTypeGuid]");
			bulkOperator.ColumnMappings.Add("EntityGuid", "[EntityGuid]");
			bulkOperator.ColumnMappings.Add("Order", "[Order]");
        }

        public override string BulkUpdateComparisonKey
        {
            get
            {
                return "(t.[Id] = s.[Id]) or (t.[EntityGuid] = s.[EntityGuid] and t.[EntityTypeGuid] = s.[EntityTypeGuid] and t.[TagId] = s.[TagId])";;
            }
        }

        public override string BulkUpdateInsertClause
        {
            get
            {
                return "([TagId], [EntityTypeGuid], [EntityGuid], [Order]) values (s.[TagId], s.[EntityTypeGuid], s.[EntityGuid], s.[Order])";
            }
        }

        public override string BulkUpdateUpdateClause
        {
            get
            {
                return "t.[TagId] = s.[TagId], t.[EntityTypeGuid] = s.[EntityTypeGuid], t.[EntityGuid] = s.[EntityGuid], t.[Order] = s.[Order]";
            }
        }

        public override string TableName
        {
            get
            {
                return "[TagItems]";
            }
        }

        public override Expression<Func<Holism.Taxonomy.DataAccess.Models.TagItem, bool>> ExistenceFilter(Holism.Taxonomy.DataAccess.Models.TagItem t)
        {
            Expression<Func<Holism.Taxonomy.DataAccess.Models.TagItem, bool>> result = null;
            if (t.Id > 0)
            {
                result = i => i.Id == t.Id;
            }
            else
            {
                result = i => (i.EntityGuid == t.EntityGuid && i.EntityGuid != null) && (i.EntityTypeGuid == t.EntityTypeGuid && i.EntityTypeGuid != null) && (i.TagId == t.TagId && i.TagId != null);
            }
            return result;
        }

        public override string TempTableCreationScript(string tempTableName)
        {
            var tempTableScript =  $@"
                    create table {tempTableName}
                    (
						[Id] bigint not null,
						[TagId] bigint not null,
						[EntityTypeGuid] uniqueidentifier not null,
						[EntityGuid] uniqueidentifier not null,
						[Order] int not null,    
                    )
                    ";
            return tempTableScript;
        }
    }
}
