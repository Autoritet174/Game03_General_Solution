using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "versality1000",
                schema: "collection",
                table: "heroes",
                newName: "versality");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "versality",
                schema: "collection",
                table: "heroes",
                newName: "versality1000");

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
        }
    }
}
