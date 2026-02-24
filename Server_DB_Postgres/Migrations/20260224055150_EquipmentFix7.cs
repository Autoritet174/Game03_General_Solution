using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class EquipmentFix7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "equipments__hero_id__slot_id__idx",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "equipments__user_id__idx",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "rarity",
                schema: "collection",
                table: "heroes");

            migrationBuilder.CreateIndex(
                name: "equipments__hero_id__slot_id__idx",
                schema: "collection",
                table: "equipments",
                columns: new[] { "hero_id", "slot_id" },
                unique: true,
                filter: "hero_id IS NOT NULL AND slot_id IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "equipments__user_id__idx",
                schema: "collection",
                table: "equipments",
                column: "user_id",
                filter: "hero_id IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "equipments__hero_id__slot_id__idx",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "equipments__user_id__idx",
                schema: "collection",
                table: "equipments");

            migrationBuilder.AddColumn<int>(
                name: "rarity",
                schema: "collection",
                table: "heroes",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "equipments__hero_id__slot_id__idx",
                schema: "collection",
                table: "equipments",
                columns: new[] { "hero_id", "slot_id" });

            migrationBuilder.CreateIndex(
                name: "equipments__user_id__idx",
                schema: "collection",
                table: "equipments",
                column: "user_id");
        }
    }
}
