using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class EquipmentFix6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "equipments__hero_id__idx",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "in_alt_slot",
                schema: "collection",
                table: "equipments");

            migrationBuilder.AddColumn<bool>(
                name: "main_slot",
                schema: "game_data",
                table: "slots",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "slot_id",
                schema: "collection",
                table: "equipments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "equipments__hero_id__slot_id__idx",
                schema: "collection",
                table: "equipments",
                columns: new[] { "hero_id", "slot_id" });

            migrationBuilder.CreateIndex(
                name: "equipments__slot_id__idx",
                schema: "collection",
                table: "equipments",
                column: "slot_id");

            migrationBuilder.CreateIndex(
                name: "equipment_types__slot_type_id__idx",
                schema: "game_data",
                table: "equipment_types",
                column: "slot_type_id");

            migrationBuilder.AddForeignKey(
                name: "equipment_types__slot_type_id__slot_types__fkey",
                schema: "game_data",
                table: "equipment_types",
                column: "slot_type_id",
                principalSchema: "game_data",
                principalTable: "slot_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "equipments__slot_id__slots__fkey",
                schema: "collection",
                table: "equipments",
                column: "slot_id",
                principalSchema: "game_data",
                principalTable: "slots",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "equipment_types__slot_type_id__slot_types__fkey",
                schema: "game_data",
                table: "equipment_types");

            migrationBuilder.DropForeignKey(
                name: "equipments__slot_id__slots__fkey",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "equipments__hero_id__slot_id__idx",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "equipments__slot_id__idx",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "equipment_types__slot_type_id__idx",
                schema: "game_data",
                table: "equipment_types");

            migrationBuilder.DropColumn(
                name: "main_slot",
                schema: "game_data",
                table: "slots");

            migrationBuilder.DropColumn(
                name: "slot_id",
                schema: "collection",
                table: "equipments");

            migrationBuilder.AddColumn<bool>(
                name: "in_alt_slot",
                schema: "collection",
                table: "equipments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "equipments__hero_id__idx",
                schema: "collection",
                table: "equipments",
                column: "hero_id");
        }
    }
}
