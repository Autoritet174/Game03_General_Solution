using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
