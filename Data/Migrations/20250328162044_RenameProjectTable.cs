using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameProjectTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEntity_Clients_ClientId",
                table: "ProjectEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEntity_ProjectStatuses_StatusId",
                table: "ProjectEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectEntity",
                table: "ProjectEntity");

            migrationBuilder.RenameTable(
                name: "ProjectEntity",
                newName: "Projects");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectEntity_StatusId",
                table: "Projects",
                newName: "IX_Projects_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectEntity_ClientId",
                table: "Projects",
                newName: "IX_Projects_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Clients_ClientId",
                table: "Projects",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectStatuses_StatusId",
                table: "Projects",
                column: "StatusId",
                principalTable: "ProjectStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Clients_ClientId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectStatuses_StatusId",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "ProjectEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_StatusId",
                table: "ProjectEntity",
                newName: "IX_ProjectEntity_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_ClientId",
                table: "ProjectEntity",
                newName: "IX_ProjectEntity_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectEntity",
                table: "ProjectEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEntity_Clients_ClientId",
                table: "ProjectEntity",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEntity_ProjectStatuses_StatusId",
                table: "ProjectEntity",
                column: "StatusId",
                principalTable: "ProjectStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
