using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations;

#pragma warning disable
/// <inheritdoc />
public partial class Start : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "_main");

        migrationBuilder.EnsureSchema(
            name: "xcross");

        migrationBuilder.CreateTable(
            name: "creature_types",
            schema: "_main",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("creature_types_pkey", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "heroes",
            schema: "_main",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                _rarity = table.Column<int>(type: "integer", nullable: false),
                base_health = table.Column<float>(type: "real", nullable: false),
                base_attack = table.Column<float>(type: "real", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("heroes_pkey", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "x_hero_creature_type",
            schema: "xcross",
            columns: table => new
            {
                hero_id = table.Column<Guid>(type: "uuid", nullable: false),
                creature_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
            },
            constraints: table =>
            {
                table.PrimaryKey("x_hero_creature_type_pkey", x => new { x.hero_id, x.creature_type_id });
                table.ForeignKey(
                    name: "x_hero_creature_type_creature_type_id_creature_types_fkey",
                    column: x => x.creature_type_id,
                    principalSchema: "_main",
                    principalTable: "creature_types",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "x_hero_creature_type_hero_id_heroes_fkey",
                    column: x => x.hero_id,
                    principalSchema: "_main",
                    principalTable: "heroes",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "creature_types_name_idx",
            schema: "_main",
            table: "creature_types",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "heroes_name_idx",
            schema: "_main",
            table: "heroes",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "x_hero_creature_type_creature_type_id_idx",
            schema: "xcross",
            table: "x_hero_creature_type",
            column: "creature_type_id");

        migrationBuilder.CreateIndex(
            name: "x_hero_creature_type_hero_id_idx",
            schema: "xcross",
            table: "x_hero_creature_type",
            column: "hero_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "x_hero_creature_type",
            schema: "xcross");

        migrationBuilder.DropTable(
            name: "creature_types",
            schema: "_main");

        migrationBuilder.DropTable(
            name: "heroes",
            schema: "_main");
    }
}
