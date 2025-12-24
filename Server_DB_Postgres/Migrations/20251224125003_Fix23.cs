using General.DTO;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "equipments__smithing_material_id__smithing_materials__fkey",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "equipments__smithing_material_id__idx",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "smithing_material_id",
                schema: "collection",
                table: "equipments");

            migrationBuilder.RenameColumn(
                name: "stats",
                schema: "game_data",
                table: "base_heroes",
                newName: "health");

            migrationBuilder.RenameColumn(
                name: "stats",
                schema: "game_data",
                table: "base_equipments",
                newName: "health");

            migrationBuilder.AddColumn<Dice>(
                name: "damage",
                schema: "game_data",
                table: "base_heroes",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<Dice>(
                name: "damage",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "smithing_material_id",
                schema: "game_data",
                table: "base_equipments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "base_equipments__smithing_material_id__idx",
                schema: "game_data",
                table: "base_equipments",
                column: "smithing_material_id");

            migrationBuilder.AddForeignKey(
                name: "base_equipments__smithing_material_id__smithing_materials__fkey",
                schema: "game_data",
                table: "base_equipments",
                column: "smithing_material_id",
                principalSchema: "game_data",
                principalTable: "smithing_materials",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "base_equipments__smithing_material_id__smithing_materials__fkey",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropIndex(
                name: "base_equipments__smithing_material_id__idx",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "damage",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.DropColumn(
                name: "damage",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "smithing_material_id",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.RenameColumn(
                name: "health",
                schema: "game_data",
                table: "base_heroes",
                newName: "stats");

            migrationBuilder.RenameColumn(
                name: "health",
                schema: "game_data",
                table: "base_equipments",
                newName: "stats");

            migrationBuilder.AddColumn<int>(
                name: "smithing_material_id",
                schema: "collection",
                table: "equipments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "equipments__smithing_material_id__idx",
                schema: "collection",
                table: "equipments",
                column: "smithing_material_id");

            migrationBuilder.AddForeignKey(
                name: "equipments__smithing_material_id__smithing_materials__fkey",
                schema: "collection",
                table: "equipments",
                column: "smithing_material_id",
                principalSchema: "game_data",
                principalTable: "smithing_materials",
                principalColumn: "id");
        }
    }
}
