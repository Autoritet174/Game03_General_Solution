using General.DTO;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "damage",
                schema: "game_data",
                table: "base_heroes");

            migrationBuilder.DropColumn(
                name: "health",
                schema: "game_data",
                table: "base_heroes");
        }
    }
}
