using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeckDoramenAndAssociates.Data.Migrations
{
    public partial class about : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "AboutUsParagraph",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "AboutUsParagraph",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLastModified",
                table: "AboutUsParagraph",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                table: "AboutUsParagraph",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "AboutUsBulletPoint",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "AboutUsBulletPoint",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLastModified",
                table: "AboutUsBulletPoint",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                table: "AboutUsBulletPoint",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "AboutUs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "AboutUs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLastModified",
                table: "AboutUs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                table: "AboutUs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AboutUsParagraph");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "AboutUsParagraph");

            migrationBuilder.DropColumn(
                name: "DateLastModified",
                table: "AboutUsParagraph");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "AboutUsParagraph");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AboutUsBulletPoint");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "AboutUsBulletPoint");

            migrationBuilder.DropColumn(
                name: "DateLastModified",
                table: "AboutUsBulletPoint");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "AboutUsBulletPoint");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AboutUs");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "AboutUs");

            migrationBuilder.DropColumn(
                name: "DateLastModified",
                table: "AboutUs");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "AboutUs");
        }
    }
}
