using General.DTO;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix29 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "versality1000",
                schema: "game_data",
                table: "base_heroes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Dice>(
                name: "versality1000",
                schema: "game_data",
                table: "base_heroes",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");
        }
    }
}
