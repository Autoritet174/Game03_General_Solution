using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "collection");

            migrationBuilder.AddColumn<Guid>(
                name: "user_device_id",
                schema: "logs",
                table: "user_authorizations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "equipments",
                schema: "collection",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    base_equipment_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("equipments__pkey", x => x.id);
                    table.ForeignKey(
                        name: "equipments__base_equipment_id__base_equipments__fkey",
                        column: x => x.base_equipment_id,
                        principalSchema: "game_data",
                        principalTable: "base_equipments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "equipments__user_id__users__fkey",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_devices",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    system_environment_user_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    time_zone_minutes = table.Column<int>(type: "integer", nullable: false),
                    device_unique_identifier = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    device_model = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    device_type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    operating_system = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    processor_type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    processor_count = table.Column<int>(type: "integer", nullable: false),
                    system_memory_size = table.Column<int>(type: "integer", nullable: false),
                    graphics_device_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    graphics_memory_size = table.Column<int>(type: "integer", nullable: false),
                    system_info_supports_instancing = table.Column<bool>(type: "boolean", nullable: false),
                    system_info_npot_support = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_devices__pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "heroes",
                schema: "collection",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    base_hero_id = table.Column<int>(type: "integer", nullable: false),
                    health = table.Column<int>(type: "integer", nullable: false),
                    attack = table.Column<int>(type: "integer", nullable: false),
                    strength = table.Column<int>(type: "integer", nullable: false),
                    agility = table.Column<int>(type: "integer", nullable: false),
                    intelligence = table.Column<int>(type: "integer", nullable: false),
                    crit_chance = table.Column<int>(type: "integer", nullable: false),
                    crit_power = table.Column<int>(type: "integer", nullable: false),
                    haste = table.Column<int>(type: "integer", nullable: false),
                    versality = table.Column<int>(type: "integer", nullable: false),
                    endurance_physical = table.Column<int>(type: "integer", nullable: false),
                    endurance_magical = table.Column<int>(type: "integer", nullable: false),
                    resist_damage_physical = table.Column<int>(type: "integer", nullable: false),
                    resist_damage_magical = table.Column<int>(type: "integer", nullable: false),
                    equipment1id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment2id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment3id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment4id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment5id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment6id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment7id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment8id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment9id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment10id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment11id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment12id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment13id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment14id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment15id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment16id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment17id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment18id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment19id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment20id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment21id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment22id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment23id = table.Column<Guid>(type: "uuid", nullable: true),
                    equipment24id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("heroes__pkey", x => x.id);
                    table.ForeignKey(
                        name: "heroes__base_hero_id__base_heroes__fkey",
                        column: x => x.base_hero_id,
                        principalSchema: "game_data",
                        principalTable: "base_heroes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "heroes__equipment10id__equipments__fkey",
                        column: x => x.equipment10id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment11id__equipments__fkey",
                        column: x => x.equipment11id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment12id__equipments__fkey",
                        column: x => x.equipment12id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment13id__equipments__fkey",
                        column: x => x.equipment13id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment14id__equipments__fkey",
                        column: x => x.equipment14id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment15id__equipments__fkey",
                        column: x => x.equipment15id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment16id__equipments__fkey",
                        column: x => x.equipment16id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment17id__equipments__fkey",
                        column: x => x.equipment17id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment18id__equipments__fkey",
                        column: x => x.equipment18id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment19id__equipments__fkey",
                        column: x => x.equipment19id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment1id__equipments__fkey",
                        column: x => x.equipment1id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment20id__equipments__fkey",
                        column: x => x.equipment20id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment21id__equipments__fkey",
                        column: x => x.equipment21id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment22id__equipments__fkey",
                        column: x => x.equipment22id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment23id__equipments__fkey",
                        column: x => x.equipment23id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment24id__equipments__fkey",
                        column: x => x.equipment24id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment2id__equipments__fkey",
                        column: x => x.equipment2id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment3id__equipments__fkey",
                        column: x => x.equipment3id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment4id__equipments__fkey",
                        column: x => x.equipment4id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment5id__equipments__fkey",
                        column: x => x.equipment5id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment6id__equipments__fkey",
                        column: x => x.equipment6id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment7id__equipments__fkey",
                        column: x => x.equipment7id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment8id__equipments__fkey",
                        column: x => x.equipment8id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__equipment9id__equipments__fkey",
                        column: x => x.equipment9id,
                        principalSchema: "collection",
                        principalTable: "equipments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "heroes__user_id__users__fkey",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "user_authorizations__user_device_id__idx",
                schema: "logs",
                table: "user_authorizations",
                column: "user_device_id");

            migrationBuilder.CreateIndex(
                name: "equipments__base_equipment_id__idx",
                schema: "collection",
                table: "equipments",
                column: "base_equipment_id");

            migrationBuilder.CreateIndex(
                name: "equipments__user_id__idx",
                schema: "collection",
                table: "equipments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "heroes__base_hero_id__idx",
                schema: "collection",
                table: "heroes",
                column: "base_hero_id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment10id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment10id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment11id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment11id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment12id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment12id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment13id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment13id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment14id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment14id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment15id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment15id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment16id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment16id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment17id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment17id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment18id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment18id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment19id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment19id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment1id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment1id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment20id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment20id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment21id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment21id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment22id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment22id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment23id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment23id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment24id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment24id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment2id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment2id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment3id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment3id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment4id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment4id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment5id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment5id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment6id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment6id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment7id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment7id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment8id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment8id");

            migrationBuilder.CreateIndex(
                name: "heroes__equipment9id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment9id");

            migrationBuilder.CreateIndex(
                name: "heroes__user_id__idx",
                schema: "collection",
                table: "heroes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "user_devices__device_unique_identifier__idx",
                schema: "users",
                table: "user_devices",
                column: "device_unique_identifier",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "user_authorizations__user_device_id__user_devices__fkey",
                schema: "logs",
                table: "user_authorizations",
                column: "user_device_id",
                principalSchema: "users",
                principalTable: "user_devices",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "user_authorizations__user_device_id__user_devices__fkey",
                schema: "logs",
                table: "user_authorizations");

            migrationBuilder.DropTable(
                name: "heroes",
                schema: "collection");

            migrationBuilder.DropTable(
                name: "user_devices",
                schema: "users");

            migrationBuilder.DropTable(
                name: "equipments",
                schema: "collection");

            migrationBuilder.DropIndex(
                name: "user_authorizations__user_device_id__idx",
                schema: "logs",
                table: "user_authorizations");

            migrationBuilder.DropColumn(
                name: "user_device_id",
                schema: "logs",
                table: "user_authorizations");
        }
    }
}
