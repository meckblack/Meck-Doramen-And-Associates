using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeckDoramenAndAssociates.Data.Migrations
{
    public partial class Imagesforheader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeaderAndFooterImages");

            migrationBuilder.CreateTable(
                name: "FooterImages",
                columns: table => new
                {
                    FooterImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FooterImages", x => x.FooterImageId);
                });

            migrationBuilder.CreateTable(
                name: "HeaderImages",
                columns: table => new
                {
                    HeaderImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeaderImages", x => x.HeaderImageId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FooterImages");

            migrationBuilder.DropTable(
                name: "HeaderImages");

            migrationBuilder.CreateTable(
                name: "HeaderAndFooterImages",
                columns: table => new
                {
                    HeaderAndFooterImagesId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FooterImage = table.Column<string>(nullable: true),
                    HeaderImage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeaderAndFooterImages", x => x.HeaderAndFooterImagesId);
                });
        }
    }
}
