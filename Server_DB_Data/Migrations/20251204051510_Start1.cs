using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class Start1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "__lists");

            migrationBuilder.EnsureSchema(
                name: "_heroes");

            migrationBuilder.EnsureSchema(
                name: "_equipment");

            migrationBuilder.EnsureSchema(
                name: "x_cross");

            migrationBuilder.CreateTable(
                name: "creature_types",
                schema: "__lists",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("creature_types_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "heroes",
                schema: "_heroes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    rarity = table.Column<int>(type: "integer", nullable: false),
                    is_unique = table.Column<bool>(type: "boolean", nullable: false),
                    health = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    damage = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("heroes_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "types_damage",
                schema: "__lists",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("types_damage_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "x_hero_creature_type",
                schema: "x_cross",
                columns: table => new
                {
                    hero_id = table.Column<int>(type: "integer", nullable: false),
                    creature_type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("x_hero_creature_type_pkey", x => new { x.hero_id, x.creature_type_id });
                    table.ForeignKey(
                        name: "x_hero_creature_type_creature_type_id_creature_types_fkey",
                        column: x => x.creature_type_id,
                        principalSchema: "__lists",
                        principalTable: "creature_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "x_hero_creature_type_hero_id_heroes_fkey",
                        column: x => x.hero_id,
                        principalSchema: "_heroes",
                        principalTable: "heroes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sword",
                schema: "_equipment",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    rarity = table.Column<int>(type: "integer", nullable: false),
                    is_unique = table.Column<bool>(type: "boolean", nullable: false),
                    damage = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    type_damage_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sword_pkey", x => x.id);
                    table.ForeignKey(
                        name: "sword_type_damage_id_types_damage_fkey",
                        column: x => x.type_damage_id,
                        principalSchema: "__lists",
                        principalTable: "types_damage",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "creature_types_name_idx",
                schema: "__lists",
                table: "creature_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "heroes_name_idx",
                schema: "_heroes",
                table: "heroes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "sword_name_idx",
                schema: "_equipment",
                table: "sword",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "sword_type_damage_id_idx",
                schema: "_equipment",
                table: "sword",
                column: "type_damage_id");

            migrationBuilder.CreateIndex(
                name: "types_damage_name_idx",
                schema: "__lists",
                table: "types_damage",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "x_hero_creature_type_creature_type_id_idx",
                schema: "x_cross",
                table: "x_hero_creature_type",
                column: "creature_type_id");

            migrationBuilder.CreateIndex(
                name: "x_hero_creature_type_hero_id_idx",
                schema: "x_cross",
                table: "x_hero_creature_type",
                column: "hero_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sword",
                schema: "_equipment");

            migrationBuilder.DropTable(
                name: "x_hero_creature_type",
                schema: "x_cross");

            migrationBuilder.DropTable(
                name: "types_damage",
                schema: "__lists");

            migrationBuilder.DropTable(
                name: "creature_types",
                schema: "__lists");

            migrationBuilder.DropTable(
                name: "heroes",
                schema: "_heroes");
        }
    }
}
