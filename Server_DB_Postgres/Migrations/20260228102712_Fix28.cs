using General.DTO;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix28 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "intelligence1000",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.DropColumn(
                name: "strength1000",
                schema: "game_data",
                table: "base_heroes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
