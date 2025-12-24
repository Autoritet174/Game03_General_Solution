using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "x_hero_creature_type__hero_id__base_heroes__fkey",
                schema: "game_data",
                table: "x_hero_creature_type");

            migrationBuilder.RenameColumn(
                name: "hero_id",
                schema: "game_data",
                table: "x_hero_creature_type",
                newName: "base_hero_id");

            migrationBuilder.AddForeignKey(
                name: "x_hero_creature_type__base_hero_id__base_heroes__fkey",
                schema: "game_data",
                table: "x_hero_creature_type",
                column: "base_hero_id",
                principalSchema: "game_data",
                principalTable: "base_heroes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "x_hero_creature_type__base_hero_id__base_heroes__fkey",
                schema: "game_data",
                table: "x_hero_creature_type");

            migrationBuilder.RenameColumn(
                name: "base_hero_id",
                schema: "game_data",
                table: "x_hero_creature_type",
                newName: "hero_id");

            migrationBuilder.AddForeignKey(
                name: "x_hero_creature_type__hero_id__base_heroes__fkey",
                schema: "game_data",
                table: "x_hero_creature_type",
                column: "hero_id",
                principalSchema: "game_data",
                principalTable: "base_heroes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
