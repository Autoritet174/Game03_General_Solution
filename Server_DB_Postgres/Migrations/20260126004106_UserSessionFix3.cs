using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class UserSessionFix3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "x_equipment_type_damage_type__damage_type_id__damage_types__fkey",
                schema: "game_data",
                table: "x_equipment_type_damage_type");

            migrationBuilder.DropForeignKey(
                name: "x_equipment_type_damage_type__equipment_type_id__equipment_types__fkey",
                schema: "game_data",
                table: "x_equipment_type_damage_type");

            migrationBuilder.DropForeignKey(
                name: "x_hero_creature_type__base_hero_id__base_heroes__fkey",
                schema: "game_data",
                table: "x_hero_creature_type");

            migrationBuilder.DropForeignKey(
                name: "x_hero_creature_type__creature_type_id__creature_types__fkey",
                schema: "game_data",
                table: "x_hero_creature_type");

            migrationBuilder.DropTable(
                name: "user_passkeys",
                schema: "users");

            migrationBuilder.DropPrimaryKey(
                name: "x_hero_creature_type__pkey",
                schema: "game_data",
                table: "x_hero_creature_type");

            migrationBuilder.DropPrimaryKey(
                name: "x_equipment_type_damage_type__pkey",
                schema: "game_data",
                table: "x_equipment_type_damage_type");

            migrationBuilder.RenameTable(
                name: "x_hero_creature_type",
                schema: "game_data",
                newName: "x_heroes_creature_types",
                newSchema: "game_data");

            migrationBuilder.RenameTable(
                name: "x_equipment_type_damage_type",
                schema: "game_data",
                newName: "x_equipment_types_damage_types",
                newSchema: "game_data");

            migrationBuilder.RenameIndex(
                name: "x_hero_creature_type__creature_type_id__idx",
                schema: "game_data",
                table: "x_heroes_creature_types",
                newName: "x_heroes_creature_types__creature_type_id__idx");

            migrationBuilder.RenameIndex(
                name: "x_equipment_type_damage_type__damage_type_id__idx",
                schema: "game_data",
                table: "x_equipment_types_damage_types",
                newName: "x_equipment_types_damage_types__damage_type_id__idx");

            migrationBuilder.AddPrimaryKey(
                name: "x_heroes_creature_types__pkey",
                schema: "game_data",
                table: "x_heroes_creature_types",
                columns: new[] { "base_hero_id", "creature_type_id" });

            migrationBuilder.AddPrimaryKey(
                name: "x_equipment_types_damage_types__pkey",
                schema: "game_data",
                table: "x_equipment_types_damage_types",
                columns: new[] { "equipment_type_id", "damage_type_id" });

            migrationBuilder.CreateTable(
                name: "user_accesskeys",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    descriptor_id = table.Column<byte[]>(type: "bytea", nullable: false),
                    public_key = table.Column<byte[]>(type: "bytea", nullable: false),
                    signature_counter = table.Column<long>(type: "bigint", nullable: false),
                    device_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_accesskeys__pkey", x => x.id);
                    table.ForeignKey(
                        name: "user_accesskeys__user_id__identity_users__fkey",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "user_accesskeys__user_id__idx",
                schema: "users",
                table: "user_accesskeys",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "x_equipment_types_damage_types__damage_type_id__damage_types__fkey",
                schema: "game_data",
                table: "x_equipment_types_damage_types",
                column: "damage_type_id",
                principalSchema: "game_data",
                principalTable: "damage_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "x_equipment_types_damage_types__equipment_type_id__equipment_types__fkey",
                schema: "game_data",
                table: "x_equipment_types_damage_types",
                column: "equipment_type_id",
                principalSchema: "game_data",
                principalTable: "equipment_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "x_heroes_creature_types__base_hero_id__base_heroes__fkey",
                schema: "game_data",
                table: "x_heroes_creature_types",
                column: "base_hero_id",
                principalSchema: "game_data",
                principalTable: "base_heroes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "x_heroes_creature_types__creature_type_id__creature_types__fkey",
                schema: "game_data",
                table: "x_heroes_creature_types",
                column: "creature_type_id",
                principalSchema: "game_data",
                principalTable: "creature_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "x_equipment_types_damage_types__damage_type_id__damage_types__fkey",
                schema: "game_data",
                table: "x_equipment_types_damage_types");

            migrationBuilder.DropForeignKey(
                name: "x_equipment_types_damage_types__equipment_type_id__equipment_types__fkey",
                schema: "game_data",
                table: "x_equipment_types_damage_types");

            migrationBuilder.DropForeignKey(
                name: "x_heroes_creature_types__base_hero_id__base_heroes__fkey",
                schema: "game_data",
                table: "x_heroes_creature_types");

            migrationBuilder.DropForeignKey(
                name: "x_heroes_creature_types__creature_type_id__creature_types__fkey",
                schema: "game_data",
                table: "x_heroes_creature_types");

            migrationBuilder.DropTable(
                name: "user_accesskeys",
                schema: "users");

            migrationBuilder.DropPrimaryKey(
                name: "x_heroes_creature_types__pkey",
                schema: "game_data",
                table: "x_heroes_creature_types");

            migrationBuilder.DropPrimaryKey(
                name: "x_equipment_types_damage_types__pkey",
                schema: "game_data",
                table: "x_equipment_types_damage_types");

            migrationBuilder.RenameTable(
                name: "x_heroes_creature_types",
                schema: "game_data",
                newName: "x_hero_creature_type",
                newSchema: "game_data");

            migrationBuilder.RenameTable(
                name: "x_equipment_types_damage_types",
                schema: "game_data",
                newName: "x_equipment_type_damage_type",
                newSchema: "game_data");

            migrationBuilder.RenameIndex(
                name: "x_heroes_creature_types__creature_type_id__idx",
                schema: "game_data",
                table: "x_hero_creature_type",
                newName: "x_hero_creature_type__creature_type_id__idx");

            migrationBuilder.RenameIndex(
                name: "x_equipment_types_damage_types__damage_type_id__idx",
                schema: "game_data",
                table: "x_equipment_type_damage_type",
                newName: "x_equipment_type_damage_type__damage_type_id__idx");

            migrationBuilder.AddPrimaryKey(
                name: "x_hero_creature_type__pkey",
                schema: "game_data",
                table: "x_hero_creature_type",
                columns: new[] { "base_hero_id", "creature_type_id" });

            migrationBuilder.AddPrimaryKey(
                name: "x_equipment_type_damage_type__pkey",
                schema: "game_data",
                table: "x_equipment_type_damage_type",
                columns: new[] { "equipment_type_id", "damage_type_id" });

            migrationBuilder.CreateTable(
                name: "user_passkeys",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    descriptor_id = table.Column<byte[]>(type: "bytea", nullable: false),
                    device_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    public_key = table.Column<byte[]>(type: "bytea", nullable: false),
                    signature_counter = table.Column<long>(type: "bigint", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_passkeys__pkey", x => x.id);
                    table.ForeignKey(
                        name: "user_passkeys__user_id__identity_users__fkey",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "user_passkeys__user_id__idx",
                schema: "users",
                table: "user_passkeys",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "x_equipment_type_damage_type__damage_type_id__damage_types__fkey",
                schema: "game_data",
                table: "x_equipment_type_damage_type",
                column: "damage_type_id",
                principalSchema: "game_data",
                principalTable: "damage_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "x_equipment_type_damage_type__equipment_type_id__equipment_types__fkey",
                schema: "game_data",
                table: "x_equipment_type_damage_type",
                column: "equipment_type_id",
                principalSchema: "game_data",
                principalTable: "equipment_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "x_hero_creature_type__base_hero_id__base_heroes__fkey",
                schema: "game_data",
                table: "x_hero_creature_type",
                column: "base_hero_id",
                principalSchema: "game_data",
                principalTable: "base_heroes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "x_hero_creature_type__creature_type_id__creature_types__fkey",
                schema: "game_data",
                table: "x_hero_creature_type",
                column: "creature_type_id",
                principalSchema: "game_data",
                principalTable: "creature_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
