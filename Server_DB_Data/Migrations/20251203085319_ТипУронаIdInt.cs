using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class ТипУронаIdInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "sword__type_damage_id_type_damage_fkey",
                schema: "equipment",
                table: "sword");

            migrationBuilder.DropIndex(
                name: "sword__type_damage_id_idx",
                schema: "equipment",
                table: "sword");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "sword__type_damage_id_idx",
                schema: "equipment",
                table: "sword",
                column: "_type_damage_id");

            migrationBuilder.AddForeignKey(
                name: "sword__type_damage_id_type_damage_fkey",
                schema: "equipment",
                table: "sword",
                column: "_type_damage_id",
                principalSchema: "_main",
                principalTable: "type_damage",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
