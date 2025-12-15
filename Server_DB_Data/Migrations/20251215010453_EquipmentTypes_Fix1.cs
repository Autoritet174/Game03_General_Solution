using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class EquipmentTypes_Fix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "spend_action_points",
                schema: "__lists",
                table: "EquipmentTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "spend_action_points",
                schema: "__lists",
                table: "EquipmentTypes",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
