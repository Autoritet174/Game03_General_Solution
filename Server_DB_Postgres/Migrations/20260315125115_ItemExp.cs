using System;
using System.Collections.Generic;
using General.DTO;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class ItemExp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "initiative",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "damage",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "health",
                schema: "game_data",
                table: "base_equipments");

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
                name: "health",
                schema: "collection",
                table: "heroes",
                newName: "initiative_");

            migrationBuilder.RenameColumn(
                name: "haste",
                schema: "collection",
                table: "heroes",
                newName: "health_");

            migrationBuilder.RenameColumn(
                name: "endurance_physical",
                schema: "collection",
                table: "heroes",
                newName: "haste_");

            migrationBuilder.RenameColumn(
                name: "endurance_magical",
                schema: "collection",
                table: "heroes",
                newName: "endurance_physical_");

            migrationBuilder.RenameColumn(
                name: "crit_power",
                schema: "collection",
                table: "heroes",
                newName: "endurance_magical_");

            migrationBuilder.RenameColumn(
                name: "crit_chance",
                schema: "collection",
                table: "heroes",
                newName: "crit_multiplier_");

            migrationBuilder.RenameColumn(
                name: "agility",
                schema: "collection",
                table: "heroes",
                newName: "crit_chance_");

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

            migrationBuilder.AddColumn<long>(
                name: "agility_",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "agility_",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "crit_chance_",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "crit_multiplier_",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "endurance_magical_",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "endurance_physical_",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<Dictionary<Guid, ItemExp>>(
                name: "experience_heroes",
                schema: "collection",
                table: "equipments",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddColumn<long>(
                name: "haste_",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "health_",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "initiative_",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "intelligence_",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "strength_",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "versality_",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<Dice>(
                name: "agility_",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "crit_chance_",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "crit_multiplier_",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "damage_",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "endurance_magical_",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "endurance_physical_",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "haste_",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "health_",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "initiative_",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "intelligence_",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "strength_",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "versality_",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "agility_",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "agility_",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "crit_chance_",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "crit_multiplier_",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "endurance_magical_",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "endurance_physical_",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "experience_heroes",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "haste_",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "health_",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "initiative_",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "intelligence_",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "strength_",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "versality_",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "agility_",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "crit_chance_",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "crit_multiplier_",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "damage_",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "endurance_magical_",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "endurance_physical_",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "haste_",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "health_",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "initiative_",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "intelligence_",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "strength_",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "versality_",
                schema: "game_data",
                table: "base_equipments");

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
                newName: "health");

            migrationBuilder.RenameColumn(
                name: "health_",
                schema: "collection",
                table: "heroes",
                newName: "haste");

            migrationBuilder.RenameColumn(
                name: "haste_",
                schema: "collection",
                table: "heroes",
                newName: "endurance_physical");

            migrationBuilder.RenameColumn(
                name: "endurance_physical_",
                schema: "collection",
                table: "heroes",
                newName: "endurance_magical");

            migrationBuilder.RenameColumn(
                name: "endurance_magical_",
                schema: "collection",
                table: "heroes",
                newName: "crit_power");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier_",
                schema: "collection",
                table: "heroes",
                newName: "crit_chance");

            migrationBuilder.RenameColumn(
                name: "crit_chance_",
                schema: "collection",
                table: "heroes",
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

            migrationBuilder.AddColumn<long>(
                name: "initiative",
                schema: "collection",
                table: "heroes",
                type: "jsonb",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Dice>(
                name: "damage",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: true,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "health",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: true,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");
        }
    }
}
