using Cogni.Database.Context;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cogni.Migrations
{
    [DbContext(typeof(CogniDbContext))]
    [Migration("20261007114500_ConcreteHobbyExamples")]
    public partial class ConcreteHobbyExamples : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                INSERT INTO tag (name_tag, id_category)
                SELECT examples.name_tag, category.id_category
                FROM (VALUES
                    ('видеоигры', 'Minecraft'),
                    ('видеоигры', 'The Sims 4'),
                    ('видеоигры', 'Stardew Valley'),
                    ('видеоигры', 'Genshin Impact'),
                    ('видеоигры', 'Baldur''s Gate 3'),
                    ('сериалы', 'Очень странные дела'),
                    ('сериалы', 'Шерлок'),
                    ('сериалы', 'Во все тяжкие'),
                    ('сериалы', 'Аркейн'),
                    ('сериалы', 'Офис')
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
                DELETE FROM tag
                WHERE name_tag IN (
                    'Minecraft', 'The Sims 4', 'Stardew Valley', 'Genshin Impact',
                    'Baldur''s Gate 3', 'Очень странные дела', 'Шерлок',
                    'Во все тяжкие', 'Аркейн', 'Офис'
                )
                AND id_category IN (
                    SELECT id_category FROM tag_category
                    WHERE name IN ('видеоигры', 'сериалы')
                );
                """);
        }
    }
}
