using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class EquipmentFix3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "equipments__slot_id__slots__fkey",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "equipments__slot_id__hero_id__idx",
                schema: "collection",
                table: "equipments");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "in_alt_slot",
                schema: "collection",
                table: "equipments");

            migrationBuilder.AddColumn<int>(
                name: "slot_id",
                schema: "collection",
                table: "equipments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "equipments__slot_id__hero_id__idx",
                schema: "collection",
                table: "equipments",
                columns: new[] { "slot_id", "hero_id" },
                unique: true);

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
    }
}
