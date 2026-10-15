using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cogni.Migrations
{
    /// <inheritdoc />
    public partial class ProfileTagSelection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_category",
                table: "tag",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tag_category",
                columns: table => new
                {
                    id_category = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tag_category_pkey", x => x.id_category);
                });

            migrationBuilder.CreateTable(
                name: "user_tag_categories",
                columns: table => new
                {
                    id_user_tag_categories = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_category = table.Column<int>(type: "integer", nullable: false),
                    id_user = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_tag_categories_pkey", x => x.id_user_tag_categories);
                    table.ForeignKey(
                        name: "user_tag_categories_id_category_fkey",
                        column: x => x.id_category,
                        principalTable: "tag_category",
                        principalColumn: "id_category",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "user_tag_categories_id_user_fkey",
                        column: x => x.id_user,
                        principalTable: "customuser",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tag_id_category",
                table: "tag",
                column: "id_category");

            migrationBuilder.CreateIndex(
                name: "IX_tag_category_name",
                table: "tag_category",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_tag_categories_id_category",
                table: "user_tag_categories",
                column: "id_category");

            migrationBuilder.CreateIndex(
                name: "IX_user_tag_categories_id_user_id_category",
                table: "user_tag_categories",
                columns: new[] { "id_user", "id_category" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "tag_id_category_fkey",
                table: "tag",
                column: "id_category",
                principalTable: "tag_category",
                principalColumn: "id_category",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.Sql("""
                INSERT INTO tag_category (name)
                VALUES ('к-поп'), ('хип-хоп'), ('рок')
                ON CONFLICT (name) DO NOTHING;

                INSERT INTO tag (name_tag, id_category)
                SELECT catalog.name_tag, category.id_category
                FROM (VALUES
                    ('к-поп', 'BTS'),
                    ('к-поп', 'BLACKPINK'),
                    ('к-поп', 'Stray Kids'),
                    ('к-поп', 'TWICE'),
                    ('хип-хоп', 'Jay-Z'),
                    ('хип-хоп', 'Kendrick Lamar'),
                    ('хип-хоп', 'Eminem'),
                    ('хип-хоп', 'Drake'),
                    ('рок', 'The Beatles'),
                    ('рок', 'Nirvana'),
                    ('рок', 'Queen'),
                    ('рок', 'Radiohead')
                ) AS catalog(category_name, name_tag)
                JOIN tag_category category ON category.name = catalog.category_name
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM tag existing
                    WHERE existing.name_tag = catalog.name_tag
                      AND existing.id_category = category.id_category
                );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "tag_id_category_fkey",
                table: "tag");

            migrationBuilder.DropTable(
                name: "user_tag_categories");

            migrationBuilder.DropTable(
                name: "tag_category");

            migrationBuilder.DropIndex(
                name: "IX_tag_id_category",
                table: "tag");

            migrationBuilder.DropColumn(
                name: "id_category",
                table: "tag");
        }
    }
}
