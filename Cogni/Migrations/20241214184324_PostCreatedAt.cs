using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cogni.Migrations
{
    /// <inheritdoc />
    public partial class PostCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "post",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "mbti_type",
                keyColumn: "id_mbti_type",
                keyValue: 10,
                column: "name_of_type",
                value: "ISFJ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "post");

            migrationBuilder.UpdateData(
                table: "mbti_type",
                keyColumn: "id_mbti_type",
                keyValue: 10,
                column: "name_of_type",
                value: "ISFG");
        }
    }
}
