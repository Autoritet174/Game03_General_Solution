using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class SlotType_Fix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "EquipmentType_slot_type_id_slot_type_fkey",
                schema: "__lists",
                table: "EquipmentType");

            migrationBuilder.AlterColumn<int>(
                name: "slot_type_id",
                schema: "__lists",
                table: "EquipmentType",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "EquipmentType_slot_type_id_slot_type_fkey",
                schema: "__lists",
                table: "EquipmentType",
                column: "slot_type_id",
                principalSchema: "__lists",
                principalTable: "SlotType",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "EquipmentType_slot_type_id_slot_type_fkey",
                schema: "__lists",
                table: "EquipmentType");

            migrationBuilder.AlterColumn<int>(
                name: "slot_type_id",
                schema: "__lists",
                table: "EquipmentType",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "EquipmentType_slot_type_id_slot_type_fkey",
                schema: "__lists",
                table: "EquipmentType",
                column: "slot_type_id",
                principalSchema: "__lists",
                principalTable: "SlotType",
                principalColumn: "id");
        }
    }
}
