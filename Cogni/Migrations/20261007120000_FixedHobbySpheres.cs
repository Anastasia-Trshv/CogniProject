using Cogni.Database.Context;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cogni.Migrations
{
    [DbContext(typeof(CogniDbContext))]
    [Migration("20261007120000_FixedHobbySpheres")]
    public partial class FixedHobbySpheres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                INSERT INTO hobby (name)
                VALUES ('Игры'), ('Спорт')
                ON CONFLICT (name) DO NOTHING;

                UPDATE tag_category category
                SET id_hobby = hobby.id_hobby
                FROM hobby
                WHERE category.name = 'видеоигры'
                  AND hobby.name = 'Игры';

                INSERT INTO tag_category (name, id_hobby)
                SELECT categories.name, hobby.id_hobby
                FROM (VALUES ('бег'), ('фитнес'), ('футбол')) AS categories(name)
                CROSS JOIN hobby
                WHERE hobby.name = 'Спорт'
                ON CONFLICT (name) DO NOTHING;

                INSERT INTO tag (name_tag, id_category)
                SELECT examples.name_tag, category.id_category
                FROM (VALUES
                    ('бег', '5 км'),
                    ('бег', 'марафон'),
                    ('фитнес', 'силовые тренировки'),
                    ('фитнес', 'йога'),
                    ('футбол', 'Чемпионат мира'),
                    ('футбол', 'Лига чемпионов')
                ) AS examples(category_name, name_tag)
                JOIN tag_category category ON category.name = examples.category_name
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM tag existing
                    WHERE existing.id_category = category.id_category
                      AND existing.name_tag = examples.name_tag
                );

                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE tag_category
                SET id_hobby = (SELECT id_hobby FROM hobby WHERE name = 'Медиа')
                WHERE name = 'видеоигры';

                DELETE FROM user_tags
                WHERE id_tag IN (
                    SELECT tag.id_tag
                    FROM tag
                    JOIN tag_category category ON category.id_category = tag.id_category
                    WHERE category.id_hobby = (SELECT id_hobby FROM hobby WHERE name = 'Спорт')
                );

                DELETE FROM user_tag_categories
                WHERE id_category IN (
                    SELECT id_category
                    FROM tag_category
                    WHERE id_hobby = (SELECT id_hobby FROM hobby WHERE name = 'Спорт')
                );

                DELETE FROM tag
                WHERE id_category IN (
                    SELECT id_category
                    FROM tag_category
                    WHERE id_hobby = (SELECT id_hobby FROM hobby WHERE name = 'Спорт')
                );

                DELETE FROM tag_category
                WHERE id_hobby = (SELECT id_hobby FROM hobby WHERE name = 'Спорт');

                DELETE FROM hobby
                WHERE name IN ('Игры', 'Спорт');
                """);
        }
    }
}
