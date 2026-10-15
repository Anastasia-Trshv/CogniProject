using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Cogni.Database.Context;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cogni.Migrations;

[DbContext(typeof(CogniDbContext))]
[Migration("20261013000000_AddSakinorvaCatalog")]
public partial class AddSakinorvaCatalog : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "sakinorva_catalog",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                json_content = table.Column<string>(type: "jsonb", nullable: false)
            },
            constraints: table => table.PrimaryKey("sakinorva_catalog_pkey", x => x.id));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "sakinorva_catalog");
    }
}
