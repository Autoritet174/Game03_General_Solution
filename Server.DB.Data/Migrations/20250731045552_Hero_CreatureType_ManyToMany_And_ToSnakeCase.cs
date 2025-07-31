using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.DB.Data.Migrations
{
    /// <inheritdoc />
    public partial class Hero_CreatureType_ManyToMany_And_ToSnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_users_id",
                table: "heroes");

            migrationBuilder.RenameIndex(
                name: "name",
                table: "heroes",
                newName: "idx_heroes_name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_heroes",
                table: "heroes",
                column: "id");

            migrationBuilder.CreateTable(
                name: "creature_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_creature_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hero_creature_type",
                columns: table => new
                {
                    creature_types_id = table.Column<Guid>(type: "uuid", nullable: false),
                    heroes_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hero_creature_type", x => new { x.creature_types_id, x.heroes_id });
                    table.ForeignKey(
                        name: "fk_hero_creature_type_creature_types_id_creature_types",
                        column: x => x.creature_types_id,
                        principalTable: "creature_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_hero_creature_type_heroes_id_heroes",
                        column: x => x.heroes_id,
                        principalTable: "heroes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_creature_types_name",
                table: "creature_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_hero_creature_type_heroes_id",
                table: "hero_creature_type",
                column: "heroes_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hero_creature_type");

            migrationBuilder.DropTable(
                name: "creature_types");

            migrationBuilder.DropPrimaryKey(
                name: "pk_heroes",
                table: "heroes");

            migrationBuilder.RenameIndex(
                name: "idx_heroes_name",
                table: "heroes",
                newName: "name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users_id",
                table: "heroes",
                column: "id");
        }
    }
}
