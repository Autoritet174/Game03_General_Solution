using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260407_112908 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "x_dungeons_npcs__npc_id__idx",
                schema: "game_data",
                table: "x_dungeons_npcs",
                column: "npc_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "x_dungeons_npcs",
                schema: "game_data");
        }
    }
}
