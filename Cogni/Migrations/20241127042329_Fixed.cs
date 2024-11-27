using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cogni.Migrations
{
    /// <inheritdoc />
    public partial class Fixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "customuser_name_key",
                table: "customuser");

            migrationBuilder.CreateIndex(
                name: "customuser_name_key",
                table: "customuser",
                column: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "customuser_name_key",
                table: "customuser");

            migrationBuilder.CreateIndex(
                name: "customuser_name_key",
                table: "customuser",
                column: "name",
                unique: true);
        }
    }
}
