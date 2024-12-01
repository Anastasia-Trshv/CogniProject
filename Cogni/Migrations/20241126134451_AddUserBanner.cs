using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cogni.Migrations
{
    /// <inheritdoc />
    public partial class AddUserBanner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BannerImage",
                table: "customuser",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannerImage",
                table: "customuser");
        }
    }
}
