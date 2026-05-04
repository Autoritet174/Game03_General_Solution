using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260504_113805 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "battlefield_id",
                schema: "game_data",
                table: "x_battlefield_npcs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "npc_id",
                schema: "game_data",
                table: "x_battlefield_npcs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "x_battlefield_npcs__battlefield_id__idx",
                schema: "game_data",
                table: "x_battlefield_npcs",
                column: "battlefield_id");

            migrationBuilder.CreateIndex(
                name: "x_battlefield_npcs__npc_id__idx",
                schema: "game_data",
                table: "x_battlefield_npcs",
                column: "npc_id");

            migrationBuilder.AddForeignKey(
                name: "x_battlefield_npcs__battlefield_id__battlefields__fkey",
                schema: "game_data",
                table: "x_battlefield_npcs",
                column: "battlefield_id",
                principalSchema: "game_data",
                principalTable: "battlefields",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "x_battlefield_npcs__npc_id__npcs__fkey",
                schema: "game_data",
                table: "x_battlefield_npcs",
                column: "npc_id",
                principalSchema: "game_data",
                principalTable: "npcs",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "x_battlefield_npcs__battlefield_id__battlefields__fkey",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.DropForeignKey(
                name: "x_battlefield_npcs__npc_id__npcs__fkey",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.DropIndex(
                name: "x_battlefield_npcs__battlefield_id__idx",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.DropIndex(
                name: "x_battlefield_npcs__npc_id__idx",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.DropColumn(
                name: "battlefield_id",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.DropColumn(
                name: "npc_id",
                schema: "game_data",
                table: "x_battlefield_npcs");
        }
    }
}
