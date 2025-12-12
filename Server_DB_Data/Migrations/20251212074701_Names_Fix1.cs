using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class Names_Fix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "EquipmentType_slot_type_id_slot_type_fkey",
                schema: "__lists",
                table: "EquipmentType");

            migrationBuilder.DropForeignKey(
                name: "MaterialDamagePercent_damage_type_id_damage_types_fkey",
                schema: "__lists",
                table: "MaterialDamagePercent");

            migrationBuilder.DropForeignKey(
                name: "MaterialDamagePercent_smithing_materials_id_smithing_materials_fkey",
                schema: "__lists",
                table: "MaterialDamagePercent");

            migrationBuilder.DropForeignKey(
                name: "Weapons_weapon_type_id_equipment_type_fkey",
                schema: "_equipment",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "X_WeaponType_DamageType_damage_type_id_damage_types_fkey",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType");

            migrationBuilder.DropForeignKey(
                name: "X_WeaponType_DamageType_weapon_type_id_weapon_types_fkey",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType");

            migrationBuilder.DropForeignKey(
                name: "X_Hero_CreatureType_creature_type_id_creature_types_fkey",
                schema: "x_Cross",
                table: "X_Hero_CreatureType");

            migrationBuilder.DropForeignKey(
                name: "X_Hero_CreatureType_hero_id_heroes_fkey",
                schema: "x_Cross",
                table: "X_Hero_CreatureType");

            migrationBuilder.DropPrimaryKey(
                name: "X_Hero_CreatureType_pkey",
                schema: "x_Cross",
                table: "X_Hero_CreatureType");

            migrationBuilder.DropPrimaryKey(
                name: "X_WeaponType_DamageType_pkey",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType");

            migrationBuilder.DropPrimaryKey(
                name: "Weapons_pkey",
                schema: "_equipment",
                table: "Weapons");

            migrationBuilder.DropPrimaryKey(
                name: "SmithingMaterials_pkey",
                schema: "__lists",
                table: "SmithingMaterials");

            migrationBuilder.DropPrimaryKey(
                name: "Heroes_pkey",
                schema: "_heroes",
                table: "Heroes");

            migrationBuilder.DropPrimaryKey(
                name: "DamageTypes_pkey",
                schema: "__lists",
                table: "DamageTypes");

            migrationBuilder.DropPrimaryKey(
                name: "CreatureTypes_pkey",
                schema: "__lists",
                table: "CreatureTypes");

            migrationBuilder.DropPrimaryKey(
                name: "SlotType_pkey",
                schema: "__lists",
                table: "SlotType");

            migrationBuilder.DropPrimaryKey(
                name: "MaterialDamagePercent_pkey",
                schema: "__lists",
                table: "MaterialDamagePercent");

            migrationBuilder.DropPrimaryKey(
                name: "WeaponTypes_pkey",
                schema: "__lists",
                table: "EquipmentType");

            migrationBuilder.RenameTable(
                name: "SlotType",
                schema: "__lists",
                newName: "SlotTypes",
                newSchema: "__lists");

            migrationBuilder.RenameTable(
                name: "MaterialDamagePercent",
                schema: "__lists",
                newName: "MaterialDamagePercents",
                newSchema: "__lists");

            migrationBuilder.RenameTable(
                name: "EquipmentType",
                schema: "__lists",
                newName: "EquipmentTypes",
                newSchema: "__lists");

            migrationBuilder.RenameIndex(
                name: "X_Hero_CreatureType_HeroId_idx",
                schema: "x_Cross",
                table: "X_Hero_CreatureType",
                newName: "X_Hero_CreatureType__HeroId__idx");

            migrationBuilder.RenameIndex(
                name: "X_Hero_CreatureType_CreatureTypeId_idx",
                schema: "x_Cross",
                table: "X_Hero_CreatureType",
                newName: "X_Hero_CreatureType__CreatureTypeId__idx");

            migrationBuilder.RenameIndex(
                name: "X_EquipmentType_DamageType_EquipmentTypeId_idx",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType",
                newName: "X_EquipmentType_DamageType__EquipmentTypeId__idx");

            migrationBuilder.RenameIndex(
                name: "X_EquipmentType_DamageType_DamageTypeId_idx",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType",
                newName: "X_EquipmentType_DamageType__DamageTypeId__idx");

            migrationBuilder.RenameIndex(
                name: "Weapons_weapon_type_id_idx",
                schema: "_equipment",
                table: "Weapons",
                newName: "Weapons__WeaponTypeId__idx");

            migrationBuilder.RenameIndex(
                name: "Weapons_name_idx",
                schema: "_equipment",
                table: "Weapons",
                newName: "Weapons__Name__idx");

            migrationBuilder.RenameIndex(
                name: "SmithingMaterials_name_idx",
                schema: "__lists",
                table: "SmithingMaterials",
                newName: "SmithingMaterials__Name__idx");

            migrationBuilder.RenameIndex(
                name: "Heroes_name_idx",
                schema: "_heroes",
                table: "Heroes",
                newName: "Heroes__Name__idx");

            migrationBuilder.RenameIndex(
                name: "DamageTypes_name_idx",
                schema: "__lists",
                table: "DamageTypes",
                newName: "DamageTypes__Name__idx");

            migrationBuilder.RenameIndex(
                name: "CreatureTypes_name_idx",
                schema: "__lists",
                table: "CreatureTypes",
                newName: "CreatureTypes__Name__idx");

            migrationBuilder.RenameIndex(
                name: "SlotType_name_idx",
                schema: "__lists",
                table: "SlotTypes",
                newName: "SlotTypes__Name__idx");

            migrationBuilder.RenameIndex(
                name: "MaterialDamagePercent_smithing_materials_id_idx",
                schema: "__lists",
                table: "MaterialDamagePercents",
                newName: "MaterialDamagePercents__SmithingMaterialsId__idx");

            migrationBuilder.RenameIndex(
                name: "MaterialDamagePercent_damage_type_id_idx",
                schema: "__lists",
                table: "MaterialDamagePercents",
                newName: "MaterialDamagePercents__DamageTypeId__idx");

            migrationBuilder.RenameIndex(
                name: "EquipmentType_slot_type_id_idx",
                schema: "__lists",
                table: "EquipmentTypes",
                newName: "EquipmentTypes__SlotTypeId__idx");

            migrationBuilder.RenameIndex(
                name: "EquipmentType_name_idx",
                schema: "__lists",
                table: "EquipmentTypes",
                newName: "EquipmentTypes__Name__idx");

            migrationBuilder.AddPrimaryKey(
                name: "X_Hero_CreatureType__pkey",
                schema: "x_Cross",
                table: "X_Hero_CreatureType",
                columns: new[] { "HeroId", "CreatureTypeId" });

            migrationBuilder.AddPrimaryKey(
                name: "X_EquipmentType_DamageType__pkey",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType",
                columns: new[] { "EquipmentTypeId", "DamageTypeId" });

            migrationBuilder.AddPrimaryKey(
                name: "Weapons__pkey",
                schema: "_equipment",
                table: "Weapons",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "SmithingMaterials__pkey",
                schema: "__lists",
                table: "SmithingMaterials",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "Heroes__pkey",
                schema: "_heroes",
                table: "Heroes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "DamageTypes__pkey",
                schema: "__lists",
                table: "DamageTypes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "CreatureTypes__pkey",
                schema: "__lists",
                table: "CreatureTypes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "SlotTypes__pkey",
                schema: "__lists",
                table: "SlotTypes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "MaterialDamagePercents__pkey",
                schema: "__lists",
                table: "MaterialDamagePercents",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "EquipmentTypes__pkey",
                schema: "__lists",
                table: "EquipmentTypes",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "EquipmentTypes__SlotTypeId__SlotTypes__fkey",
                schema: "__lists",
                table: "EquipmentTypes",
                column: "slot_type_id",
                principalSchema: "__lists",
                principalTable: "SlotTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "MaterialDamagePercents__DamageTypeId__DamageTypes__fkey",
                schema: "__lists",
                table: "MaterialDamagePercents",
                column: "damage_type_id",
                principalSchema: "__lists",
                principalTable: "DamageTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "MaterialDamagePercents__SmithingMaterialsId__SmithingMaterials__fkey",
                schema: "__lists",
                table: "MaterialDamagePercents",
                column: "smithing_materials_id",
                principalSchema: "__lists",
                principalTable: "SmithingMaterials",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Weapons__WeaponTypeId__EquipmentTypes__fkey",
                schema: "_equipment",
                table: "Weapons",
                column: "weapon_type_id",
                principalSchema: "__lists",
                principalTable: "EquipmentTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "X_EquipmentType_DamageType__DamageTypeId__DamageTypes__fkey",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType",
                column: "DamageTypeId",
                principalSchema: "__lists",
                principalTable: "DamageTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "X_EquipmentType_DamageType__EquipmentTypeId__EquipmentTypes__fkey",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType",
                column: "EquipmentTypeId",
                principalSchema: "__lists",
                principalTable: "EquipmentTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "X_Hero_CreatureType__CreatureTypeId__CreatureTypes__fkey",
                schema: "x_Cross",
                table: "X_Hero_CreatureType",
                column: "CreatureTypeId",
                principalSchema: "__lists",
                principalTable: "CreatureTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "X_Hero_CreatureType__HeroId__Heroes__fkey",
                schema: "x_Cross",
                table: "X_Hero_CreatureType",
                column: "HeroId",
                principalSchema: "_heroes",
                principalTable: "Heroes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "EquipmentTypes__SlotTypeId__SlotTypes__fkey",
                schema: "__lists",
                table: "EquipmentTypes");

            migrationBuilder.DropForeignKey(
                name: "MaterialDamagePercents__DamageTypeId__DamageTypes__fkey",
                schema: "__lists",
                table: "MaterialDamagePercents");

            migrationBuilder.DropForeignKey(
                name: "MaterialDamagePercents__SmithingMaterialsId__SmithingMaterials__fkey",
                schema: "__lists",
                table: "MaterialDamagePercents");

            migrationBuilder.DropForeignKey(
                name: "Weapons__WeaponTypeId__EquipmentTypes__fkey",
                schema: "_equipment",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "X_EquipmentType_DamageType__DamageTypeId__DamageTypes__fkey",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType");

            migrationBuilder.DropForeignKey(
                name: "X_EquipmentType_DamageType__EquipmentTypeId__EquipmentTypes__fkey",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType");

            migrationBuilder.DropForeignKey(
                name: "X_Hero_CreatureType__CreatureTypeId__CreatureTypes__fkey",
                schema: "x_Cross",
                table: "X_Hero_CreatureType");

            migrationBuilder.DropForeignKey(
                name: "X_Hero_CreatureType__HeroId__Heroes__fkey",
                schema: "x_Cross",
                table: "X_Hero_CreatureType");

            migrationBuilder.DropPrimaryKey(
                name: "X_Hero_CreatureType__pkey",
                schema: "x_Cross",
                table: "X_Hero_CreatureType");

            migrationBuilder.DropPrimaryKey(
                name: "X_EquipmentType_DamageType__pkey",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType");

            migrationBuilder.DropPrimaryKey(
                name: "Weapons__pkey",
                schema: "_equipment",
                table: "Weapons");

            migrationBuilder.DropPrimaryKey(
                name: "SmithingMaterials__pkey",
                schema: "__lists",
                table: "SmithingMaterials");

            migrationBuilder.DropPrimaryKey(
                name: "Heroes__pkey",
                schema: "_heroes",
                table: "Heroes");

            migrationBuilder.DropPrimaryKey(
                name: "DamageTypes__pkey",
                schema: "__lists",
                table: "DamageTypes");

            migrationBuilder.DropPrimaryKey(
                name: "CreatureTypes__pkey",
                schema: "__lists",
                table: "CreatureTypes");

            migrationBuilder.DropPrimaryKey(
                name: "SlotTypes__pkey",
                schema: "__lists",
                table: "SlotTypes");

            migrationBuilder.DropPrimaryKey(
                name: "MaterialDamagePercents__pkey",
                schema: "__lists",
                table: "MaterialDamagePercents");

            migrationBuilder.DropPrimaryKey(
                name: "EquipmentTypes__pkey",
                schema: "__lists",
                table: "EquipmentTypes");

            migrationBuilder.RenameTable(
                name: "SlotTypes",
                schema: "__lists",
                newName: "SlotType",
                newSchema: "__lists");

            migrationBuilder.RenameTable(
                name: "MaterialDamagePercents",
                schema: "__lists",
                newName: "MaterialDamagePercent",
                newSchema: "__lists");

            migrationBuilder.RenameTable(
                name: "EquipmentTypes",
                schema: "__lists",
                newName: "EquipmentType",
                newSchema: "__lists");

            migrationBuilder.RenameIndex(
                name: "X_Hero_CreatureType__HeroId__idx",
                schema: "x_Cross",
                table: "X_Hero_CreatureType",
                newName: "X_Hero_CreatureType_HeroId_idx");

            migrationBuilder.RenameIndex(
                name: "X_Hero_CreatureType__CreatureTypeId__idx",
                schema: "x_Cross",
                table: "X_Hero_CreatureType",
                newName: "X_Hero_CreatureType_CreatureTypeId_idx");

            migrationBuilder.RenameIndex(
                name: "X_EquipmentType_DamageType__EquipmentTypeId__idx",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType",
                newName: "X_EquipmentType_DamageType_EquipmentTypeId_idx");

            migrationBuilder.RenameIndex(
                name: "X_EquipmentType_DamageType__DamageTypeId__idx",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType",
                newName: "X_EquipmentType_DamageType_DamageTypeId_idx");

            migrationBuilder.RenameIndex(
                name: "Weapons__WeaponTypeId__idx",
                schema: "_equipment",
                table: "Weapons",
                newName: "Weapons_weapon_type_id_idx");

            migrationBuilder.RenameIndex(
                name: "Weapons__Name__idx",
                schema: "_equipment",
                table: "Weapons",
                newName: "Weapons_name_idx");

            migrationBuilder.RenameIndex(
                name: "SmithingMaterials__Name__idx",
                schema: "__lists",
                table: "SmithingMaterials",
                newName: "SmithingMaterials_name_idx");

            migrationBuilder.RenameIndex(
                name: "Heroes__Name__idx",
                schema: "_heroes",
                table: "Heroes",
                newName: "Heroes_name_idx");

            migrationBuilder.RenameIndex(
                name: "DamageTypes__Name__idx",
                schema: "__lists",
                table: "DamageTypes",
                newName: "DamageTypes_name_idx");

            migrationBuilder.RenameIndex(
                name: "CreatureTypes__Name__idx",
                schema: "__lists",
                table: "CreatureTypes",
                newName: "CreatureTypes_name_idx");

            migrationBuilder.RenameIndex(
                name: "SlotTypes__Name__idx",
                schema: "__lists",
                table: "SlotType",
                newName: "SlotType_name_idx");

            migrationBuilder.RenameIndex(
                name: "MaterialDamagePercents__SmithingMaterialsId__idx",
                schema: "__lists",
                table: "MaterialDamagePercent",
                newName: "MaterialDamagePercent_smithing_materials_id_idx");

            migrationBuilder.RenameIndex(
                name: "MaterialDamagePercents__DamageTypeId__idx",
                schema: "__lists",
                table: "MaterialDamagePercent",
                newName: "MaterialDamagePercent_damage_type_id_idx");

            migrationBuilder.RenameIndex(
                name: "EquipmentTypes__SlotTypeId__idx",
                schema: "__lists",
                table: "EquipmentType",
                newName: "EquipmentType_slot_type_id_idx");

            migrationBuilder.RenameIndex(
                name: "EquipmentTypes__Name__idx",
                schema: "__lists",
                table: "EquipmentType",
                newName: "EquipmentType_name_idx");

            migrationBuilder.AddPrimaryKey(
                name: "X_Hero_CreatureType_pkey",
                schema: "x_Cross",
                table: "X_Hero_CreatureType",
                columns: new[] { "HeroId", "CreatureTypeId" });

            migrationBuilder.AddPrimaryKey(
                name: "X_EquipmentType_DamageType_pkey",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType",
                columns: new[] { "EquipmentTypeId", "DamageTypeId" });

            migrationBuilder.AddPrimaryKey(
                name: "Weapons_pkey",
                schema: "_equipment",
                table: "Weapons",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "SmithingMaterials_pkey",
                schema: "__lists",
                table: "SmithingMaterials",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "Heroes_pkey",
                schema: "_heroes",
                table: "Heroes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "DamageTypes_pkey",
                schema: "__lists",
                table: "DamageTypes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "CreatureTypes_pkey",
                schema: "__lists",
                table: "CreatureTypes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "SlotType_pkey",
                schema: "__lists",
                table: "SlotType",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "MaterialDamagePercent_pkey",
                schema: "__lists",
                table: "MaterialDamagePercent",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "EquipmentType_pkey",
                schema: "__lists",
                table: "EquipmentType",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "EquipmentType_slot_type_id_slot_type_fkey",
                schema: "__lists",
                table: "EquipmentType",
                column: "slot_type_id",
                principalSchema: "__lists",
                principalTable: "SlotType",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "MaterialDamagePercent_damage_type_id_damage_types_fkey",
                schema: "__lists",
                table: "MaterialDamagePercent",
                column: "damage_type_id",
                principalSchema: "__lists",
                principalTable: "DamageTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "MaterialDamagePercent_smithing_materials_id_smithing_materials_fkey",
                schema: "__lists",
                table: "MaterialDamagePercent",
                column: "smithing_materials_id",
                principalSchema: "__lists",
                principalTable: "SmithingMaterials",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Weapons_weapon_type_id_equipment_type_fkey",
                schema: "_equipment",
                table: "Weapons",
                column: "weapon_type_id",
                principalSchema: "__lists",
                principalTable: "EquipmentType",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "X_EquipmentType_DamageType_damage_type_id_damage_types_fkey",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType",
                column: "DamageTypeId",
                principalSchema: "__lists",
                principalTable: "DamageTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "X_EquipmentType_DamageType_equipment_type_id_equipment_type_fkey",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType",
                column: "EquipmentTypeId",
                principalSchema: "__lists",
                principalTable: "EquipmentType",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "X_Hero_CreatureType_creature_type_id_creature_types_fkey",
                schema: "x_Cross",
                table: "X_Hero_CreatureType",
                column: "CreatureTypeId",
                principalSchema: "__lists",
                principalTable: "CreatureTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "X_Hero_CreatureType_hero_id_heroes_fkey",
                schema: "x_Cross",
                table: "X_Hero_CreatureType",
                column: "HeroId",
                principalSchema: "_heroes",
                principalTable: "Heroes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
