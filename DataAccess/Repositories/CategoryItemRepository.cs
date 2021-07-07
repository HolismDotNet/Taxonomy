using Holism.EntityFramework;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq.Expressions;
using Holism.Taxonomy.DataAccess.DbContexts;

namespace Holism.Taxonomy.DataAccess.Repositories
{
    public partial class CategoryItemRepository : Repository<Holism.Taxonomy.DataAccess.Models.CategoryItem>
    {
        public CategoryItemRepository(string databaseName = null)
            : base(new CategoryItemDbContext(databaseName))
        {
        }

        public override DataTable ConfigureDataTable()
        {
            var table = new DataTable();

			table.Columns.Add("Id", typeof(long));
			table.Columns.Add("CategoryId", typeof(long));
			table.Columns.Add("EntityTypeGuid", typeof(Guid));
			table.Columns.Add("EntityGuid", typeof(Guid));
			table.Columns.Add("Order", typeof(int));

            return table;
        }

        public override void AddRecord(DataTable table, Holism.Taxonomy.DataAccess.Models.CategoryItem categoryItem)
        {
            var row = table.NewRow();

			row["Id"] = (object)categoryItem.Id ?? DBNull.Value;
			row["CategoryId"] = (object)categoryItem.CategoryId ?? DBNull.Value;
			row["EntityTypeGuid"] = (object)categoryItem.EntityTypeGuid ?? DBNull.Value;
			row["EntityGuid"] = (object)categoryItem.EntityGuid ?? DBNull.Value;
			row["Order"] = (object)categoryItem.Order ?? DBNull.Value;

            table.Rows.Add(row);
        }

        public override void AddColumnMappings(SqlBulkCopy bulkOperator)
        {
			bulkOperator.ColumnMappings.Add("Id", "[Id]");
			bulkOperator.ColumnMappings.Add("CategoryId", "[CategoryId]");
			bulkOperator.ColumnMappings.Add("EntityTypeGuid", "[EntityTypeGuid]");
			bulkOperator.ColumnMappings.Add("EntityGuid", "[EntityGuid]");
			bulkOperator.ColumnMappings.Add("Order", "[Order]");
        }

        public override string BulkUpdateComparisonKey
        {
            get
            {
                return "(t.[Id] = s.[Id]) or (t.[CategoryId] = s.[CategoryId] and t.[EntityGuid] = s.[EntityGuid] and t.[EntityTypeGuid] = s.[EntityTypeGuid])";;
            }
        }

        public override string BulkUpdateInsertClause
        {
            get
            {
                return "([CategoryId], [EntityTypeGuid], [EntityGuid], [Order]) values (s.[CategoryId], s.[EntityTypeGuid], s.[EntityGuid], s.[Order])";
            }
        }

        public override string BulkUpdateUpdateClause
        {
            get
            {
                return "t.[CategoryId] = s.[CategoryId], t.[EntityTypeGuid] = s.[EntityTypeGuid], t.[EntityGuid] = s.[EntityGuid], t.[Order] = s.[Order]";
            }
        }

        public override string TableName
        {
            get
            {
                return "[CategoryItems]";
            }
        }

        public override Expression<Func<Holism.Taxonomy.DataAccess.Models.CategoryItem, bool>> ExistenceFilter(Holism.Taxonomy.DataAccess.Models.CategoryItem t)
        {
            Expression<Func<Holism.Taxonomy.DataAccess.Models.CategoryItem, bool>> result = null;
            if (t.Id > 0)
            {
                result = i => i.Id == t.Id;
            }
            else
            {
                result = i => (i.CategoryId == t.CategoryId && i.CategoryId != null) && (i.EntityGuid == t.EntityGuid && i.EntityGuid != null) && (i.EntityTypeGuid == t.EntityTypeGuid && i.EntityTypeGuid != null);
            }
            return result;
        }

        public override string TempTableCreationScript(string tempTableName)
        {
            var tempTableScript =  $@"
                    create table {tempTableName}
                    (
						[Id] bigint not null,
						[CategoryId] bigint not null,
						[EntityTypeGuid] uniqueidentifier not null,
						[EntityGuid] uniqueidentifier not null,
						[Order] int not null,    
                    )
                    ";
            return tempTableScript;
        }
    }
}
