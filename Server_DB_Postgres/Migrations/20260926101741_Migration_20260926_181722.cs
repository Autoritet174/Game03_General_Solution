using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260926_181722 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "x_base_heroes_abilities__base_hero_id__idx",
                schema: "game_data",
                table: "x_base_heroes_abilities");

            migrationBuilder.CreateIndex(
                name: "x_base_heroes_abilities__base_hero_id__ability_id__idx",
                schema: "game_data",
                table: "x_base_heroes_abilities",
                columns: new[] { "base_hero_id", "ability_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "x_base_heroes_abilities__base_hero_id__ability_id__idx",
                schema: "game_data",
                table: "x_base_heroes_abilities");

            migrationBuilder.CreateIndex(
                name: "x_base_heroes_abilities__base_hero_id__idx",
                schema: "game_data",
                table: "x_base_heroes_abilities",
                column: "base_hero_id");
        }
    }
}
