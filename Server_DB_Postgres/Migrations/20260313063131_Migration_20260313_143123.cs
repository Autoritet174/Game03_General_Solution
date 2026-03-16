using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260313_143123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "versality1000",
                schema: "collection",
                table: "heroes",
                newName: "versality");

            migrationBuilder.RenameColumn(
                name: "strength1000",
                schema: "collection",
                table: "heroes",
                newName: "strength");

            migrationBuilder.RenameColumn(
                name: "intelligence1000",
                schema: "collection",
                table: "heroes",
                newName: "intelligence");

            migrationBuilder.RenameColumn(
                name: "initiative1000",
                schema: "collection",
                table: "heroes",
                newName: "initiative");

            migrationBuilder.RenameColumn(
                name: "health1000",
                schema: "collection",
                table: "heroes",
                newName: "health");

            migrationBuilder.RenameColumn(
                name: "haste1000",
                schema: "collection",
                table: "heroes",
                newName: "haste");

            migrationBuilder.RenameColumn(
                name: "endurance_physical1000",
                schema: "collection",
                table: "heroes",
                newName: "endurance_physical");

            migrationBuilder.RenameColumn(
                name: "endurance_magical1000",
                schema: "collection",
                table: "heroes",
                newName: "endurance_magical");

            migrationBuilder.RenameColumn(
                name: "crit_power1000",
                schema: "collection",
                table: "heroes",
                newName: "crit_power");

            migrationBuilder.RenameColumn(
                name: "crit_chance1000",
                schema: "collection",
                table: "heroes",
                newName: "crit_chance");

            migrationBuilder.RenameColumn(
                name: "agility1000",
                schema: "collection",
                table: "heroes",
                newName: "agility");

            migrationBuilder.RenameColumn(
                name: "versality1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "versality");

            migrationBuilder.RenameColumn(
                name: "strength1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "strength");

            migrationBuilder.RenameColumn(
                name: "intelligence1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "intelligence");

            migrationBuilder.RenameColumn(
                name: "health1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "health");

            migrationBuilder.RenameColumn(
                name: "haste1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "haste");

            migrationBuilder.RenameColumn(
                name: "endurance_physical1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_physical");

            migrationBuilder.RenameColumn(
                name: "endurance_magical1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_magical");

            migrationBuilder.RenameColumn(
                name: "damage1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "damage");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_multiplier");

            migrationBuilder.RenameColumn(
                name: "crit_chance1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_chance");

            migrationBuilder.RenameColumn(
                name: "agility1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "agility");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "versality",
                schema: "collection",
                table: "heroes",
                newName: "versality1000");

            migrationBuilder.RenameColumn(
                name: "strength",
                schema: "collection",
                table: "heroes",
                newName: "strength1000");

            migrationBuilder.RenameColumn(
                name: "intelligence",
                schema: "collection",
                table: "heroes",
                newName: "intelligence1000");

            migrationBuilder.RenameColumn(
                name: "initiative",
                schema: "collection",
                table: "heroes",
                newName: "initiative1000");

            migrationBuilder.RenameColumn(
                name: "health",
                schema: "collection",
                table: "heroes",
                newName: "health1000");

            migrationBuilder.RenameColumn(
                name: "haste",
                schema: "collection",
                table: "heroes",
                newName: "haste1000");

            migrationBuilder.RenameColumn(
                name: "endurance_physical",
                schema: "collection",
                table: "heroes",
                newName: "endurance_physical1000");

            migrationBuilder.RenameColumn(
                name: "endurance_magical",
                schema: "collection",
                table: "heroes",
                newName: "endurance_magical1000");

            migrationBuilder.RenameColumn(
                name: "crit_power",
                schema: "collection",
                table: "heroes",
                newName: "crit_power1000");

            migrationBuilder.RenameColumn(
                name: "crit_chance",
                schema: "collection",
                table: "heroes",
                newName: "crit_chance1000");

            migrationBuilder.RenameColumn(
                name: "agility",
                schema: "collection",
                table: "heroes",
                newName: "agility1000");

            migrationBuilder.RenameColumn(
                name: "versality",
                schema: "game_data",
                table: "base_heroes",
                newName: "versality1000");

            migrationBuilder.RenameColumn(
                name: "strength",
                schema: "game_data",
                table: "base_heroes",
                newName: "strength1000");

            migrationBuilder.RenameColumn(
                name: "intelligence",
                schema: "game_data",
                table: "base_heroes",
                newName: "intelligence1000");

            migrationBuilder.RenameColumn(
                name: "health",
                schema: "game_data",
                table: "base_heroes",
                newName: "health1000");

            migrationBuilder.RenameColumn(
                name: "haste",
                schema: "game_data",
                table: "base_heroes",
                newName: "haste1000");

            migrationBuilder.RenameColumn(
                name: "endurance_physical",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_physical1000");

            migrationBuilder.RenameColumn(
                name: "endurance_magical",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_magical1000");

            migrationBuilder.RenameColumn(
                name: "damage",
                schema: "game_data",
                table: "base_heroes",
                newName: "damage1000");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_multiplier1000");

            migrationBuilder.RenameColumn(
                name: "crit_chance",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_chance1000");

            migrationBuilder.RenameColumn(
                name: "agility",
                schema: "game_data",
                table: "base_heroes",
                newName: "agility1000");
        }
    }
}
