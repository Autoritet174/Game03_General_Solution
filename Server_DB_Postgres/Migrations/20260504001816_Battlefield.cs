using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Battlefield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "x_dungeons_npcs",
                schema: "game_data");

            migrationBuilder.DropTable(
                name: "dungeons",
                schema: "game_data");

            migrationBuilder.CreateTable(
                name: "battlefields",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    level = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    difficulty = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("battlefields__pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "x_battlefield_npcs",
                schema: "game_data",
                columns: table => new
                {
                    battlefield_id = table.Column<int>(type: "integer", nullable: false),
                    npc_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("x_battlefield_npcs__pkey", x => new { x.battlefield_id, x.npc_id });
                    table.ForeignKey(
                        name: "x_battlefield_npcs__battlefield_id__battlefields__fkey",
                        column: x => x.battlefield_id,
                        principalSchema: "game_data",
                        principalTable: "battlefields",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "x_battlefield_npcs__npc_id__npcs__fkey",
                        column: x => x.npc_id,
                        principalSchema: "game_data",
                        principalTable: "npcs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "battlefields__name__idx",
                schema: "game_data",
                table: "battlefields",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "x_battlefield_npcs__npc_id__idx",
                schema: "game_data",
                table: "x_battlefield_npcs",
                column: "npc_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "x_battlefield_npcs",
                schema: "game_data");

            migrationBuilder.DropTable(
                name: "battlefields",
                schema: "game_data");

            migrationBuilder.CreateTable(
                name: "dungeons",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    difficulty = table.Column<int>(type: "integer", nullable: false),
                    level = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("dungeons__pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "x_dungeons_npcs",
                schema: "game_data",
                columns: table => new
                {
                    dungeon_id = table.Column<int>(type: "integer", nullable: false),
                    npc_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("x_dungeons_npcs__pkey", x => new { x.dungeon_id, x.npc_id });
                    table.ForeignKey(
                        name: "x_dungeons_npcs__dungeon_id__dungeons__fkey",
                        column: x => x.dungeon_id,
                        principalSchema: "game_data",
                        principalTable: "dungeons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "x_dungeons_npcs__npc_id__npcs__fkey",
                        column: x => x.npc_id,
                        principalSchema: "game_data",
                        principalTable: "npcs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "dungeons__name__idx",
                schema: "game_data",
                table: "dungeons",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "x_dungeons_npcs__npc_id__idx",
                schema: "game_data",
                table: "x_dungeons_npcs",
                column: "npc_id");
        }
    }
}
