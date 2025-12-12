using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class SlotType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "slot_type_id",
                schema: "__lists",
                table: "EquipmentType",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SlotType",
                schema: "__lists",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    name_ru = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SlotType_pkey", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "EquipmentType_slot_type_id_idx",
                schema: "__lists",
                table: "EquipmentType",
                column: "slot_type_id");

            migrationBuilder.CreateIndex(
                name: "SlotType_name_idx",
                schema: "__lists",
                table: "SlotType",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "EquipmentType_slot_type_id_slot_type_fkey",
                schema: "__lists",
                table: "EquipmentType",
                column: "slot_type_id",
                principalSchema: "__lists",
                principalTable: "SlotType",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "EquipmentType_slot_type_id_slot_type_fkey",
                schema: "__lists",
                table: "EquipmentType");

            migrationBuilder.DropTable(
                name: "SlotType",
                schema: "__lists");

            migrationBuilder.DropIndex(
                name: "EquipmentType_slot_type_id_idx",
                schema: "__lists",
                table: "EquipmentType");

            migrationBuilder.DropColumn(
                name: "slot_type_id",
                schema: "__lists",
                table: "EquipmentType");
        }
    }
}
