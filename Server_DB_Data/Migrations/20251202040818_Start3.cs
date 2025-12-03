using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class Start3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "base_attack",
                schema: "_main",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "base_health",
                schema: "_main",
                table: "heroes");

            migrationBuilder.AddColumn<string>(
                name: "attack",
                schema: "_main",
                table: "heroes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "health",
                schema: "_main",
                table: "heroes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "attack",
                schema: "_main",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "health",
                schema: "_main",
                table: "heroes");

            migrationBuilder.AddColumn<float>(
                name: "base_attack",
                schema: "_main",
                table: "heroes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "base_health",
                schema: "_main",
                table: "heroes",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
