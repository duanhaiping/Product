using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BIMPlatform.Migrations
{
    public partial class addProjectUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         
            migrationBuilder.CreateTable(
                name: "Pro_ProjectUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pro_ProjectUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pro_ProjectUser_Pro_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Pro_Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pro_ProjectUser_Sys_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Sys_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pro_ProjectUser_ProjectId",
                table: "Pro_ProjectUser",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Pro_ProjectUser_UserId",
                table: "Pro_ProjectUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pro_ProjectUser");

            migrationBuilder.DropTable(
                name: "Sys_Users");
        }
    }
}
