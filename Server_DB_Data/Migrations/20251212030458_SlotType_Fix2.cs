using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class SlotType_Fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "can_craft_jewelcrafting",
                schema: "__lists",
                table: "EquipmentType",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "can_craft_smithing",
                schema: "__lists",
                table: "EquipmentType",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "can_craft_jewelcrafting",
                schema: "__lists",
                table: "EquipmentType");

            migrationBuilder.DropColumn(
                name: "can_craft_smithing",
                schema: "__lists",
                table: "EquipmentType");
        }
    }
}
