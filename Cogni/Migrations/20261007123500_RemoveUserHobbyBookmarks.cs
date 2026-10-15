using Cogni.Database.Context;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cogni.Migrations
{
    [DbContext(typeof(CogniDbContext))]
    [Migration("20261007123500_RemoveUserHobbyBookmarks")]
    public partial class RemoveUserHobbyBookmarks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS user_hobbies;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_hobbies",
                columns: table => new
                {
                    id_user_hobbies = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_hobby = table.Column<int>(type: "integer", nullable: false),
                    id_user = table.Column<int>(type: "integer", nullable: false),
                    is_bookmarked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_hobbies_pkey", x => x.id_user_hobbies);
                    table.ForeignKey(
                        name: "user_hobbies_id_hobby_fkey",
                        column: x => x.id_hobby,
                        principalTable: "hobby",
                        principalColumn: "id_hobby",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "user_hobbies_id_user_fkey",
                        column: x => x.id_user,
                        principalTable: "customuser",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_hobbies_id_hobby",
                table: "user_hobbies",
                column: "id_hobby");
            migrationBuilder.CreateIndex(
                name: "IX_user_hobbies_id_user_id_hobby",
                table: "user_hobbies",
                columns: new[] { "id_user", "id_hobby" },
                unique: true);
        }
    }
}
