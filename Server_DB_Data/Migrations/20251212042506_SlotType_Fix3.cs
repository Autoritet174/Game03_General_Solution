using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class SlotType_Fix3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "attack",
                schema: "__lists",
                table: "EquipmentType",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "spend_action_points",
                schema: "__lists",
                table: "EquipmentType",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "attack",
                schema: "__lists",
                table: "EquipmentType");

            migrationBuilder.DropColumn(
                name: "spend_action_points",
                schema: "__lists",
                table: "EquipmentType");
        }
    }
}
