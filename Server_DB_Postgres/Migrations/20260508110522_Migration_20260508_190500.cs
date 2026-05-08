using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260508_190500 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "x_battlefields_base_npcs",
                schema: "game_data");

            migrationBuilder.DropTable(
                name: "base_npcs",
                schema: "game_data");

            migrationBuilder.AddColumn<bool>(
                name: "is_playable",
                schema: "game_data",
                table: "base_heroes",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateTable(
                name: "x_battlefields_base_heroes",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    battlefield_id = table.Column<int>(type: "integer", nullable: false),
                    base_hero_id = table.Column<int>(type: "integer", nullable: false),
                    guarant_spawn = table.Column<bool>(type: "boolean", nullable: false),
                    probability_spawn = table.Column<int>(type: "integer", nullable: false),
                    possible_rank = table.Column<bool>(type: "boolean", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("x_battlefields_base_heroes__pkey", x => x.id);
                    table.ForeignKey(
                        name: "x_battlefields_base_heroes__base_hero_id__base_heroes__fkey",
                        column: x => x.base_hero_id,
                        principalSchema: "game_data",
                        principalTable: "base_heroes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "x_battlefields_base_heroes__battlefield_id__battlefields__fkey",
                        column: x => x.battlefield_id,
                        principalSchema: "game_data",
                        principalTable: "battlefields",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "x_battlefields_base_heroes__base_hero_id__idx",
                schema: "game_data",
                table: "x_battlefields_base_heroes",
                column: "base_hero_id");

            migrationBuilder.CreateIndex(
                name: "x_battlefields_base_heroes__battlefield_id__idx",
                schema: "game_data",
                table: "x_battlefields_base_heroes",
                column: "battlefield_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "x_battlefields_base_heroes",
                schema: "game_data");

            migrationBuilder.DropColumn(
                name: "is_playable",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.CreateTable(
                name: "base_npcs",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    health = table.Column<float>(type: "real", nullable: false),
                    is_playable = table.Column<bool>(type: "boolean", nullable: false),
                    main_stat = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    rarity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("base_npcs__pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "x_battlefields_base_npcs",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    base_npc_id = table.Column<int>(type: "integer", nullable: false),
                    battlefield_id = table.Column<int>(type: "integer", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    guarant_spawn = table.Column<bool>(type: "boolean", nullable: false),
                    possible_rank = table.Column<bool>(type: "boolean", nullable: false),
                    probability_spawn = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("x_battlefields_base_npcs__pkey", x => x.id);
                    table.ForeignKey(
                        name: "x_battlefields_base_npcs__base_npc_id__base_npcs__fkey",
                        column: x => x.base_npc_id,
                        principalSchema: "game_data",
                        principalTable: "base_npcs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "x_battlefields_base_npcs__battlefield_id__battlefields__fkey",
                        column: x => x.battlefield_id,
                        principalSchema: "game_data",
                        principalTable: "battlefields",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "base_npcs__name__idx",
                schema: "game_data",
                table: "base_npcs",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "x_battlefields_base_npcs__base_npc_id__idx",
                schema: "game_data",
                table: "x_battlefields_base_npcs",
                column: "base_npc_id");

            migrationBuilder.CreateIndex(
                name: "x_battlefields_base_npcs__battlefield_id__idx",
                schema: "game_data",
                table: "x_battlefields_base_npcs",
                column: "battlefield_id");
        }
    }
}
