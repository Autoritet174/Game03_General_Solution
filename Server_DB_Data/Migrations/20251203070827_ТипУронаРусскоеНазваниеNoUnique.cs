using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class ТипУронаРусскоеНазваниеNoUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "type_damage_name_ru_idx",
                schema: "_main",
                table: "type_damage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "type_damage_name_ru_idx",
                schema: "_main",
                table: "type_damage",
                column: "name_ru",
                unique: true);
        }
    }
}
