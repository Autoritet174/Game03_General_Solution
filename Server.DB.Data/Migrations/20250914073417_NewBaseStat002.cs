using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.DB.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewBaseStat002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "base_attack",
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
                name: "base_attack",
                schema: "main",
                table: "heroes");
        }
    }
}
