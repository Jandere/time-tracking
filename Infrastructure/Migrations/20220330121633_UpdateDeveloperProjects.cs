using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeveloperProjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeveloperProject_AspNetUsers_DeveloperId",
                table: "DeveloperProject");

            migrationBuilder.DropForeignKey(
                name: "FK_DeveloperProject_Projects_ProjectId",
                table: "DeveloperProject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeveloperProject",
                table: "DeveloperProject");

            migrationBuilder.RenameTable(
                name: "DeveloperProject",
                newName: "DeveloperProjects");

            migrationBuilder.RenameIndex(
                name: "IX_DeveloperProject_ProjectId",
                table: "DeveloperProjects",
                newName: "IX_DeveloperProjects_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_DeveloperProject_DeveloperId",
                table: "DeveloperProjects",
                newName: "IX_DeveloperProjects_DeveloperId");

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeveloperProjects",
                table: "DeveloperProjects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeveloperProjects_AspNetUsers_DeveloperId",
                table: "DeveloperProjects",
                column: "DeveloperId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeveloperProjects_Projects_ProjectId",
                table: "DeveloperProjects",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeveloperProjects_AspNetUsers_DeveloperId",
                table: "DeveloperProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_DeveloperProjects_Projects_ProjectId",
                table: "DeveloperProjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeveloperProjects",
                table: "DeveloperProjects");

            migrationBuilder.RenameTable(
                name: "DeveloperProjects",
                newName: "DeveloperProject");

            migrationBuilder.RenameIndex(
                name: "IX_DeveloperProjects_ProjectId",
                table: "DeveloperProject",
                newName: "IX_DeveloperProject_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_DeveloperProjects_DeveloperId",
                table: "DeveloperProject",
                newName: "IX_DeveloperProject_DeveloperId");

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeveloperProject",
                table: "DeveloperProject",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeveloperProject_AspNetUsers_DeveloperId",
                table: "DeveloperProject",
                column: "DeveloperId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeveloperProject_Projects_ProjectId",
                table: "DeveloperProject",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
