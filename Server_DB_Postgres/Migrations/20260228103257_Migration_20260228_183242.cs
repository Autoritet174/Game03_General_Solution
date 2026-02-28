using General.DTO;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260228_183242 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Dice>(
                name: "strength1000",
                schema: "game_data",
                table: "base_heroes",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{\"c\":0,\"s\":0}'::jsonb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "strength1000",
                schema: "game_data",
                table: "base_heroes");
        }
    }
}
