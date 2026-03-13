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
                newName: "versality_1000");

            migrationBuilder.RenameColumn(
                name: "strength1000",
                schema: "collection",
                table: "heroes",
                newName: "strength_1000");

            migrationBuilder.RenameColumn(
                name: "intelligence1000",
                schema: "collection",
                table: "heroes",
                newName: "intelligence_1000");

            migrationBuilder.RenameColumn(
                name: "initiative1000",
                schema: "collection",
                table: "heroes",
                newName: "initiative_1000");

            migrationBuilder.RenameColumn(
                name: "health1000",
                schema: "collection",
                table: "heroes",
                newName: "health_1000");

            migrationBuilder.RenameColumn(
                name: "haste1000",
                schema: "collection",
                table: "heroes",
                newName: "haste_1000");

            migrationBuilder.RenameColumn(
                name: "endurance_physical1000",
                schema: "collection",
                table: "heroes",
                newName: "endurance_physical_1000");

            migrationBuilder.RenameColumn(
                name: "endurance_magical1000",
                schema: "collection",
                table: "heroes",
                newName: "endurance_magical_1000");

            migrationBuilder.RenameColumn(
                name: "crit_power1000",
                schema: "collection",
                table: "heroes",
                newName: "crit_power_1000");

            migrationBuilder.RenameColumn(
                name: "crit_chance1000",
                schema: "collection",
                table: "heroes",
                newName: "crit_chance_1000");

            migrationBuilder.RenameColumn(
                name: "agility1000",
                schema: "collection",
                table: "heroes",
                newName: "agility_1000");

            migrationBuilder.RenameColumn(
                name: "versality1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "versality_1000");

            migrationBuilder.RenameColumn(
                name: "strength1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "strength_1000");

            migrationBuilder.RenameColumn(
                name: "intelligence1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "intelligence_1000");

            migrationBuilder.RenameColumn(
                name: "health1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "health_1000");

            migrationBuilder.RenameColumn(
                name: "haste1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "haste_1000");

            migrationBuilder.RenameColumn(
                name: "endurance_physical1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_physical_1000");

            migrationBuilder.RenameColumn(
                name: "endurance_magical1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_magical_1000");

            migrationBuilder.RenameColumn(
                name: "damage1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "damage_1000");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_multiplier_1000");

            migrationBuilder.RenameColumn(
                name: "crit_chance1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_chance_1000");

            migrationBuilder.RenameColumn(
                name: "agility1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "agility_1000");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "versality_1000",
                schema: "collection",
                table: "heroes",
                newName: "versality1000");

            migrationBuilder.RenameColumn(
                name: "strength_1000",
                schema: "collection",
                table: "heroes",
                newName: "strength1000");

            migrationBuilder.RenameColumn(
                name: "intelligence_1000",
                schema: "collection",
                table: "heroes",
                newName: "intelligence1000");

            migrationBuilder.RenameColumn(
                name: "initiative_1000",
                schema: "collection",
                table: "heroes",
                newName: "initiative1000");

            migrationBuilder.RenameColumn(
                name: "health_1000",
                schema: "collection",
                table: "heroes",
                newName: "health1000");

            migrationBuilder.RenameColumn(
                name: "haste_1000",
                schema: "collection",
                table: "heroes",
                newName: "haste1000");

            migrationBuilder.RenameColumn(
                name: "endurance_physical_1000",
                schema: "collection",
                table: "heroes",
                newName: "endurance_physical1000");

            migrationBuilder.RenameColumn(
                name: "endurance_magical_1000",
                schema: "collection",
                table: "heroes",
                newName: "endurance_magical1000");

            migrationBuilder.RenameColumn(
                name: "crit_power_1000",
                schema: "collection",
                table: "heroes",
                newName: "crit_power1000");

            migrationBuilder.RenameColumn(
                name: "crit_chance_1000",
                schema: "collection",
                table: "heroes",
                newName: "crit_chance1000");

            migrationBuilder.RenameColumn(
                name: "agility_1000",
                schema: "collection",
                table: "heroes",
                newName: "agility1000");

            migrationBuilder.RenameColumn(
                name: "versality_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "versality1000");

            migrationBuilder.RenameColumn(
                name: "strength_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "strength1000");

            migrationBuilder.RenameColumn(
                name: "intelligence_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "intelligence1000");

            migrationBuilder.RenameColumn(
                name: "health_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "health1000");

            migrationBuilder.RenameColumn(
                name: "haste_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "haste1000");

            migrationBuilder.RenameColumn(
                name: "endurance_physical_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_physical1000");

            migrationBuilder.RenameColumn(
                name: "endurance_magical_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_magical1000");

            migrationBuilder.RenameColumn(
                name: "damage_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "damage1000");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_multiplier1000");

            migrationBuilder.RenameColumn(
                name: "crit_chance_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_chance1000");

            migrationBuilder.RenameColumn(
                name: "agility_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "agility1000");
        }
    }
}
