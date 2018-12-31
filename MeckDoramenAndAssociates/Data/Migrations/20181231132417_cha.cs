using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeckDoramenAndAssociates.Data.Migrations
{
    public partial class cha : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "SubServices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "SubServices",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLastModified",
                table: "SubServices",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                table: "SubServices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Services",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Services",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLastModified",
                table: "Services",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                table: "Services",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Paragraphs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Paragraphs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLastModified",
                table: "Paragraphs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                table: "Paragraphs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "BulletPoints",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "BulletPoints",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLastModified",
                table: "BulletPoints",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                table: "BulletPoints",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SubServices");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "SubServices");

            migrationBuilder.DropColumn(
                name: "DateLastModified",
                table: "SubServices");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "SubServices");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "DateLastModified",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Paragraphs");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Paragraphs");

            migrationBuilder.DropColumn(
                name: "DateLastModified",
                table: "Paragraphs");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Paragraphs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BulletPoints");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "BulletPoints");

            migrationBuilder.DropColumn(
                name: "DateLastModified",
                table: "BulletPoints");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "BulletPoints");
        }
    }
}
