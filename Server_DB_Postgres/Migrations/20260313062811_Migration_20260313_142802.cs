using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260313_142802 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "crit_power1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_multiplier1000");

            migrationBuilder.AddColumn<long>(
                name: "initiative1000",
                schema: "collection",
                table: "heroes",
                type: "jsonb",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "initiative1000",
                schema: "collection",
                table: "heroes");

            migrationBuilder.RenameColumn(
                name: "crit_multiplier1000",
                schema: "game_data",
                table: "base_heroes",
                newName: "crit_power1000");
        }
    }
}
