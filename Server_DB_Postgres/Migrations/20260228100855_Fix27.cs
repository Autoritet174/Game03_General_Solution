using General.DTO;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix27 : Migration
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
                name: "crit_power",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "damage",
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
                name: "haste",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "intelligence",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "resist_damage_magical",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "resist_damage_physical",
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
                name: "damage",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.DropColumn(
                name: "health",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.AddColumn<long>(
                name: "agility1000",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "crit_chance1000",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "crit_power1000",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "endurance_magical1000",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "endurance_physical1000",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "haste1000",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "intelligence1000",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "resist_damage_magical1000",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "resist_damage_physical1000",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "strength1000",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "versality1000",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<Dice>(
                name: "damage",
                schema: "game_data",
                table: "equipment_types",
                type: "jsonb",
                nullable: true,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb",
                oldClrType: typeof(Dice),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AddColumn<Dice>(
                name: "agility1000",
                schema: "game_data",
                table: "base_heroes",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "crit_chance1000",
                schema: "game_data",
                table: "base_heroes",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "crit_power1000",
                schema: "game_data",
                table: "base_heroes",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "damage1000",
                schema: "game_data",
                table: "base_heroes",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "endurance_magical1000",
                schema: "game_data",
                table: "base_heroes",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "endurance_physical1000",
                schema: "game_data",
                table: "base_heroes",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "haste1000",
                schema: "game_data",
                table: "base_heroes",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "health1000",
                schema: "game_data",
                table: "base_heroes",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "intelligence1000",
                schema: "game_data",
                table: "base_heroes",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "strength1000",
                schema: "game_data",
                table: "base_heroes",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "versality1000",
                schema: "game_data",
                table: "base_heroes",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AlterColumn<Dice>(
                name: "health",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: true,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb",
                oldClrType: typeof(Dice),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<Dice>(
                name: "damage",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: true,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb",
                oldClrType: typeof(Dice),
                oldType: "jsonb",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "agility1000",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "crit_chance1000",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "crit_power1000",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "endurance_magical1000",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "endurance_physical1000",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "haste1000",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "intelligence1000",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "resist_damage_magical1000",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "resist_damage_physical1000",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "strength1000",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "versality1000",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "agility1000",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.DropColumn(
                name: "crit_chance1000",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.DropColumn(
                name: "crit_power1000",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.DropColumn(
                name: "damage1000",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.DropColumn(
                name: "endurance_magical1000",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.DropColumn(
                name: "endurance_physical1000",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.DropColumn(
                name: "haste1000",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.DropColumn(
                name: "health1000",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.DropColumn(
                name: "intelligence1000",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.DropColumn(
                name: "strength1000",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.DropColumn(
                name: "versality1000",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.AddColumn<int>(
                name: "agility",
                schema: "collection",
                table: "heroes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "crit_chance",
                schema: "collection",
                table: "heroes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "crit_power",
                schema: "collection",
                table: "heroes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Dice>(
                name: "damage",
                schema: "collection",
                table: "heroes",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "endurance_magical",
                schema: "collection",
                table: "heroes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "endurance_physical",
                schema: "collection",
                table: "heroes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "haste",
                schema: "collection",
                table: "heroes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "intelligence",
                schema: "collection",
                table: "heroes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "resist_damage_magical",
                schema: "collection",
                table: "heroes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "resist_damage_physical",
                schema: "collection",
                table: "heroes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "strength",
                schema: "collection",
                table: "heroes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "versality",
                schema: "collection",
                table: "heroes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Dice>(
                name: "damage",
                schema: "game_data",
                table: "equipment_types",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(Dice),
                oldType: "jsonb",
                oldNullable: true,
                oldDefaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AddColumn<Dice>(
                name: "damage",
                schema: "game_data",
                table: "base_heroes",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<Dice>(
                name: "health",
                schema: "game_data",
                table: "base_heroes",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AlterColumn<Dice>(
                name: "health",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(Dice),
                oldType: "jsonb",
                oldNullable: true,
                oldDefaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");

            migrationBuilder.AlterColumn<Dice>(
                name: "damage",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(Dice),
                oldType: "jsonb",
                oldNullable: true,
                oldDefaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");
        }
    }
}
