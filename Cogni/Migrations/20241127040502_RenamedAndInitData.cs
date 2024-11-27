using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cogni.Migrations
{
    /// <inheritdoc />
    public partial class RenamedAndInitData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "mbti_type",
                columns: new[] { "id_mbti_type", "name_of_type" },
                values: new object[,]
                {
                    { 1, "ENFJ" },
                    { 2, "ENTJ" },
                    { 3, "ENFP" },
                    { 4, "ENTP" },
                    { 5, "INFJ" },
                    { 6, "INTJ" },
                    { 7, "INFP" },
                    { 8, "INTP" },
                    { 9, "ISFP" },
                    { 10, "ISFG" },
                    { 11, "ESFP" },
                    { 12, "ESFJ" },
                    { 13, "ISTJ" },
                    { 14, "ISTP" },
                    { 15, "ESTP" },
                    { 16, "ESTJ" }
                });

            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "id_role", "name_role" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "Admin" },
                    { 3, "Moderator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "mbti_type",
                keyColumn: "id_mbti_type",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "mbti_type",
                keyColumn: "id_mbti_type",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "mbti_type",
                keyColumn: "id_mbti_type",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "mbti_type",
                keyColumn: "id_mbti_type",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "mbti_type",
                keyColumn: "id_mbti_type",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "mbti_type",
                keyColumn: "id_mbti_type",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "mbti_type",
                keyColumn: "id_mbti_type",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "mbti_type",
                keyColumn: "id_mbti_type",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "mbti_type",
                keyColumn: "id_mbti_type",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "mbti_type",
                keyColumn: "id_mbti_type",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "mbti_type",
                keyColumn: "id_mbti_type",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "mbti_type",
                keyColumn: "id_mbti_type",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "mbti_type",
                keyColumn: "id_mbti_type",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "mbti_type",
                keyColumn: "id_mbti_type",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "mbti_type",
                keyColumn: "id_mbti_type",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "mbti_type",
                keyColumn: "id_mbti_type",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "role",
                keyColumn: "id_role",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "role",
                keyColumn: "id_role",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "role",
                keyColumn: "id_role",
                keyValue: 3);
        }
    }
}
