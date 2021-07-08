using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Holism.Taxonomy.DataAccess.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hierarchies",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(nullable: false),
                    EntityTypeGuid = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    IconGuid = table.Column<Guid>(nullable: true),
                    IconSvg = table.Column<string>(nullable: true),
                    ParentId = table.Column<long>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    Show = table.Column<bool>(nullable: false),
                    ItemsCount = table.Column<int>(nullable: true),
                    UrlKey = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: true),
                    IsLeaf = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hierarchies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HierarchyItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HierarchyId = table.Column<long>(nullable: false),
                    EntityGuid = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HierarchyItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TagItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagId = table.Column<long>(nullable: false),
                    EntityGuid = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(nullable: false),
                    EntityTypeGuid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IconGuid = table.Column<Guid>(nullable: true),
                    IconSvg = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    Show = table.Column<bool>(nullable: false),
                    ItemsCount = table.Column<int>(nullable: true),
                    UrlKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hierarchies");

            migrationBuilder.DropTable(
                name: "HierarchyItems");

            migrationBuilder.DropTable(
                name: "TagItems");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
