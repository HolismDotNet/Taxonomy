using Holism.EntityFramework;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq.Expressions;
using Holism.Taxonomy.DataAccess.DbContexts;

namespace Holism.Taxonomy.DataAccess.Repositories
{
    public partial class HierarchyRepository : Repository<Holism.Taxonomy.DataAccess.Models.Hierarchy>
    {
        public HierarchyRepository(string databaseName = null)
            : base(new HierarchyDbContext(databaseName))
        {
        }

        public override DataTable ConfigureDataTable()
        {
            var table = new DataTable();

			table.Columns.Add("Id", typeof(long));
			table.Columns.Add("Guid", typeof(Guid));
			table.Columns.Add("EntityTypeGuid", typeof(Guid));
			table.Columns.Add("Title", typeof(string));
			table.Columns.Add("Code", typeof(string));
			table.Columns.Add("IconGuid", typeof(Guid));
			table.Columns.Add("IconSvg", typeof(string));
			table.Columns.Add("ParentHierarchyId", typeof(long));
			table.Columns.Add("Description", typeof(string));
			table.Columns.Add("Order", typeof(int));
			table.Columns.Add("Show", typeof(bool));
			table.Columns.Add("ItemsCount", typeof(int));
			table.Columns.Add("UrlKey", typeof(string));
			table.Columns.Add("Level", typeof(int));
			table.Columns.Add("IsLeaf", typeof(bool));

            return table;
        }

        public override void AddRecord(DataTable table, Holism.Taxonomy.DataAccess.Models.Hierarchy hierarchy)
        {
            var row = table.NewRow();

			row["Id"] = (object)hierarchy.Id ?? DBNull.Value;
			row["Guid"] = (object)hierarchy.Guid ?? DBNull.Value;
			row["EntityTypeGuid"] = (object)hierarchy.EntityTypeGuid ?? DBNull.Value;
			row["Title"] = (object)hierarchy.Title ?? DBNull.Value;
			row["Code"] = (object)hierarchy.Code ?? DBNull.Value;
			row["IconGuid"] = (object)hierarchy.IconGuid ?? DBNull.Value;
			row["IconSvg"] = (object)hierarchy.IconSvg ?? DBNull.Value;
			row["ParentHierarchyId"] = (object)hierarchy.ParentHierarchyId ?? DBNull.Value;
			row["Description"] = (object)hierarchy.Description ?? DBNull.Value;
			row["Order"] = (object)hierarchy.Order ?? DBNull.Value;
			row["Show"] = (object)hierarchy.Show ?? DBNull.Value;
			row["ItemsCount"] = (object)hierarchy.ItemsCount ?? DBNull.Value;
			row["UrlKey"] = (object)hierarchy.UrlKey ?? DBNull.Value;
			row["Level"] = (object)hierarchy.Level ?? DBNull.Value;
			row["IsLeaf"] = (object)hierarchy.IsLeaf ?? DBNull.Value;

            table.Rows.Add(row);
        }

        public override void AddColumnMappings(SqlBulkCopy bulkOperator)
        {
			bulkOperator.ColumnMappings.Add("Id", "[Id]");
			bulkOperator.ColumnMappings.Add("Guid", "[Guid]");
			bulkOperator.ColumnMappings.Add("EntityTypeGuid", "[EntityTypeGuid]");
			bulkOperator.ColumnMappings.Add("Title", "[Title]");
			bulkOperator.ColumnMappings.Add("Code", "[Code]");
			bulkOperator.ColumnMappings.Add("IconGuid", "[IconGuid]");
			bulkOperator.ColumnMappings.Add("IconSvg", "[IconSvg]");
			bulkOperator.ColumnMappings.Add("ParentHierarchyId", "[ParentHierarchyId]");
			bulkOperator.ColumnMappings.Add("Description", "[Description]");
			bulkOperator.ColumnMappings.Add("Order", "[Order]");
			bulkOperator.ColumnMappings.Add("Show", "[Show]");
			bulkOperator.ColumnMappings.Add("ItemsCount", "[ItemsCount]");
			bulkOperator.ColumnMappings.Add("UrlKey", "[UrlKey]");
			bulkOperator.ColumnMappings.Add("Level", "[Level]");
			bulkOperator.ColumnMappings.Add("IsLeaf", "[IsLeaf]");
        }

        public override string BulkUpdateComparisonKey
        {
            get
            {
                return "(t.[Id] = s.[Id])";;
            }
        }

        public override string BulkUpdateInsertClause
        {
            get
            {
                return "([Guid], [EntityTypeGuid], [Title], [Code], [IconGuid], [IconSvg], [ParentHierarchyId], [Description], [Order], [Show], [ItemsCount], [UrlKey], [Level], [IsLeaf]) values (s.[Guid], s.[EntityTypeGuid], s.[Title], s.[Code], s.[IconGuid], s.[IconSvg], s.[ParentHierarchyId], s.[Description], s.[Order], s.[Show], s.[ItemsCount], s.[UrlKey], s.[Level], s.[IsLeaf])";
            }
        }

        public override string BulkUpdateUpdateClause
        {
            get
            {
                return "t.[Guid] = s.[Guid], t.[EntityTypeGuid] = s.[EntityTypeGuid], t.[Title] = s.[Title], t.[Code] = s.[Code], t.[IconGuid] = s.[IconGuid], t.[IconSvg] = s.[IconSvg], t.[ParentHierarchyId] = s.[ParentHierarchyId], t.[Description] = s.[Description], t.[Order] = s.[Order], t.[Show] = s.[Show], t.[ItemsCount] = s.[ItemsCount], t.[UrlKey] = s.[UrlKey], t.[Level] = s.[Level], t.[IsLeaf] = s.[IsLeaf]";
            }
        }

        public override string TableName
        {
            get
            {
                return "[Hierarchies]";
            }
        }

        public override Expression<Func<Holism.Taxonomy.DataAccess.Models.Hierarchy, bool>> ExistenceFilter(Holism.Taxonomy.DataAccess.Models.Hierarchy t)
        {
            Expression<Func<Holism.Taxonomy.DataAccess.Models.Hierarchy, bool>> result = null;
            if (t.Id > 0)
            {
                result = i => i.Id == t.Id;
            }
            else
            {
                result = i => i.Id == t.Id;
            }
            return result;
        }

        public override string TempTableCreationScript(string tempTableName)
        {
            var tempTableScript =  $@"
                    create table {tempTableName}
                    (
						[Id] bigint not null,
						[Guid] uniqueidentifier not null,
						[EntityTypeGuid] uniqueidentifier not null,
						[Title] nvarchar(100) not null,
						[Code] nvarchar(100) null,
						[IconGuid] uniqueidentifier null,
						[IconSvg] nvarchar(MAX) null,
						[ParentHierarchyId] bigint null,
						[Description] nvarchar(MAX) null,
						[Order] int not null,
						[Show] bit not null,
						[ItemsCount] int null,
						[UrlKey] nvarchar(400) null,
						[Level] int null,
						[IsLeaf] bit null,    
                    )
                    ";
            return tempTableScript;
        }
    }
}
