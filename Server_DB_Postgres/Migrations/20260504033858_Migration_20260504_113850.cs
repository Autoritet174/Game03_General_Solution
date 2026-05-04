using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260504_113850 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "guarant_spawn",
                schema: "game_data",
                table: "x_battlefield_npcs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "probability_spawn",
                schema: "game_data",
                table: "x_battlefield_npcs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "guarant_spawn",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.DropColumn(
                name: "probability_spawn",
                schema: "game_data",
                table: "x_battlefield_npcs");
        }
    }
}
