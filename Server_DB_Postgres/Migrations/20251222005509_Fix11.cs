using General.DTO;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "damage",
                schema: "game_data",
                table: "equipment_types");

            migrationBuilder.DropColumn(
                name: "health",
                schema: "game_data",
                table: "equipment_types");

            migrationBuilder.DropColumn(
                name: "damage",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.DropColumn(
                name: "health",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.AddColumn<Dice>(
                name: "damage",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<Dice>(
                name: "health",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "damage",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "health",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.AddColumn<Dice>(
                name: "damage",
                schema: "game_data",
                table: "equipment_types",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<Dice>(
                name: "health",
                schema: "game_data",
                table: "equipment_types",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "damage",
                schema: "game_data",
                table: "base_heroes",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "health",
                schema: "game_data",
                table: "base_heroes",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
