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
                name: "initiative_1000",
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
                name: "health_1000",
                schema: "collection",
                table: "heroes",
                newName: "initiative__1000");

            migrationBuilder.RenameColumn(
                name: "haste_1000",
                schema: "collection",
                table: "heroes",
                newName: "health__1000");

            migrationBuilder.RenameColumn(
                name: "endurance_physical_1000",
                schema: "collection",
                table: "heroes",
                newName: "haste__1000");

            migrationBuilder.RenameColumn(
                name: "endurance_magical_1000",
                schema: "collection",
                table: "heroes",
                newName: "endurance_physical__1000");

            migrationBuilder.RenameColumn(
                name: "crit_power_1000",
                schema: "collection",
                table: "heroes",
                newName: "endurance_magical__1000");

            migrationBuilder.RenameColumn(
                name: "crit_chance_1000",
                schema: "collection",
                table: "heroes",
                newName: "crit_multiplier__1000");

            migrationBuilder.RenameColumn(
                name: "agility_1000",
                schema: "collection",
                table: "heroes",
                newName: "crit_chance__1000");

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

            migrationBuilder.AddColumn<long>(
                name: "agility__1000",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "agility__1000",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "crit_chance__1000",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "crit_multiplier__1000",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "endurance_magical__1000",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "endurance_physical__1000",
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
                name: "haste__1000",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "health__1000",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "initiative__1000",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "intelligence__1000",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "strength__1000",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "versality__1000",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<Dice>(
                name: "agility__1000",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "crit_chance__1000",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "crit_multiplier__1000",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "damage__1000",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "endurance_magical__1000",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "endurance_physical__1000",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "haste__1000",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "health__1000",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "initiative__1000",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "intelligence__1000",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "strength__1000",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "versality__1000",
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
                name: "agility__1000",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "agility__1000",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "crit_chance__1000",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "crit_multiplier__1000",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "endurance_magical__1000",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "endurance_physical__1000",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "experience_heroes",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "haste__1000",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "health__1000",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "initiative__1000",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "intelligence__1000",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "strength__1000",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "versality__1000",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "agility__1000",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "crit_chance__1000",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "crit_multiplier__1000",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "damage__1000",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "endurance_magical__1000",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "endurance_physical__1000",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "haste__1000",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "health__1000",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "initiative__1000",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "intelligence__1000",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "strength__1000",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "versality__1000",
                schema: "game_data",
                table: "base_equipments");

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
                newName: "health_1000");

            migrationBuilder.RenameColumn(
                name: "health__1000",
                schema: "collection",
                table: "heroes",
                newName: "haste_1000");

            migrationBuilder.RenameColumn(
                name: "haste__1000",
                schema: "collection",
                table: "heroes",
                newName: "endurance_physical_1000");

            migrationBuilder.RenameColumn(
                name: "endurance_physical__1000",
                schema: "collection",
                table: "heroes",
                newName: "endurance_magical_1000");

            migrationBuilder.RenameColumn(
                name: "endurance_magical__1000",
                schema: "collection",
                table: "heroes",
                newName: "crit_power_1000");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier__1000",
                schema: "collection",
                table: "heroes",
                newName: "crit_chance_1000");

            migrationBuilder.RenameColumn(
                name: "crit_chance__1000",
                schema: "collection",
                table: "heroes",
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

            migrationBuilder.AddColumn<long>(
                name: "initiative_1000",
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
