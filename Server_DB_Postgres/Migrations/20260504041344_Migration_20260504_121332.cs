using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260504_121332 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "x_battlefield_npcs__battlefield_id__battlefields__fkey",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.DropForeignKey(
                name: "x_battlefield_npcs__npc_id__npcs__fkey",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.DropPrimaryKey(
                name: "x_battlefield_npcs__pkey",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.RenameTable(
                name: "x_battlefield_npcs",
                schema: "game_data",
                newName: "x_battlefields_npcs",
                newSchema: "game_data");

            migrationBuilder.RenameIndex(
                name: "x_battlefield_npcs__npc_id__idx",
                schema: "game_data",
                table: "x_battlefields_npcs",
                newName: "x_battlefields_npcs__npc_id__idx");

            migrationBuilder.RenameIndex(
                name: "x_battlefield_npcs__battlefield_id__idx",
                schema: "game_data",
                table: "x_battlefields_npcs",
                newName: "x_battlefields_npcs__battlefield_id__idx");

            migrationBuilder.AddPrimaryKey(
                name: "x_battlefields_npcs__pkey",
                schema: "game_data",
                table: "x_battlefields_npcs",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "x_battlefields_npcs__battlefield_id__battlefields__fkey",
                schema: "game_data",
                table: "x_battlefields_npcs",
                column: "battlefield_id",
                principalSchema: "game_data",
                principalTable: "battlefields",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "x_battlefields_npcs__npc_id__npcs__fkey",
                schema: "game_data",
                table: "x_battlefields_npcs",
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
                name: "x_battlefields_npcs__battlefield_id__battlefields__fkey",
                schema: "game_data",
                table: "x_battlefields_npcs");

            migrationBuilder.DropForeignKey(
                name: "x_battlefields_npcs__npc_id__npcs__fkey",
                schema: "game_data",
                table: "x_battlefields_npcs");

            migrationBuilder.DropPrimaryKey(
                name: "x_battlefields_npcs__pkey",
                schema: "game_data",
                table: "x_battlefields_npcs");

            migrationBuilder.RenameTable(
                name: "x_battlefields_npcs",
                schema: "game_data",
                newName: "x_battlefield_npcs",
                newSchema: "game_data");

            migrationBuilder.RenameIndex(
                name: "x_battlefields_npcs__npc_id__idx",
                schema: "game_data",
                table: "x_battlefield_npcs",
                newName: "x_battlefield_npcs__npc_id__idx");

            migrationBuilder.RenameIndex(
                name: "x_battlefields_npcs__battlefield_id__idx",
                schema: "game_data",
                table: "x_battlefield_npcs",
                newName: "x_battlefield_npcs__battlefield_id__idx");

            migrationBuilder.AddPrimaryKey(
                name: "x_battlefield_npcs__pkey",
                schema: "game_data",
                table: "x_battlefield_npcs",
                column: "id");

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
    }
}
