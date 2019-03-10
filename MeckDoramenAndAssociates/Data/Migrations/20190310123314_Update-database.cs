using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeckDoramenAndAssociates.Data.Migrations
{
    public partial class Updatedatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanManagePartner",
                table: "Roles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Partners",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Partners",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLastModified",
                table: "Partners",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                table: "Partners",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanManagePartner",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "DateLastModified",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Partners");
        }
    }
}
