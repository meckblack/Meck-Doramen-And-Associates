using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeckDoramenAndAssociates.Data.Migrations
{
    public partial class AboutUs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutUs",
                columns: table => new
                {
                    AboutUsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUs", x => x.AboutUsId);
                });

            migrationBuilder.CreateTable(
                name: "AboutUsParagraph",
                columns: table => new
                {
                    AboutUsParagraphId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Body = table.Column<string>(nullable: false),
                    AboutUsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUsParagraph", x => x.AboutUsParagraphId);
                    table.ForeignKey(
                        name: "FK_AboutUsParagraph_AboutUs_AboutUsId",
                        column: x => x.AboutUsId,
                        principalTable: "AboutUs",
                        principalColumn: "AboutUsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AboutUsBulletPoint",
                columns: table => new
                {
                    AboutUsBulletPointId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Body = table.Column<string>(nullable: false),
                    AboutUsParagraphId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUsBulletPoint", x => x.AboutUsBulletPointId);
                    table.ForeignKey(
                        name: "FK_AboutUsBulletPoint_AboutUsParagraph_AboutUsParagraphId",
                        column: x => x.AboutUsParagraphId,
                        principalTable: "AboutUsParagraph",
                        principalColumn: "AboutUsParagraphId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsBulletPoint_AboutUsParagraphId",
                table: "AboutUsBulletPoint",
                column: "AboutUsParagraphId");

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsParagraph_AboutUsId",
                table: "AboutUsParagraph",
                column: "AboutUsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutUsBulletPoint");

            migrationBuilder.DropTable(
                name: "AboutUsParagraph");

            migrationBuilder.DropTable(
                name: "AboutUs");
        }
    }
}
