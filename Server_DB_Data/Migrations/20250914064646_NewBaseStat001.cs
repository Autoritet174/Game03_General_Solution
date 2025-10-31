using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class NewBaseStat001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "rarity",
                schema: "main",
                table: "heroes",
                newName: "_rarity");

            migrationBuilder.AddColumn<float>(
                name: "base_health",
                schema: "main",
                table: "heroes",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "base_health",
                schema: "main",
                table: "heroes");

            migrationBuilder.RenameColumn(
                name: "_rarity",
                schema: "main",
                table: "heroes",
                newName: "rarity");
        }
    }
}
