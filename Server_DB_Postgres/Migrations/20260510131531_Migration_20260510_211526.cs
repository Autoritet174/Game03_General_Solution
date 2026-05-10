using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260510_211526 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "enemy_count_max",
                schema: "game_data",
                table: "battlefields",
                newName: "max_hero_count");

            migrationBuilder.AddColumn<int>(
                name: "max_enemy_count",
                schema: "game_data",
                table: "battlefields",
                type: "integer",
                nullable: false,
                defaultValue: 12);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "max_enemy_count",
                schema: "game_data",
                table: "battlefields");

            migrationBuilder.RenameColumn(
                name: "max_hero_count",
                schema: "game_data",
                table: "battlefields",
                newName: "enemy_count_max");
        }
    }
}
