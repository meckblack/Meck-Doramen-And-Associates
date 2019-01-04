using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeckDoramenAndAssociates.Data.Migrations
{
    public partial class MarketResearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarketResearches",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    MarketResearchId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketResearches", x => x.MarketResearchId);
                });

            migrationBuilder.CreateTable(
                name: "MarketResearchParagraphs",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    MarketResearchParagraphId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Body = table.Column<string>(nullable: false),
                    MarketResearchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketResearchParagraphs", x => x.MarketResearchParagraphId);
                    table.ForeignKey(
                        name: "FK_MarketResearchParagraphs_MarketResearches_MarketResearchId",
                        column: x => x.MarketResearchId,
                        principalTable: "MarketResearches",
                        principalColumn: "MarketResearchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarketResearchBulletPoints",
                columns: table => new
                {
                    CreatedBy = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    MarketResearchBulletPointId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Body = table.Column<string>(nullable: false),
                    MarketResearchParagraphId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketResearchBulletPoints", x => x.MarketResearchBulletPointId);
                    table.ForeignKey(
                        name: "FK_MarketResearchBulletPoints_MarketResearchParagraphs_MarketResearchParagraphId",
                        column: x => x.MarketResearchParagraphId,
                        principalTable: "MarketResearchParagraphs",
                        principalColumn: "MarketResearchParagraphId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarketResearchBulletPoints_MarketResearchParagraphId",
                table: "MarketResearchBulletPoints",
                column: "MarketResearchParagraphId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketResearchParagraphs_MarketResearchId",
                table: "MarketResearchParagraphs",
                column: "MarketResearchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarketResearchBulletPoints");

            migrationBuilder.DropTable(
                name: "MarketResearchParagraphs");

            migrationBuilder.DropTable(
                name: "MarketResearches");
        }
    }
}
