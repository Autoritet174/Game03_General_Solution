using System.Collections.Generic;
using General;
using General.DTO;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class EquipmentPossibleStatAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Dictionary<EStatType, Dice>>(
                name: "possible_stats",
                schema: "game_data",
                table: "base_equipments",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "possible_stats",
                schema: "game_data",
                table: "base_equipments");
        }
    }
}
