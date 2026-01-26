using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "equipment_types__slot_type_id__slot_types__fkey",
                schema: "game_data",
                table: "equipment_types");

            migrationBuilder.DropIndex(
                name: "equipment_types__slot_type_id__idx",
                schema: "game_data",
                table: "equipment_types");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
