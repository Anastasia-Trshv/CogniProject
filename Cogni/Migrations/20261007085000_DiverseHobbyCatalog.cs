using Cogni.Database.Context;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cogni.Migrations
{
    [DbContext(typeof(CogniDbContext))]
    [Migration("20261007085000_DiverseHobbyCatalog")]
    public partial class DiverseHobbyCatalog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                INSERT INTO tag_category (name)
                VALUES ('Творчество'), ('Медиа'), ('Музыка')
                ON CONFLICT (name) DO NOTHING;

                WITH catalog(position, category_name, name_tag) AS (
                    VALUES
                        (1, 'Творчество', 'рисование'),
                        (2, 'Медиа', 'видеоигры'),
                        (3, 'Медиа', 'сериалы'),
                        (4, 'Музыка', 'рок'),
                        (5, 'Творчество', 'бисероплетение'),
                        (6, 'Творчество', 'вязание')
                )
                INSERT INTO tag (name_tag, id_category)
                SELECT catalog.name_tag, category.id_category
                FROM catalog
                JOIN tag_category category ON category.name = catalog.category_name
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM tag existing
                    WHERE existing.name_tag = catalog.name_tag
                      AND existing.id_category = category.id_category
                )
                ORDER BY catalog.position;

                INSERT INTO user_tag_categories (id_user, id_category, is_bookmarked)
                SELECT user_account.id_user, category.id_category, TRUE
                FROM customuser user_account
                CROSS JOIN tag_category category
                WHERE user_account.email = 'alex.demo@cogni.local'
                  AND category.name IN ('Творчество', 'Медиа', 'Музыка')
                ON CONFLICT (id_user, id_category)
                DO UPDATE SET is_bookmarked = TRUE;

                WITH sample_tags(position, category_name, name_tag) AS (
                    VALUES
                        (1, 'Творчество', 'рисование'),
                        (2, 'Медиа', 'видеоигры'),
                        (3, 'Медиа', 'сериалы'),
                        (4, 'Музыка', 'рок'),
                        (5, 'Творчество', 'бисероплетение'),
                        (6, 'Творчество', 'вязание')
                )
                INSERT INTO user_tags (id_user, id_tag)
                SELECT user_account.id_user, tag.id_tag
                FROM customuser user_account
                CROSS JOIN sample_tags
                JOIN tag_category category ON category.name = sample_tags.category_name
                JOIN tag ON tag.id_category = category.id_category AND tag.name_tag = sample_tags.name_tag
                WHERE user_account.email = 'alex.demo@cogni.local'
                  AND NOT EXISTS (
                      SELECT 1
                      FROM user_tags existing
                      WHERE existing.id_user = user_account.id_user
                        AND existing.id_tag = tag.id_tag
                  )
                ORDER BY sample_tags.position;
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Keep catalog entries and selections because they may be used by profiles after migration.
        }
    }
}
