using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260504_122914 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "x_battlefields_base_npcs__base_npc_id__npcs__fkey",
                schema: "game_data",
                table: "x_battlefields_base_npcs");

            migrationBuilder.DropPrimaryKey(
                name: "npcs__pkey",
                schema: "game_data",
                table: "npcs");

            migrationBuilder.RenameTable(
                name: "npcs",
                schema: "game_data",
                newName: "base_npcs",
                newSchema: "game_data");

            migrationBuilder.RenameIndex(
                name: "npcs__name__idx",
                schema: "game_data",
                table: "base_npcs",
                newName: "base_npcs__name__idx");

            migrationBuilder.AddPrimaryKey(
                name: "base_npcs__pkey",
                schema: "game_data",
                table: "base_npcs",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "x_battlefields_base_npcs__base_npc_id__base_npcs__fkey",
                schema: "game_data",
                table: "x_battlefields_base_npcs",
                column: "base_npc_id",
                principalSchema: "game_data",
                principalTable: "base_npcs",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "x_battlefields_base_npcs__base_npc_id__base_npcs__fkey",
                schema: "game_data",
                table: "x_battlefields_base_npcs");

            migrationBuilder.DropPrimaryKey(
                name: "base_npcs__pkey",
                schema: "game_data",
                table: "base_npcs");

            migrationBuilder.RenameTable(
                name: "base_npcs",
                schema: "game_data",
                newName: "npcs",
                newSchema: "game_data");

            migrationBuilder.RenameIndex(
                name: "base_npcs__name__idx",
                schema: "game_data",
                table: "npcs",
                newName: "npcs__name__idx");

            migrationBuilder.AddPrimaryKey(
                name: "npcs__pkey",
                schema: "game_data",
                table: "npcs",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "x_battlefields_base_npcs__base_npc_id__npcs__fkey",
                schema: "game_data",
                table: "x_battlefields_base_npcs",
                column: "base_npc_id",
                principalSchema: "game_data",
                principalTable: "npcs",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
