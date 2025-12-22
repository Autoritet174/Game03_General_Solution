using General.DTO;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "attack",
                schema: "collection",
                table: "heroes");

            migrationBuilder.RenameColumn(
                name: "versality",
                schema: "collection",
                table: "heroes",
                newName: "versality1000");

            migrationBuilder.RenameColumn(
                name: "health",
                schema: "collection",
                table: "heroes",
                newName: "health1000");

            migrationBuilder.RenameColumn(
                name: "haste",
                schema: "collection",
                table: "heroes",
                newName: "haste1000");

            migrationBuilder.RenameColumn(
                name: "crit_power",
                schema: "collection",
                table: "heroes",
                newName: "crit_power1000");

            migrationBuilder.RenameColumn(
                name: "crit_chance",
                schema: "collection",
                table: "heroes",
                newName: "crit_chance1000");

            migrationBuilder.AddColumn<Dice>(
                name: "damage",
                schema: "collection",
                table: "heroes",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "damage",
                schema: "collection",
                table: "heroes");

            migrationBuilder.RenameColumn(
                name: "versality1000",
                schema: "collection",
                table: "heroes",
                newName: "versality");

            migrationBuilder.RenameColumn(
                name: "health1000",
                schema: "collection",
                table: "heroes",
                newName: "health");

            migrationBuilder.RenameColumn(
                name: "haste1000",
                schema: "collection",
                table: "heroes",
                newName: "haste");

            migrationBuilder.RenameColumn(
                name: "crit_power1000",
                schema: "collection",
                table: "heroes",
                newName: "crit_power");

            migrationBuilder.RenameColumn(
                name: "crit_chance1000",
                schema: "collection",
                table: "heroes",
                newName: "crit_chance");

            migrationBuilder.AddColumn<long>(
                name: "attack",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
