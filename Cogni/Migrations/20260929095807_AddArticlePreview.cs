using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cogni.Migrations
{
    /// <inheritdoc />
    public partial class AddArticlePreview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArticlePreview",
                table: "article",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticlePreview",
                table: "article");
        }
    }
}
