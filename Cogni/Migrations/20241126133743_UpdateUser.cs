using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cogni.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password",
                table: "customuser");

            migrationBuilder.DropColumn(
                name: "type_mbti",
                table: "customuser");

            migrationBuilder.AddColumn<string>(
                name: "AToken",
                table: "customuser",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "customuser",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RToken",
                table: "customuser",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "customuser",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte[]>(
                name: "Salt",
                table: "customuser",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AToken",
                table: "customuser");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "customuser");

            migrationBuilder.DropColumn(
                name: "RToken",
                table: "customuser");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "customuser");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "customuser");

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "customuser",
                type: "character varying(45)",
                maxLength: 45,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "type_mbti",
                table: "customuser",
                type: "character varying(4)",
                maxLength: 4,
                nullable: true);
        }
    }
}
