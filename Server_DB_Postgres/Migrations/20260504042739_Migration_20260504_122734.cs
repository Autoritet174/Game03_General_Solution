using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260504_122734 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "x_battlefields_npcs",
                schema: "game_data");

            migrationBuilder.RenameColumn(
                name: "health_base",
                schema: "game_data",
                table: "npcs",
                newName: "health");

            migrationBuilder.CreateTable(
                name: "x_battlefields_base_npcs",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    battlefield_id = table.Column<int>(type: "integer", nullable: false),
                    base_npc_id = table.Column<int>(type: "integer", nullable: false),
                    guarant_spawn = table.Column<bool>(type: "boolean", nullable: false),
                    probability_spawn = table.Column<int>(type: "integer", nullable: false),
                    possible_rank = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("x_battlefields_base_npcs__pkey", x => x.id);
                    table.ForeignKey(
                        name: "x_battlefields_base_npcs__base_npc_id__npcs__fkey",
                        column: x => x.base_npc_id,
                        principalSchema: "game_data",
                        principalTable: "npcs",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "x_battlefields_base_npcs",
                schema: "game_data");

            migrationBuilder.RenameColumn(
                name: "health",
                schema: "game_data",
                table: "npcs",
                newName: "health_base");

            migrationBuilder.CreateTable(
                name: "x_battlefields_npcs",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    battlefield_id = table.Column<int>(type: "integer", nullable: false),
                    npc_id = table.Column<int>(type: "integer", nullable: false),
                    guarant_spawn = table.Column<bool>(type: "boolean", nullable: false),
                    possible_rank = table.Column<bool>(type: "boolean", nullable: false),
                    probability_spawn = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("x_battlefields_npcs__pkey", x => x.id);
                    table.ForeignKey(
                        name: "x_battlefields_npcs__battlefield_id__battlefields__fkey",
                        column: x => x.battlefield_id,
                        principalSchema: "game_data",
                        principalTable: "battlefields",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "x_battlefields_npcs__npc_id__npcs__fkey",
                        column: x => x.npc_id,
                        principalSchema: "game_data",
                        principalTable: "npcs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "x_battlefields_npcs__battlefield_id__idx",
                schema: "game_data",
                table: "x_battlefields_npcs",
                column: "battlefield_id");

            migrationBuilder.CreateIndex(
                name: "x_battlefields_npcs__npc_id__idx",
                schema: "game_data",
                table: "x_battlefields_npcs",
                column: "npc_id");
        }
    }
}
