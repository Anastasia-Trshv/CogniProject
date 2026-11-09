using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cogni.Migrations
{
    /// <inheritdoc />
    public partial class AddPostReactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_likes_user_id",
                table: "likes");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "likes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "post_id",
                table: "likes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reaction_type",
                table: "likes",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "like");

            migrationBuilder.AddPrimaryKey(
                name: "likes_pkey",
                table: "likes",
                columns: new[] { "user_id", "post_id" });

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropPrimaryKey(
                name: "likes_pkey",
                table: "likes");

            migrationBuilder.DropColumn(
                name: "reaction_type",
                table: "likes");

            migrationBuilder.AlterColumn<int>(
                name: "post_id",
                table: "likes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "likes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_likes_user_id",
                table: "likes",
                column: "user_id");
        }
    }
}
