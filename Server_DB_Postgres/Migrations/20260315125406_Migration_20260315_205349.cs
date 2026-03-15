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
                name: "versality__1000",
                schema: "collection",
                table: "heroes",
                newName: "versality_1000");

            migrationBuilder.RenameColumn(
                name: "strength__1000",
                schema: "collection",
                table: "heroes",
                newName: "strength_1000");

            migrationBuilder.RenameColumn(
                name: "intelligence__1000",
                schema: "collection",
                table: "heroes",
                newName: "intelligence_1000");

            migrationBuilder.RenameColumn(
                name: "initiative__1000",
                schema: "collection",
                table: "heroes",
                newName: "initiative_1000");

            migrationBuilder.RenameColumn(
                name: "health__1000",
                schema: "collection",
                table: "heroes",
                newName: "health_1000");

            migrationBuilder.RenameColumn(
                name: "haste__1000",
                schema: "collection",
                table: "heroes",
                newName: "haste_1000");

            migrationBuilder.RenameColumn(
                name: "endurance_physical__1000",
                schema: "collection",
                table: "heroes",
                newName: "endurance_physical_1000");

            migrationBuilder.RenameColumn(
                name: "endurance_magical__1000",
                schema: "collection",
                table: "heroes",
                newName: "endurance_magical_1000");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier__1000",
                schema: "collection",
                table: "heroes",
                newName: "crit_multiplier_1000");

            migrationBuilder.RenameColumn(
                name: "crit_chance__1000",
                schema: "collection",
                table: "heroes",
                newName: "crit_chance_1000");

            migrationBuilder.RenameColumn(
                name: "agility__1000",
                schema: "collection",
                table: "heroes",
                newName: "agility_1000");

            migrationBuilder.RenameColumn(
                name: "versality__1000",
                schema: "collection",
                table: "equipments",
                newName: "versality_1000");

            migrationBuilder.RenameColumn(
                name: "strength__1000",
                schema: "collection",
                table: "equipments",
                newName: "strength_1000");

            migrationBuilder.RenameColumn(
                name: "intelligence__1000",
                schema: "collection",
                table: "equipments",
                newName: "intelligence_1000");

            migrationBuilder.RenameColumn(
                name: "initiative__1000",
                schema: "collection",
                table: "equipments",
                newName: "initiative_1000");

            migrationBuilder.RenameColumn(
                name: "health__1000",
                schema: "collection",
                table: "equipments",
                newName: "health_1000");

            migrationBuilder.RenameColumn(
                name: "haste__1000",
                schema: "collection",
                table: "equipments",
                newName: "haste_1000");

            migrationBuilder.RenameColumn(
                name: "endurance_physical__1000",
                schema: "collection",
                table: "equipments",
                newName: "endurance_physical_1000");

            migrationBuilder.RenameColumn(
                name: "endurance_magical__1000",
                schema: "collection",
                table: "equipments",
                newName: "endurance_magical_1000");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier__1000",
                schema: "collection",
                table: "equipments",
                newName: "crit_multiplier_1000");

            migrationBuilder.RenameColumn(
                name: "crit_chance__1000",
                schema: "collection",
                table: "equipments",
                newName: "crit_chance_1000");

            migrationBuilder.RenameColumn(
                name: "agility__1000",
                schema: "collection",
                table: "equipments",
                newName: "agility_1000");

            migrationBuilder.RenameColumn(
                name: "versality__1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "versality_1000");

            migrationBuilder.RenameColumn(
                name: "strength__1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "strength_1000");

            migrationBuilder.RenameColumn(
                name: "intelligence__1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "intelligence_1000");

            migrationBuilder.RenameColumn(
                name: "initiative__1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "initiative_1000");

            migrationBuilder.RenameColumn(
                name: "health__1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "health_1000");

            migrationBuilder.RenameColumn(
                name: "haste__1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "haste_1000");

            migrationBuilder.RenameColumn(
                name: "endurance_physical__1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_physical_1000");

            migrationBuilder.RenameColumn(
                name: "endurance_magical__1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_magical_1000");

            migrationBuilder.RenameColumn(
                name: "damage__1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "damage_1000");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier__1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_multiplier_1000");

            migrationBuilder.RenameColumn(
                name: "crit_chance__1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_chance_1000");

            migrationBuilder.RenameColumn(
                name: "agility__1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "agility_1000");

            migrationBuilder.RenameColumn(
                name: "versality__1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "versality_1000");

            migrationBuilder.RenameColumn(
                name: "strength__1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "strength_1000");

            migrationBuilder.RenameColumn(
                name: "intelligence__1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "intelligence_1000");

            migrationBuilder.RenameColumn(
                name: "initiative__1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "initiative_1000");

            migrationBuilder.RenameColumn(
                name: "health__1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "health_1000");

            migrationBuilder.RenameColumn(
                name: "haste__1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "haste_1000");

            migrationBuilder.RenameColumn(
                name: "endurance_physical__1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "endurance_physical_1000");

            migrationBuilder.RenameColumn(
                name: "endurance_magical__1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "endurance_magical_1000");

            migrationBuilder.RenameColumn(
                name: "damage__1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "damage_1000");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier__1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "crit_multiplier_1000");

            migrationBuilder.RenameColumn(
                name: "crit_chance__1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "crit_chance_1000");

            migrationBuilder.RenameColumn(
                name: "agility__1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "agility_1000");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "versality_1000",
                schema: "collection",
                table: "heroes",
                newName: "versality__1000");

            migrationBuilder.RenameColumn(
                name: "strength_1000",
                schema: "collection",
                table: "heroes",
                newName: "strength__1000");

            migrationBuilder.RenameColumn(
                name: "intelligence_1000",
                schema: "collection",
                table: "heroes",
                newName: "intelligence__1000");

            migrationBuilder.RenameColumn(
                name: "initiative_1000",
                schema: "collection",
                table: "heroes",
                newName: "initiative__1000");

            migrationBuilder.RenameColumn(
                name: "health_1000",
                schema: "collection",
                table: "heroes",
                newName: "health__1000");

            migrationBuilder.RenameColumn(
                name: "haste_1000",
                schema: "collection",
                table: "heroes",
                newName: "haste__1000");

            migrationBuilder.RenameColumn(
                name: "endurance_physical_1000",
                schema: "collection",
                table: "heroes",
                newName: "endurance_physical__1000");

            migrationBuilder.RenameColumn(
                name: "endurance_magical_1000",
                schema: "collection",
                table: "heroes",
                newName: "endurance_magical__1000");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier_1000",
                schema: "collection",
                table: "heroes",
                newName: "crit_multiplier__1000");

            migrationBuilder.RenameColumn(
                name: "crit_chance_1000",
                schema: "collection",
                table: "heroes",
                newName: "crit_chance__1000");

            migrationBuilder.RenameColumn(
                name: "agility_1000",
                schema: "collection",
                table: "heroes",
                newName: "agility__1000");

            migrationBuilder.RenameColumn(
                name: "versality_1000",
                schema: "collection",
                table: "equipments",
                newName: "versality__1000");

            migrationBuilder.RenameColumn(
                name: "strength_1000",
                schema: "collection",
                table: "equipments",
                newName: "strength__1000");

            migrationBuilder.RenameColumn(
                name: "intelligence_1000",
                schema: "collection",
                table: "equipments",
                newName: "intelligence__1000");

            migrationBuilder.RenameColumn(
                name: "initiative_1000",
                schema: "collection",
                table: "equipments",
                newName: "initiative__1000");

            migrationBuilder.RenameColumn(
                name: "health_1000",
                schema: "collection",
                table: "equipments",
                newName: "health__1000");

            migrationBuilder.RenameColumn(
                name: "haste_1000",
                schema: "collection",
                table: "equipments",
                newName: "haste__1000");

            migrationBuilder.RenameColumn(
                name: "endurance_physical_1000",
                schema: "collection",
                table: "equipments",
                newName: "endurance_physical__1000");

            migrationBuilder.RenameColumn(
                name: "endurance_magical_1000",
                schema: "collection",
                table: "equipments",
                newName: "endurance_magical__1000");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier_1000",
                schema: "collection",
                table: "equipments",
                newName: "crit_multiplier__1000");

            migrationBuilder.RenameColumn(
                name: "crit_chance_1000",
                schema: "collection",
                table: "equipments",
                newName: "crit_chance__1000");

            migrationBuilder.RenameColumn(
                name: "agility_1000",
                schema: "collection",
                table: "equipments",
                newName: "agility__1000");

            migrationBuilder.RenameColumn(
                name: "versality_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "versality__1000");

            migrationBuilder.RenameColumn(
                name: "strength_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "strength__1000");

            migrationBuilder.RenameColumn(
                name: "intelligence_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "intelligence__1000");

            migrationBuilder.RenameColumn(
                name: "initiative_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "initiative__1000");

            migrationBuilder.RenameColumn(
                name: "health_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "health__1000");

            migrationBuilder.RenameColumn(
                name: "haste_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "haste__1000");

            migrationBuilder.RenameColumn(
                name: "endurance_physical_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_physical__1000");

            migrationBuilder.RenameColumn(
                name: "endurance_magical_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_magical__1000");

            migrationBuilder.RenameColumn(
                name: "damage_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "damage__1000");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_multiplier__1000");

            migrationBuilder.RenameColumn(
                name: "crit_chance_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_chance__1000");

            migrationBuilder.RenameColumn(
                name: "agility_1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "agility__1000");

            migrationBuilder.RenameColumn(
                name: "versality_1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "versality__1000");

            migrationBuilder.RenameColumn(
                name: "strength_1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "strength__1000");

            migrationBuilder.RenameColumn(
                name: "intelligence_1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "intelligence__1000");

            migrationBuilder.RenameColumn(
                name: "initiative_1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "initiative__1000");

            migrationBuilder.RenameColumn(
                name: "health_1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "health__1000");

            migrationBuilder.RenameColumn(
                name: "haste_1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "haste__1000");

            migrationBuilder.RenameColumn(
                name: "endurance_physical_1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "endurance_physical__1000");

            migrationBuilder.RenameColumn(
                name: "endurance_magical_1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "endurance_magical__1000");

            migrationBuilder.RenameColumn(
                name: "damage_1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "damage__1000");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier_1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "crit_multiplier__1000");

            migrationBuilder.RenameColumn(
                name: "crit_chance_1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "crit_chance__1000");

            migrationBuilder.RenameColumn(
                name: "agility_1000",
                schema: "game_data",
                table: "base_equipments",
                newName: "agility__1000");
        }
    }
}
