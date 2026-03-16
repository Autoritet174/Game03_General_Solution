using System.Collections.Generic;
using General.DTO;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class BigPatch1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "agility",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "crit_chance",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "crit_multiplier",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "endurance_magical",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "endurance_physical",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "experience_now",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "haste",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "health",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "initiative",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "intelligence",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "strength",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "versality",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "agility",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "crit_chance",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "crit_multiplier",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "endurance_magical",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "endurance_physical",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "haste",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "health",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "initiative",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "intelligence",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "strength",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "versality",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "agility",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "crit_chance",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "crit_multiplier",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "damage",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "endurance_magical",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "endurance_physical",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "haste",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "health",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "initiative",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "intelligence",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "strength",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "versality",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.RenameColumn(
                name: "versality",
                schema: "game_data",
                table: "base_heroes",
                newName: "versality");

            migrationBuilder.RenameColumn(
                name: "strength",
                schema: "game_data",
                table: "base_heroes",
                newName: "strength");

            migrationBuilder.RenameColumn(
                name: "intelligence",
                schema: "game_data",
                table: "base_heroes",
                newName: "intelligence");

            migrationBuilder.RenameColumn(
                name: "initiative",
                schema: "game_data",
                table: "base_heroes",
                newName: "initiative");

            migrationBuilder.RenameColumn(
                name: "health",
                schema: "game_data",
                table: "base_heroes",
                newName: "health");

            migrationBuilder.RenameColumn(
                name: "haste",
                schema: "game_data",
                table: "base_heroes",
                newName: "haste");

            migrationBuilder.RenameColumn(
                name: "endurance_physical",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_physical");

            migrationBuilder.RenameColumn(
                name: "endurance_magical",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_magical");

            migrationBuilder.RenameColumn(
                name: "damage",
                schema: "game_data",
                table: "base_heroes",
                newName: "damage");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_multiplier");

            migrationBuilder.RenameColumn(
                name: "crit_chance",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_chance");

            migrationBuilder.RenameColumn(
                name: "agility",
                schema: "game_data",
                table: "base_heroes",
                newName: "agility");

            migrationBuilder.AddColumn<float>(
                name: "agility",
                schema: "collection",
                table: "heroes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "crit_chance",
                schema: "collection",
                table: "heroes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "crit_multiplier",
                schema: "collection",
                table: "heroes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "endurance_magical",
                schema: "collection",
                table: "heroes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "endurance_physical",
                schema: "collection",
                table: "heroes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "experience",
                schema: "collection",
                table: "heroes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "haste",
                schema: "collection",
                table: "heroes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "health",
                schema: "collection",
                table: "heroes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "initiative",
                schema: "collection",
                table: "heroes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "intelligence",
                schema: "collection",
                table: "heroes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "strength",
                schema: "collection",
                table: "heroes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "versality",
                schema: "collection",
                table: "heroes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<Dictionary<int, float>>(
                name: "stats",
                schema: "collection",
                table: "equipments",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "agility",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "crit_chance",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "crit_multiplier",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "endurance_magical",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "endurance_physical",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "experience",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "haste",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "health",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "initiative",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "intelligence",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "strength",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "versality",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "stats",
                schema: "collection",
                table: "equipments");

            migrationBuilder.RenameColumn(
                name: "versality",
                schema: "game_data",
                table: "base_heroes",
                newName: "versality");

            migrationBuilder.RenameColumn(
                name: "strength",
                schema: "game_data",
                table: "base_heroes",
                newName: "strength");

            migrationBuilder.RenameColumn(
                name: "intelligence",
                schema: "game_data",
                table: "base_heroes",
                newName: "intelligence");

            migrationBuilder.RenameColumn(
                name: "initiative",
                schema: "game_data",
                table: "base_heroes",
                newName: "initiative");

            migrationBuilder.RenameColumn(
                name: "health",
                schema: "game_data",
                table: "base_heroes",
                newName: "health");

            migrationBuilder.RenameColumn(
                name: "haste",
                schema: "game_data",
                table: "base_heroes",
                newName: "haste");

            migrationBuilder.RenameColumn(
                name: "endurance_physical",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_physical");

            migrationBuilder.RenameColumn(
                name: "endurance_magical",
                schema: "game_data",
                table: "base_heroes",
                newName: "endurance_magical");

            migrationBuilder.RenameColumn(
                name: "damage",
                schema: "game_data",
                table: "base_heroes",
                newName: "damage");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_multiplier");

            migrationBuilder.RenameColumn(
                name: "crit_chance",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_chance");

            migrationBuilder.RenameColumn(
                name: "agility",
                schema: "game_data",
                table: "base_heroes",
                newName: "agility");

            migrationBuilder.AddColumn<long>(
                name: "agility",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "crit_chance",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "crit_multiplier",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "endurance_magical",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "endurance_physical",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "experience_now",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "haste",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "health",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "initiative",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "intelligence",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "strength",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "versality",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "agility",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "crit_chance",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "crit_multiplier",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "endurance_magical",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "endurance_physical",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "haste",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "health",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "initiative",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "intelligence",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "strength",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "versality",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<Dice>(
                name: "agility",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "crit_chance",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "crit_multiplier",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "damage",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "endurance_magical",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "endurance_physical",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "haste",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "health",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "initiative",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "intelligence",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "strength",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "versality",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");
        }
    }
}
