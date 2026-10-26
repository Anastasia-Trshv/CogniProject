using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cogni.Migrations
{
    public partial class HobbyGenreHierarchy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hobby",
                columns: table => new
                {
                    id_hobby = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false)
                },
                constraints: table => table.PrimaryKey("hobby_pkey", x => x.id_hobby));

            migrationBuilder.CreateIndex(
                name: "IX_hobby_name",
                table: "hobby",
                column: "name",
                unique: true);

            migrationBuilder.AddColumn<int>(
                name: "id_hobby",
                table: "tag_category",
                type: "integer",
                nullable: true);

            migrationBuilder.Sql("""
                INSERT INTO hobby (name)
                VALUES ('Творчество'), ('Медиа'), ('Музыка'), ('Другое')
                ON CONFLICT (name) DO NOTHING;

                INSERT INTO tag_category (name)
                SELECT DISTINCT tag.name_tag
                FROM tag
                JOIN tag_category old_category ON old_category.id_category = tag.id_category
                WHERE old_category.name IN ('Творчество', 'Медиа', 'Музыка')
                  AND tag.name_tag IS NOT NULL
                ON CONFLICT (name) DO NOTHING;

                UPDATE tag_category category
                SET id_hobby = hobby.id_hobby
                FROM hobby
                WHERE (hobby.name = 'Музыка' AND category.name IN ('к-поп', 'хип-хоп', 'рок'))
                   OR (hobby.name = 'Творчество' AND category.name IN ('рисование', 'бисероплетение', 'вязание'))
                   OR (hobby.name = 'Медиа' AND category.name IN ('видеоигры', 'сериалы'));

                UPDATE tag_category category
                SET id_hobby = hobby.id_hobby
                FROM hobby, tag old_tag
                JOIN tag_category old_category ON old_category.id_category = old_tag.id_category
                WHERE old_category.name IN ('Творчество', 'Медиа', 'Музыка')
                  AND old_tag.name_tag = category.name
                  AND hobby.name = old_category.name;

                UPDATE tag_category category
                SET id_hobby = hobby.id_hobby
                FROM hobby
                WHERE category.id_hobby IS NULL
                  AND hobby.name = 'Другое';

                INSERT INTO user_tag_categories (id_user, id_category, is_bookmarked)
                SELECT DISTINCT selection.id_user, category.id_category, FALSE
                FROM user_tags selection
                JOIN tag selected_tag ON selected_tag.id_tag = selection.id_tag
                JOIN tag_category old_category ON old_category.id_category = selected_tag.id_category
                JOIN tag_category category ON category.name = selected_tag.name_tag
                WHERE old_category.name IN ('Творчество', 'Медиа', 'Музыка')
                ON CONFLICT (id_user, id_category) DO NOTHING;

                DELETE FROM user_tags
                WHERE id_tag IN (
                    SELECT tag.id_tag
                    FROM tag
                    JOIN tag_category category ON category.id_category = tag.id_category
                    WHERE category.name IN ('Творчество', 'Медиа', 'Музыка')
                );

                DELETE FROM tag
                WHERE id_category IN (
                    SELECT id_category FROM tag_category
                    WHERE name IN ('Творчество', 'Медиа', 'Музыка')
                );

                DELETE FROM user_tag_categories
                WHERE id_category IN (
                    SELECT id_category FROM tag_category
                    WHERE name IN ('Творчество', 'Медиа', 'Музыка')
                );

                DELETE FROM tag_category
                WHERE name IN ('Творчество', 'Медиа', 'Музыка');

                ALTER TABLE tag_category
                    ALTER COLUMN id_hobby SET NOT NULL;

                ALTER TABLE tag_category
                    ADD CONSTRAINT tag_category_id_hobby_fkey
                    FOREIGN KEY (id_hobby) REFERENCES hobby (id_hobby) ON DELETE CASCADE;

                ALTER TABLE user_tag_categories
                    DROP COLUMN is_bookmarked;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_tag_category_id_hobby",
                table: "tag_category",
                column: "id_hobby");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_bookmarked",
                table: "user_tag_categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.DropForeignKey(
                name: "tag_category_id_hobby_fkey",
                table: "tag_category");
            migrationBuilder.DropIndex(
                name: "IX_tag_category_id_hobby",
                table: "tag_category");
            migrationBuilder.DropColumn(
                name: "id_hobby",
                table: "tag_category");
            migrationBuilder.DropTable(name: "hobby");
        }
    }
}
