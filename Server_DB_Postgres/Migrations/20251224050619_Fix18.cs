using General.DTO;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "attack_old",
            //    schema: "game_data",
            //    table: "equipment_types",
            //    type: "text",
            //    nullable: false,
            //    defaultValue: "");

            migrationBuilder.AddColumn<Dice>(
                name: "damage",
                schema: "game_data",
                table: "equipment_types",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "attack_old",
            //    schema: "game_data",
            //    table: "equipment_types");

            migrationBuilder.DropColumn(
                name: "damage",
                schema: "game_data",
                table: "equipment_types");
        }
    }
}
