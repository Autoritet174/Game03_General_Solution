using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class CREATE_TABLE_hero_x_creature_type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "relations");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "main",
                table: "heroes",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "main",
                table: "creature_types",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "hero_x_creature_type",
                schema: "relations",
                columns: table => new
                {
                    hero_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creature_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hero_x_creature_type", x => new { x.hero_id, x.creature_type_id });
                    table.ForeignKey(
                        name: "fk_hero_x_creature_type_creature_type_id_creature_types",
                        column: x => x.creature_type_id,
                        principalSchema: "main",
                        principalTable: "creature_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_hero_x_creature_type_hero_id_heroes",
                        column: x => x.hero_id,
                        principalSchema: "main",
                        principalTable: "heroes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_heroes_name",
                schema: "main",
                table: "heroes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_creature_types_name",
                schema: "main",
                table: "creature_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_hero_x_creature_type_creature_type_id",
                schema: "relations",
                table: "hero_x_creature_type",
                column: "creature_type_id");

            migrationBuilder.CreateIndex(
                name: "idx_hero_x_creature_type_hero_id",
                schema: "relations",
                table: "hero_x_creature_type",
                column: "hero_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hero_x_creature_type",
                schema: "relations");

            migrationBuilder.DropIndex(
                name: "idx_heroes_name",
                schema: "main",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "idx_creature_types_name",
                schema: "main",
                table: "creature_types");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "main",
                table: "heroes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "main",
                table: "creature_types",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);
        }
    }
}
