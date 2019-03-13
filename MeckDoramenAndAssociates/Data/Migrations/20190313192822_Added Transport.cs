using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeckDoramenAndAssociates.Data.Migrations
{
    public partial class AddedTransport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "FooterAboutUs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "FooterAboutUs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLastModified",
                table: "FooterAboutUs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                table: "FooterAboutUs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Brochure",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Brochure",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLastModified",
                table: "Brochure",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                table: "Brochure",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "FooterAboutUs");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "FooterAboutUs");

            migrationBuilder.DropColumn(
                name: "DateLastModified",
                table: "FooterAboutUs");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "FooterAboutUs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Brochure");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Brochure");

            migrationBuilder.DropColumn(
                name: "DateLastModified",
                table: "Brochure");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Brochure");
        }
    }
}
