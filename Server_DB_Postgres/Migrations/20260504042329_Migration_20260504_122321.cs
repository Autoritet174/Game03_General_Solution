using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260504_122321 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "difficulty",
                schema: "game_data",
                table: "battlefields");

            migrationBuilder.DropColumn(
                name: "level",
                schema: "game_data",
                table: "battlefields");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "difficulty",
                schema: "game_data",
                table: "battlefields",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "level",
                schema: "game_data",
                table: "battlefields",
                type: "integer",
                nullable: false,
                defaultValue: 1);
        }
    }
}
