using Holism.EntityFramework;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq.Expressions;
using Holism.Taxonomy.DataAccess.DbContexts;

namespace Holism.Taxonomy.DataAccess.Repositories
{
    public partial class TagRepository : Repository<Holism.Taxonomy.DataAccess.Models.Tag>
    {
        public TagRepository(string databaseName = null)
            : base(new TagDbContext(databaseName))
        {
        }

        public override DataTable ConfigureDataTable()
        {
            var table = new DataTable();

			table.Columns.Add("Id", typeof(long));
			table.Columns.Add("Guid", typeof(Guid));
			table.Columns.Add("EntityTypeGuid", typeof(Guid));
			table.Columns.Add("Name", typeof(string));
			table.Columns.Add("IconGuid", typeof(Guid));
			table.Columns.Add("IconSvg", typeof(string));
			table.Columns.Add("Description", typeof(string));
			table.Columns.Add("Order", typeof(int));
			table.Columns.Add("Show", typeof(bool));
			table.Columns.Add("ItemsCount", typeof(int));
			table.Columns.Add("UrlKey", typeof(string));

            return table;
        }

        public override void AddRecord(DataTable table, Holism.Taxonomy.DataAccess.Models.Tag tag)
        {
            var row = table.NewRow();

			row["Id"] = (object)tag.Id ?? DBNull.Value;
			row["Guid"] = (object)tag.Guid ?? DBNull.Value;
			row["EntityTypeGuid"] = (object)tag.EntityTypeGuid ?? DBNull.Value;
			row["Name"] = (object)tag.Name ?? DBNull.Value;
			row["IconGuid"] = (object)tag.IconGuid ?? DBNull.Value;
			row["IconSvg"] = (object)tag.IconSvg ?? DBNull.Value;
			row["Description"] = (object)tag.Description ?? DBNull.Value;
			row["Order"] = (object)tag.Order ?? DBNull.Value;
			row["Show"] = (object)tag.Show ?? DBNull.Value;
			row["ItemsCount"] = (object)tag.ItemsCount ?? DBNull.Value;
			row["UrlKey"] = (object)tag.UrlKey ?? DBNull.Value;

            table.Rows.Add(row);
        }

        public override void AddColumnMappings(SqlBulkCopy bulkOperator)
        {
			bulkOperator.ColumnMappings.Add("Id", "[Id]");
			bulkOperator.ColumnMappings.Add("Guid", "[Guid]");
			bulkOperator.ColumnMappings.Add("EntityTypeGuid", "[EntityTypeGuid]");
			bulkOperator.ColumnMappings.Add("Name", "[Name]");
			bulkOperator.ColumnMappings.Add("IconGuid", "[IconGuid]");
			bulkOperator.ColumnMappings.Add("IconSvg", "[IconSvg]");
			bulkOperator.ColumnMappings.Add("Description", "[Description]");
			bulkOperator.ColumnMappings.Add("Order", "[Order]");
			bulkOperator.ColumnMappings.Add("Show", "[Show]");
			bulkOperator.ColumnMappings.Add("ItemsCount", "[ItemsCount]");
			bulkOperator.ColumnMappings.Add("UrlKey", "[UrlKey]");
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
                return "([Guid], [EntityTypeGuid], [Name], [IconGuid], [IconSvg], [Description], [Order], [Show], [ItemsCount], [UrlKey]) values (s.[Guid], s.[EntityTypeGuid], s.[Name], s.[IconGuid], s.[IconSvg], s.[Description], s.[Order], s.[Show], s.[ItemsCount], s.[UrlKey])";
            }
        }

        public override string BulkUpdateUpdateClause
        {
            get
            {
                return "t.[Guid] = s.[Guid], t.[EntityTypeGuid] = s.[EntityTypeGuid], t.[Name] = s.[Name], t.[IconGuid] = s.[IconGuid], t.[IconSvg] = s.[IconSvg], t.[Description] = s.[Description], t.[Order] = s.[Order], t.[Show] = s.[Show], t.[ItemsCount] = s.[ItemsCount], t.[UrlKey] = s.[UrlKey]";
            }
        }

        public override string TableName
        {
            get
            {
                return "[Tags]";
            }
        }

        public override Expression<Func<Holism.Taxonomy.DataAccess.Models.Tag, bool>> ExistenceFilter(Holism.Taxonomy.DataAccess.Models.Tag t)
        {
            Expression<Func<Holism.Taxonomy.DataAccess.Models.Tag, bool>> result = null;
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
						[Name] nvarchar(100) not null,
						[IconGuid] uniqueidentifier null,
						[IconSvg] nvarchar(MAX) null,
						[Description] nvarchar(MAX) null,
						[Order] int not null,
						[Show] bit not null,
						[ItemsCount] int null,
						[UrlKey] nvarchar(100) null,    
                    )
                    ";
            return tempTableScript;
        }
    }
}
