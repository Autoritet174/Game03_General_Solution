using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class МечТипУрона : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "_type_damage_id",
                schema: "equipment",
                table: "sword",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "type_damage",
                schema: "_main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("type_damage_pkey", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "sword__type_damage_id_idx",
                schema: "equipment",
                table: "sword",
                column: "_type_damage_id");

            migrationBuilder.CreateIndex(
                name: "type_damage_name_idx",
                schema: "_main",
                table: "type_damage",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "sword__type_damage_id_type_damage_fkey",
                schema: "equipment",
                table: "sword",
                column: "_type_damage_id",
                principalSchema: "_main",
                principalTable: "type_damage",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "sword__type_damage_id_type_damage_fkey",
                schema: "equipment",
                table: "sword");

            migrationBuilder.DropTable(
                name: "type_damage",
                schema: "_main");

            migrationBuilder.DropIndex(
                name: "sword__type_damage_id_idx",
                schema: "equipment",
                table: "sword");

            migrationBuilder.DropColumn(
                name: "_type_damage_id",
                schema: "equipment",
                table: "sword");
        }
    }
}
