using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260504_113720 : Migration
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
                name: "guarant_spawn",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.DropColumn(
                name: "npc_id",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.DropColumn(
                name: "possible_rank",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.DropColumn(
                name: "probability_spawn",
                schema: "game_data",
                table: "x_battlefield_npcs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "battlefield_id",
                schema: "game_data",
                table: "x_battlefield_npcs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "guarant_spawn",
                schema: "game_data",
                table: "x_battlefield_npcs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "npc_id",
                schema: "game_data",
                table: "x_battlefield_npcs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "possible_rank",
                schema: "game_data",
                table: "x_battlefield_npcs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "probability_spawn",
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
    }
}
