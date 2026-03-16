using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260315_205349 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "versality_",
                schema: "collection",
                table: "heroes",
                newName: "versality");

            migrationBuilder.RenameColumn(
                name: "strength_",
                schema: "collection",
                table: "heroes",
                newName: "strength");

            migrationBuilder.RenameColumn(
                name: "intelligence_",
                schema: "collection",
                table: "heroes",
                newName: "intelligence");

            migrationBuilder.RenameColumn(
                name: "initiative_",
                schema: "collection",
                table: "heroes",
                newName: "initiative");

            migrationBuilder.RenameColumn(
                name: "health_",
                schema: "collection",
                table: "heroes",
                newName: "health");

            migrationBuilder.RenameColumn(
                name: "haste_",
                schema: "collection",
                table: "heroes",
                newName: "haste");

            migrationBuilder.RenameColumn(
                name: "endurance_physical_",
                schema: "collection",
                table: "heroes",
                newName: "endurance_physical");

            migrationBuilder.RenameColumn(
                name: "endurance_magical_",
                schema: "collection",
                table: "heroes",
                newName: "endurance_magical");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier_",
                schema: "collection",
                table: "heroes",
                newName: "crit_multiplier");

            migrationBuilder.RenameColumn(
                name: "crit_chance_",
                schema: "collection",
                table: "heroes",
                newName: "crit_chance");

            migrationBuilder.RenameColumn(
                name: "agility_",
                schema: "collection",
                table: "heroes",
                newName: "agility");

            migrationBuilder.RenameColumn(
                name: "versality_",
                schema: "collection",
                table: "equipments",
                newName: "versality");

            migrationBuilder.RenameColumn(
                name: "strength_",
                schema: "collection",
                table: "equipments",
                newName: "strength");

            migrationBuilder.RenameColumn(
                name: "intelligence_",
                schema: "collection",
                table: "equipments",
                newName: "intelligence");

            migrationBuilder.RenameColumn(
                name: "initiative_",
                schema: "collection",
                table: "equipments",
                newName: "initiative");

            migrationBuilder.RenameColumn(
                name: "health_",
                schema: "collection",
                table: "equipments",
                newName: "health");

            migrationBuilder.RenameColumn(
                name: "haste_",
                schema: "collection",
                table: "equipments",
                newName: "haste");

            migrationBuilder.RenameColumn(
                name: "endurance_physical_",
                schema: "collection",
                table: "equipments",
                newName: "endurance_physical");

            migrationBuilder.RenameColumn(
                name: "endurance_magical_",
                schema: "collection",
                table: "equipments",
                newName: "endurance_magical");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier_",
                schema: "collection",
                table: "equipments",
                newName: "crit_multiplier");

            migrationBuilder.RenameColumn(
                name: "crit_chance_",
                schema: "collection",
                table: "equipments",
                newName: "crit_chance");

            migrationBuilder.RenameColumn(
                name: "agility_",
                schema: "collection",
                table: "equipments",
                newName: "agility");

            migrationBuilder.RenameColumn(
                name: "versality_",
                schema: "game_data",
                table: "base_heroes",
                newName: "versality");

            migrationBuilder.RenameColumn(
                name: "strength_",
                schema: "game_data",
                table: "base_heroes",
                newName: "strength");

            migrationBuilder.RenameColumn(
                name: "intelligence_",
                schema: "game_data",
                table: "base_heroes",
                newName: "intelligence");

            migrationBuilder.RenameColumn(
                name: "initiative_",
                schema: "game_data",
                table: "base_heroes",
                newName: "initiative");

            migrationBuilder.RenameColumn(
                name: "health_",
                schema: "game_data",
                table: "base_heroes",
                newName: "health");

            migrationBuilder.RenameColumn(
                name: "haste_",
                schema: "game_data",
                table: "base_heroes",
                newName: "haste");

            migrationBuilder.RenameColumn(
                name: "endurance_physical_",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_physical");

            migrationBuilder.RenameColumn(
                name: "endurance_magical_",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_magical");

            migrationBuilder.RenameColumn(
                name: "damage_",
                schema: "game_data",
                table: "base_heroes",
                newName: "damage");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier_",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_multiplier");

            migrationBuilder.RenameColumn(
                name: "crit_chance_",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_chance");

            migrationBuilder.RenameColumn(
                name: "agility_",
                schema: "game_data",
                table: "base_heroes",
                newName: "agility");

            migrationBuilder.RenameColumn(
                name: "versality_",
                schema: "game_data",
                table: "base_equipments",
                newName: "versality");

            migrationBuilder.RenameColumn(
                name: "strength_",
                schema: "game_data",
                table: "base_equipments",
                newName: "strength");

            migrationBuilder.RenameColumn(
                name: "intelligence_",
                schema: "game_data",
                table: "base_equipments",
                newName: "intelligence");

            migrationBuilder.RenameColumn(
                name: "initiative_",
                schema: "game_data",
                table: "base_equipments",
                newName: "initiative");

            migrationBuilder.RenameColumn(
                name: "health_",
                schema: "game_data",
                table: "base_equipments",
                newName: "health");

            migrationBuilder.RenameColumn(
                name: "haste_",
                schema: "game_data",
                table: "base_equipments",
                newName: "haste");

            migrationBuilder.RenameColumn(
                name: "endurance_physical_",
                schema: "game_data",
                table: "base_equipments",
                newName: "endurance_physical");

            migrationBuilder.RenameColumn(
                name: "endurance_magical_",
                schema: "game_data",
                table: "base_equipments",
                newName: "endurance_magical");

            migrationBuilder.RenameColumn(
                name: "damage_",
                schema: "game_data",
                table: "base_equipments",
                newName: "damage");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier_",
                schema: "game_data",
                table: "base_equipments",
                newName: "crit_multiplier");

            migrationBuilder.RenameColumn(
                name: "crit_chance_",
                schema: "game_data",
                table: "base_equipments",
                newName: "crit_chance");

            migrationBuilder.RenameColumn(
                name: "agility_",
                schema: "game_data",
                table: "base_equipments",
                newName: "agility");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "versality",
                schema: "collection",
                table: "heroes",
                newName: "versality_");

            migrationBuilder.RenameColumn(
                name: "strength",
                schema: "collection",
                table: "heroes",
                newName: "strength_");

            migrationBuilder.RenameColumn(
                name: "intelligence",
                schema: "collection",
                table: "heroes",
                newName: "intelligence_");

            migrationBuilder.RenameColumn(
                name: "initiative",
                schema: "collection",
                table: "heroes",
                newName: "initiative_");

            migrationBuilder.RenameColumn(
                name: "health",
                schema: "collection",
                table: "heroes",
                newName: "health_");

            migrationBuilder.RenameColumn(
                name: "haste",
                schema: "collection",
                table: "heroes",
                newName: "haste_");

            migrationBuilder.RenameColumn(
                name: "endurance_physical",
                schema: "collection",
                table: "heroes",
                newName: "endurance_physical_");

            migrationBuilder.RenameColumn(
                name: "endurance_magical",
                schema: "collection",
                table: "heroes",
                newName: "endurance_magical_");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier",
                schema: "collection",
                table: "heroes",
                newName: "crit_multiplier_");

            migrationBuilder.RenameColumn(
                name: "crit_chance",
                schema: "collection",
                table: "heroes",
                newName: "crit_chance_");

            migrationBuilder.RenameColumn(
                name: "agility",
                schema: "collection",
                table: "heroes",
                newName: "agility_");

            migrationBuilder.RenameColumn(
                name: "versality",
                schema: "collection",
                table: "equipments",
                newName: "versality_");

            migrationBuilder.RenameColumn(
                name: "strength",
                schema: "collection",
                table: "equipments",
                newName: "strength_");

            migrationBuilder.RenameColumn(
                name: "intelligence",
                schema: "collection",
                table: "equipments",
                newName: "intelligence_");

            migrationBuilder.RenameColumn(
                name: "initiative",
                schema: "collection",
                table: "equipments",
                newName: "initiative_");

            migrationBuilder.RenameColumn(
                name: "health",
                schema: "collection",
                table: "equipments",
                newName: "health_");

            migrationBuilder.RenameColumn(
                name: "haste",
                schema: "collection",
                table: "equipments",
                newName: "haste_");

            migrationBuilder.RenameColumn(
                name: "endurance_physical",
                schema: "collection",
                table: "equipments",
                newName: "endurance_physical_");

            migrationBuilder.RenameColumn(
                name: "endurance_magical",
                schema: "collection",
                table: "equipments",
                newName: "endurance_magical_");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier",
                schema: "collection",
                table: "equipments",
                newName: "crit_multiplier_");

            migrationBuilder.RenameColumn(
                name: "crit_chance",
                schema: "collection",
                table: "equipments",
                newName: "crit_chance_");

            migrationBuilder.RenameColumn(
                name: "agility",
                schema: "collection",
                table: "equipments",
                newName: "agility_");

            migrationBuilder.RenameColumn(
                name: "versality",
                schema: "game_data",
                table: "base_heroes",
                newName: "versality_");

            migrationBuilder.RenameColumn(
                name: "strength",
                schema: "game_data",
                table: "base_heroes",
                newName: "strength_");

            migrationBuilder.RenameColumn(
                name: "intelligence",
                schema: "game_data",
                table: "base_heroes",
                newName: "intelligence_");

            migrationBuilder.RenameColumn(
                name: "initiative",
                schema: "game_data",
                table: "base_heroes",
                newName: "initiative_");

            migrationBuilder.RenameColumn(
                name: "health",
                schema: "game_data",
                table: "base_heroes",
                newName: "health_");

            migrationBuilder.RenameColumn(
                name: "haste",
                schema: "game_data",
                table: "base_heroes",
                newName: "haste_");

            migrationBuilder.RenameColumn(
                name: "endurance_physical",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_physical_");

            migrationBuilder.RenameColumn(
                name: "endurance_magical",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_magical_");

            migrationBuilder.RenameColumn(
                name: "damage",
                schema: "game_data",
                table: "base_heroes",
                newName: "damage_");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_multiplier_");

            migrationBuilder.RenameColumn(
                name: "crit_chance",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_chance_");

            migrationBuilder.RenameColumn(
                name: "agility",
                schema: "game_data",
                table: "base_heroes",
                newName: "agility_");

            migrationBuilder.RenameColumn(
                name: "versality",
                schema: "game_data",
                table: "base_equipments",
                newName: "versality_");

            migrationBuilder.RenameColumn(
                name: "strength",
                schema: "game_data",
                table: "base_equipments",
                newName: "strength_");

            migrationBuilder.RenameColumn(
                name: "intelligence",
                schema: "game_data",
                table: "base_equipments",
                newName: "intelligence_");

            migrationBuilder.RenameColumn(
                name: "initiative",
                schema: "game_data",
                table: "base_equipments",
                newName: "initiative_");

            migrationBuilder.RenameColumn(
                name: "health",
                schema: "game_data",
                table: "base_equipments",
                newName: "health_");

            migrationBuilder.RenameColumn(
                name: "haste",
                schema: "game_data",
                table: "base_equipments",
                newName: "haste_");

            migrationBuilder.RenameColumn(
                name: "endurance_physical",
                schema: "game_data",
                table: "base_equipments",
                newName: "endurance_physical_");

            migrationBuilder.RenameColumn(
                name: "endurance_magical",
                schema: "game_data",
                table: "base_equipments",
                newName: "endurance_magical_");

            migrationBuilder.RenameColumn(
                name: "damage",
                schema: "game_data",
                table: "base_equipments",
                newName: "damage_");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier",
                schema: "game_data",
                table: "base_equipments",
                newName: "crit_multiplier_");

            migrationBuilder.RenameColumn(
                name: "crit_chance",
                schema: "game_data",
                table: "base_equipments",
                newName: "crit_chance_");

            migrationBuilder.RenameColumn(
                name: "agility",
                schema: "game_data",
                table: "base_equipments",
                newName: "agility_");
        }
    }
}
