using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeckDoramenAndAssociates.Data.Migrations
{
    public partial class upding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "HeaderImages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "HeaderImages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLastModified",
                table: "HeaderImages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                table: "HeaderImages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "FooterImages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "FooterImages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLastModified",
                table: "FooterImages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                table: "FooterImages",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "HeaderImages");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "HeaderImages");

            migrationBuilder.DropColumn(
                name: "DateLastModified",
                table: "HeaderImages");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "HeaderImages");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "FooterImages");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "FooterImages");

            migrationBuilder.DropColumn(
                name: "DateLastModified",
                table: "FooterImages");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "FooterImages");
        }
    }
}
