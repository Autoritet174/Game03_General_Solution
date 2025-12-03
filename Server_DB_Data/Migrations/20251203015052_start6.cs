using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class start6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "heroes");

            migrationBuilder.RenameTable(
                name: "heroes",
                schema: "_main",
                newName: "heroes",
                newSchema: "heroes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "heroes",
                schema: "heroes",
                newName: "heroes",
                newSchema: "_main");
        }
    }
}
