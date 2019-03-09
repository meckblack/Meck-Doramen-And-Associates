using Microsoft.EntityFrameworkCore.Migrations;

namespace MeckDoramenAndAssociates.Data.Migrations
{
    public partial class rolechanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CanDoEverything",
                table: "Roles",
                newName: "CanMangeUsers");

            migrationBuilder.AddColumn<bool>(
                name: "CanManageAboutUs",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManageEnquiry",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManageLandingDetails",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManageMarketResearch",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManageNews",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManageServices",
                table: "Roles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanManageAboutUs",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanManageEnquiry",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanManageLandingDetails",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanManageMarketResearch",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanManageNews",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanManageServices",
                table: "Roles");

            migrationBuilder.RenameColumn(
                name: "CanMangeUsers",
                table: "Roles",
                newName: "CanDoEverything");
        }
    }
}
