using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "game_data");

            migrationBuilder.EnsureSchema(
                name: "logs");

            migrationBuilder.EnsureSchema(
                name: "server");

            migrationBuilder.EnsureSchema(
                name: "users");

            migrationBuilder.CreateTable(
                name: "base_heroes",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    rarity = table.Column<int>(type: "integer", nullable: false),
                    is_unique = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    health = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    damage = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    main_stat = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("base_heroes__pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "creature_types",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("creature_types__pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "damage_types",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    name_ru = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    dev_hint_ru = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("damage_types__pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "slot_types",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    name_ru = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("slot_types__pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "smithing_materials",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    name_ru = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("smithing_materials__pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_ban_reasons",
                schema: "server",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_ban_reasons__pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    email_verified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    time_zone = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_admin = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    version = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users__pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "x_hero_creature_type",
                schema: "game_data",
                columns: table => new
                {
                    hero_id = table.Column<int>(type: "integer", nullable: false),
                    creature_type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("x_hero_creature_type__pkey", x => new { x.hero_id, x.creature_type_id });
                    table.ForeignKey(
                        name: "x_hero_creature_type__creature_type_id__creature_types__fkey",
                        column: x => x.creature_type_id,
                        principalSchema: "game_data",
                        principalTable: "creature_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "x_hero_creature_type__hero_id__base_heroes__fkey",
                        column: x => x.hero_id,
                        principalSchema: "game_data",
                        principalTable: "base_heroes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "equipment_types",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    name_ru = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    mass_physical = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    mass_magical = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    slot_type_id = table.Column<int>(type: "integer", nullable: false),
                    can_craft_smithing = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    can_craft_jewelcrafting = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    attack = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    spend_action_points = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("equipment_types__pkey", x => x.id);
                    table.ForeignKey(
                        name: "equipment_types__slot_type_id__slot_types__fkey",
                        column: x => x.slot_type_id,
                        principalSchema: "game_data",
                        principalTable: "slot_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "material_damage_percents",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    smithing_materials_id = table.Column<int>(type: "integer", nullable: false),
                    damage_type_id = table.Column<int>(type: "integer", nullable: false),
                    percent = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("material_damage_percents__pkey", x => x.id);
                    table.ForeignKey(
                        name: "material_damage_percents__damage_type_id__damage_types__fkey",
                        column: x => x.damage_type_id,
                        principalSchema: "game_data",
                        principalTable: "damage_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "material_damage_percents__smithing_materials_id__smithing_materials__fkey",
                        column: x => x.smithing_materials_id,
                        principalSchema: "game_data",
                        principalTable: "smithing_materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_authorizations",
                schema: "logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    success = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_authorizations__pkey", x => x.id);
                    table.ForeignKey(
                        name: "user_authorizations__user_id__users__fkey",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_bans",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_ban_reason_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_bans__pkey", x => x.id);
                    table.ForeignKey(
                        name: "user_bans__user_ban_reason_id__user_ban_reasons__fkey",
                        column: x => x.user_ban_reason_id,
                        principalSchema: "server",
                        principalTable: "user_ban_reasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "user_bans__user_id__users__fkey",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "base_equipments",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    rarity = table.Column<int>(type: "integer", nullable: false),
                    is_unique = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    damage = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    equipment_type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("base_equipments__pkey", x => x.id);
                    table.ForeignKey(
                        name: "base_equipments__equipment_type_id__equipment_types__fkey",
                        column: x => x.equipment_type_id,
                        principalSchema: "game_data",
                        principalTable: "equipment_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "x_equipment_type_damage_type",
                schema: "game_data",
                columns: table => new
                {
                    equipment_type_id = table.Column<int>(type: "integer", nullable: false),
                    damage_type_id = table.Column<int>(type: "integer", nullable: false),
                    damage_coef = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("x_equipment_type_damage_type__pkey", x => new { x.equipment_type_id, x.damage_type_id });
                    table.ForeignKey(
                        name: "x_equipment_type_damage_type__damage_type_id__damage_types__fkey",
                        column: x => x.damage_type_id,
                        principalSchema: "game_data",
                        principalTable: "damage_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "x_equipment_type_damage_type__equipment_type_id__equipment_types__fkey",
                        column: x => x.equipment_type_id,
                        principalSchema: "game_data",
                        principalTable: "equipment_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "base_equipments__equipment_type_id__idx",
                schema: "game_data",
                table: "base_equipments",
                column: "equipment_type_id");

            migrationBuilder.CreateIndex(
                name: "base_equipments__name__idx",
                schema: "game_data",
                table: "base_equipments",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "base_heroes__name__idx",
                schema: "game_data",
                table: "base_heroes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "creature_types__name__idx",
                schema: "game_data",
                table: "creature_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "damage_types__name__idx",
                schema: "game_data",
                table: "damage_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "equipment_types__name__idx",
                schema: "game_data",
                table: "equipment_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "equipment_types__slot_type_id__idx",
                schema: "game_data",
                table: "equipment_types",
                column: "slot_type_id");

            migrationBuilder.CreateIndex(
                name: "material_damage_percents__damage_type_id__idx",
                schema: "game_data",
                table: "material_damage_percents",
                column: "damage_type_id");

            migrationBuilder.CreateIndex(
                name: "material_damage_percents__smithing_materials_id__idx",
                schema: "game_data",
                table: "material_damage_percents",
                column: "smithing_materials_id");

            migrationBuilder.CreateIndex(
                name: "slot_types__name__idx",
                schema: "game_data",
                table: "slot_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "smithing_materials__name__idx",
                schema: "game_data",
                table: "smithing_materials",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "user_authorizations__user_id__idx",
                schema: "logs",
                table: "user_authorizations",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "user_ban_reasons__name__idx",
                schema: "server",
                table: "user_ban_reasons",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "user_bans__user_ban_reason_id__idx",
                schema: "users",
                table: "user_bans",
                column: "user_ban_reason_id");

            migrationBuilder.CreateIndex(
                name: "user_bans__user_id__idx",
                schema: "users",
                table: "user_bans",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "users__email__idx",
                schema: "users",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "x_equipment_type_damage_type__damage_type_id__idx",
                schema: "game_data",
                table: "x_equipment_type_damage_type",
                column: "damage_type_id");

            migrationBuilder.CreateIndex(
                name: "x_hero_creature_type__creature_type_id__idx",
                schema: "game_data",
                table: "x_hero_creature_type",
                column: "creature_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "base_equipments",
                schema: "game_data");

            migrationBuilder.DropTable(
                name: "material_damage_percents",
                schema: "game_data");

            migrationBuilder.DropTable(
                name: "user_authorizations",
                schema: "logs");

            migrationBuilder.DropTable(
                name: "user_bans",
                schema: "users");

            migrationBuilder.DropTable(
                name: "x_equipment_type_damage_type",
                schema: "game_data");

            migrationBuilder.DropTable(
                name: "x_hero_creature_type",
                schema: "game_data");

            migrationBuilder.DropTable(
                name: "smithing_materials",
                schema: "game_data");

            migrationBuilder.DropTable(
                name: "user_ban_reasons",
                schema: "server");

            migrationBuilder.DropTable(
                name: "users",
                schema: "users");

            migrationBuilder.DropTable(
                name: "damage_types",
                schema: "game_data");

            migrationBuilder.DropTable(
                name: "equipment_types",
                schema: "game_data");

            migrationBuilder.DropTable(
                name: "creature_types",
                schema: "game_data");

            migrationBuilder.DropTable(
                name: "base_heroes",
                schema: "game_data");

            migrationBuilder.DropTable(
                name: "slot_types",
                schema: "game_data");
        }
    }
}
