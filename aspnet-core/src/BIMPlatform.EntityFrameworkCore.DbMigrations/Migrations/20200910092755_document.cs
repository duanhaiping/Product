using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BIMPlatform.Migrations
{
    public partial class document : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pro_Document",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FolderID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    DocNumber = table.Column<string>(nullable: true),
                    Suffix = table.Column<string>(maxLength: 10, nullable: true),
                    Properties = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    RecycleIdentity = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pro_Document", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pro_Document");
        }
    }
}
