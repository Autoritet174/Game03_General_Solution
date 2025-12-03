using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class ТипУронаРусскоеНазвание : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name_ru",
                schema: "_main",
                table: "type_damage",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "type_damage_name_ru_idx",
                schema: "_main",
                table: "type_damage",
                column: "name_ru",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "type_damage_name_ru_idx",
                schema: "_main",
                table: "type_damage");

            migrationBuilder.DropColumn(
                name: "name_ru",
                schema: "_main",
                table: "type_damage");
        }
    }
}
