using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeckDoramenAndAssociates.Data.Migrations
{
    public partial class AddedNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SkillFive",
                table: "LandingSkills");

            migrationBuilder.DropColumn(
                name: "SkillFiveRating",
                table: "LandingSkills");

            migrationBuilder.DropColumn(
                name: "SkillFour",
                table: "LandingSkills");

            migrationBuilder.DropColumn(
                name: "SkillFourRating",
                table: "LandingSkills");

            migrationBuilder.DropColumn(
                name: "SkillOne",
                table: "LandingSkills");

            migrationBuilder.DropColumn(
                name: "SkillOneRating",
                table: "LandingSkills");

            migrationBuilder.DropColumn(
                name: "SkillSix",
                table: "LandingSkills");

            migrationBuilder.DropColumn(
                name: "SkillSixRating",
                table: "LandingSkills");

            migrationBuilder.DropColumn(
                name: "SkillThree",
                table: "LandingSkills");

            migrationBuilder.DropColumn(
                name: "SkillThreeRating",
                table: "LandingSkills");

            migrationBuilder.DropColumn(
                name: "SkillTwo",
                table: "LandingSkills");

            migrationBuilder.DropColumn(
                name: "SkillTwoRating",
                table: "LandingSkills");

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    NewsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    Body = table.Column<string>(nullable: false),
                    Image = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.NewsId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.AddColumn<string>(
                name: "SkillFive",
                table: "LandingSkills",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SkillFiveRating",
                table: "LandingSkills",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SkillFour",
                table: "LandingSkills",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SkillFourRating",
                table: "LandingSkills",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SkillOne",
                table: "LandingSkills",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SkillOneRating",
                table: "LandingSkills",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SkillSix",
                table: "LandingSkills",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SkillSixRating",
                table: "LandingSkills",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SkillThree",
                table: "LandingSkills",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SkillThreeRating",
                table: "LandingSkills",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SkillTwo",
                table: "LandingSkills",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SkillTwoRating",
                table: "LandingSkills",
                nullable: false,
                defaultValue: 0);
        }
    }
}
