using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeckDoramenAndAssociates.Data.Migrations
{
    public partial class Removedfooterimage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FooterImages");

            migrationBuilder.DropColumn(
                name: "OpenWeekdays",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "WeekdaysCloseTime",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "WeekdaysOpenTime",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "WeekendsCloseTIme",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "WeekendsOpenTime",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "OpenWeekends",
                table: "Contacts",
                newName: "Number2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Number2",
                table: "Contacts",
                newName: "OpenWeekends");

            migrationBuilder.AddColumn<string>(
                name: "OpenWeekdays",
                table: "Contacts",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "WeekdaysCloseTime",
                table: "Contacts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WeekdaysOpenTime",
                table: "Contacts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WeekendsCloseTIme",
                table: "Contacts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WeekendsOpenTime",
                table: "Contacts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "FooterImages",
                columns: table => new
                {
                    FooterImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    LastModifiedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FooterImages", x => x.FooterImageId);
                });
        }
    }
}
