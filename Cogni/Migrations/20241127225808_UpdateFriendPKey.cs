using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cogni.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFriendPKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "friends_pkey",
                table: "friends");

            migrationBuilder.DropIndex(
                name: "IX_friends_friend_id",
                table: "friends");

            migrationBuilder.AddPrimaryKey(
                name: "friends_pkey",
                table: "friends",
                columns: new[] { "friend_id", "user_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "friends_pkey",
                table: "friends");

            migrationBuilder.AddPrimaryKey(
                name: "friends_pkey",
                table: "friends",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_friends_friend_id",
                table: "friends",
                column: "friend_id");
        }
    }
}
