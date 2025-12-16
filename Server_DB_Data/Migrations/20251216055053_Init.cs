using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "__lists");

            migrationBuilder.EnsureSchema(
                name: "_heroes");

            migrationBuilder.EnsureSchema(
                name: "_equipment");

            migrationBuilder.EnsureSchema(
                name: "x_Cross");

            migrationBuilder.CreateTable(
                name: "CreatureTypes",
                schema: "__lists",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CreatureTypes__pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DamageTypes",
                schema: "__lists",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    name_ru = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DevHintRu = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("DamageTypes__pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Heroes",
                schema: "_heroes",
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
                    table.PrimaryKey("Heroes__pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SlotTypes",
                schema: "__lists",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    name_ru = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SlotTypes__pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SmithingMaterials",
                schema: "__lists",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    name_ru = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SmithingMaterials__pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "X_Hero_CreatureType",
                schema: "x_Cross",
                columns: table => new
                {
                    HeroId = table.Column<int>(type: "integer", nullable: false),
                    CreatureTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("X_Hero_CreatureType__pkey", x => new { x.HeroId, x.CreatureTypeId });
                    table.ForeignKey(
                        name: "X_Hero_CreatureType__CreatureTypeId__CreatureTypes__fkey",
                        column: x => x.CreatureTypeId,
                        principalSchema: "__lists",
                        principalTable: "CreatureTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "X_Hero_CreatureType__HeroId__Heroes__fkey",
                        column: x => x.HeroId,
                        principalSchema: "_heroes",
                        principalTable: "Heroes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentTypes",
                schema: "__lists",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    name_ru = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    mass = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    slot_type_id = table.Column<int>(type: "integer", nullable: false),
                    can_craft_smithing = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    can_craft_jewelcrafting = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    attack = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    spend_action_points = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("EquipmentTypes__pkey", x => x.id);
                    table.ForeignKey(
                        name: "EquipmentTypes__SlotTypeId__SlotTypes__fkey",
                        column: x => x.slot_type_id,
                        principalSchema: "__lists",
                        principalTable: "SlotTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialDamagePercents",
                schema: "__lists",
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
                    table.PrimaryKey("MaterialDamagePercents__pkey", x => x.id);
                    table.ForeignKey(
                        name: "MaterialDamagePercents__DamageTypeId__DamageTypes__fkey",
                        column: x => x.damage_type_id,
                        principalSchema: "__lists",
                        principalTable: "DamageTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "MaterialDamagePercents__SmithingMaterialsId__SmithingMaterials__fkey",
                        column: x => x.smithing_materials_id,
                        principalSchema: "__lists",
                        principalTable: "SmithingMaterials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                schema: "_equipment",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    rarity = table.Column<int>(type: "integer", nullable: false),
                    is_unique = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    damage = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    weapon_type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Weapons__pkey", x => x.id);
                    table.ForeignKey(
                        name: "Weapons__WeaponTypeId__EquipmentTypes__fkey",
                        column: x => x.weapon_type_id,
                        principalSchema: "__lists",
                        principalTable: "EquipmentTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "X_EquipmentType_DamageType",
                schema: "x_Cross",
                columns: table => new
                {
                    EquipmentTypeId = table.Column<int>(type: "integer", nullable: false),
                    DamageTypeId = table.Column<int>(type: "integer", nullable: false),
                    DamageCoef = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("X_EquipmentType_DamageType__pkey", x => new { x.EquipmentTypeId, x.DamageTypeId });
                    table.ForeignKey(
                        name: "X_EquipmentType_DamageType__DamageTypeId__DamageTypes__fkey",
                        column: x => x.DamageTypeId,
                        principalSchema: "__lists",
                        principalTable: "DamageTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "X_EquipmentType_DamageType__EquipmentTypeId__EquipmentTypes__fkey",
                        column: x => x.EquipmentTypeId,
                        principalSchema: "__lists",
                        principalTable: "EquipmentTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "CreatureTypes__Name__idx",
                schema: "__lists",
                table: "CreatureTypes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "DamageTypes__Name__idx",
                schema: "__lists",
                table: "DamageTypes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EquipmentTypes__Name__idx",
                schema: "__lists",
                table: "EquipmentTypes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EquipmentTypes__SlotTypeId__idx",
                schema: "__lists",
                table: "EquipmentTypes",
                column: "slot_type_id");

            migrationBuilder.CreateIndex(
                name: "Heroes__Name__idx",
                schema: "_heroes",
                table: "Heroes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "MaterialDamagePercents__DamageTypeId__idx",
                schema: "__lists",
                table: "MaterialDamagePercents",
                column: "damage_type_id");

            migrationBuilder.CreateIndex(
                name: "MaterialDamagePercents__SmithingMaterialsId__idx",
                schema: "__lists",
                table: "MaterialDamagePercents",
                column: "smithing_materials_id");

            migrationBuilder.CreateIndex(
                name: "SlotTypes__Name__idx",
                schema: "__lists",
                table: "SlotTypes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "SmithingMaterials__Name__idx",
                schema: "__lists",
                table: "SmithingMaterials",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Weapons__Name__idx",
                schema: "_equipment",
                table: "Weapons",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Weapons__WeaponTypeId__idx",
                schema: "_equipment",
                table: "Weapons",
                column: "weapon_type_id");

            migrationBuilder.CreateIndex(
                name: "X_EquipmentType_DamageType__DamageTypeId__idx",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType",
                column: "DamageTypeId");

            migrationBuilder.CreateIndex(
                name: "X_Hero_CreatureType__CreatureTypeId__idx",
                schema: "x_Cross",
                table: "X_Hero_CreatureType",
                column: "CreatureTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialDamagePercents",
                schema: "__lists");

            migrationBuilder.DropTable(
                name: "Weapons",
                schema: "_equipment");

            migrationBuilder.DropTable(
                name: "X_EquipmentType_DamageType",
                schema: "x_Cross");

            migrationBuilder.DropTable(
                name: "X_Hero_CreatureType",
                schema: "x_Cross");

            migrationBuilder.DropTable(
                name: "SmithingMaterials",
                schema: "__lists");

            migrationBuilder.DropTable(
                name: "DamageTypes",
                schema: "__lists");

            migrationBuilder.DropTable(
                name: "EquipmentTypes",
                schema: "__lists");

            migrationBuilder.DropTable(
                name: "CreatureTypes",
                schema: "__lists");

            migrationBuilder.DropTable(
                name: "Heroes",
                schema: "_heroes");

            migrationBuilder.DropTable(
                name: "SlotTypes",
                schema: "__lists");
        }
    }
}
