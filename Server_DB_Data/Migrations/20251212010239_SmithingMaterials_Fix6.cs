using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class SmithingMaterials_Fix6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Временно удаляем FK
            migrationBuilder.DropForeignKey(
                name: "Weapons_weapon_type_id_weapon_types_fkey",
                schema: "_equipment",
                table: "Weapons");

            migrationBuilder.Sql(@"
            -- Основная таблица WeaponTypes -> EquipmentType
            ALTER TABLE ""__lists"".""WeaponTypes"" RENAME TO ""EquipmentType"";
            
            -- Sequence для автоинкремента (если существует)
            DO $$ 
            BEGIN
                IF EXISTS (
                    SELECT 1 FROM pg_class 
                    WHERE relname = 'weapontypes_id_seq' 
                    AND relnamespace = (SELECT oid FROM pg_namespace WHERE nspname = '__lists')
                ) THEN
                    ALTER SEQUENCE ""__lists"".""weapontypes_id_seq"" RENAME TO ""equipmenttype_id_seq"";
                END IF;
            END $$;
            
            -- Кросс-таблица
            ALTER TABLE ""x_Cross"".""X_WeaponType_DamageType"" RENAME TO ""X_EquipmentType_DamageType"";
            
            -- Столбцы в кросс-таблице
            ALTER TABLE ""x_Cross"".""X_EquipmentType_DamageType"" 
            RENAME COLUMN ""WeaponTypeId"" TO ""EquipmentTypeId"";
            
            -- Индексы основной таблицы
            ALTER INDEX IF EXISTS ""__lists"".""WeaponTypes_name_idx"" 
            RENAME TO ""EquipmentType_name_idx"";
            
            -- Индексы кросс-таблицы
            ALTER INDEX IF EXISTS ""x_Cross"".""X_WeaponType_DamageType_WeaponTypeId_idx"" 
            RENAME TO ""X_EquipmentType_DamageType_EquipmentTypeId_idx"";
            
            ALTER INDEX IF EXISTS ""x_Cross"".""X_WeaponType_DamageType_DamageTypeId_idx"" 
            RENAME TO ""X_EquipmentType_DamageType_DamageTypeId_idx"";
        ");

            // Восстанавливаем FK с новым именем
            migrationBuilder.AddForeignKey(
                name: "Weapons_weapon_type_id_equipment_type_fkey",
                schema: "_equipment",
                table: "Weapons",
                column: "weapon_type_id",
                principalSchema: "__lists",
                principalTable: "EquipmentType",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Удаляем новый FK
            migrationBuilder.DropForeignKey(
                name: "Weapons_weapon_type_id_equipment_type_fkey",
                schema: "_equipment",
                table: "Weapons");

            migrationBuilder.Sql(@"
            -- Обратные операции
            ALTER TABLE ""__lists"".""EquipmentType"" RENAME TO ""WeaponTypes"";
            
            -- Восстанавливаем sequence если переименовывали
            DO $$ 
            BEGIN
                IF EXISTS (
                    SELECT 1 FROM pg_class 
                    WHERE relname = 'equipmenttype_id_seq' 
                    AND relnamespace = (SELECT oid FROM pg_namespace WHERE nspname = '__lists')
                ) THEN
                    ALTER SEQUENCE ""__lists"".""equipmenttype_id_seq"" RENAME TO ""weapontypes_id_seq"";
                END IF;
            END $$;
            
            -- Кросс-таблица
            ALTER TABLE ""x_Cross"".""X_EquipmentType_DamageType"" RENAME TO ""X_WeaponType_DamageType"";
            ALTER TABLE ""x_Cross"".""X_WeaponType_DamageType"" RENAME COLUMN ""EquipmentTypeId"" TO ""WeaponTypeId"";
            
            -- Индексы
            ALTER INDEX IF EXISTS ""__lists"".""EquipmentType_name_idx"" RENAME TO ""WeaponTypes_name_idx"";
            ALTER INDEX IF EXISTS ""x_Cross"".""X_EquipmentType_DamageType_EquipmentTypeId_idx"" RENAME TO ""X_WeaponType_DamageType_WeaponTypeId_idx"";
            ALTER INDEX IF EXISTS ""x_Cross"".""X_EquipmentType_DamageType_DamageTypeId_idx"" RENAME TO ""X_WeaponType_DamageType_DamageTypeId_idx"";
        ");

            // Восстанавливаем исходный FK
            migrationBuilder.AddForeignKey(
                name: "Weapons_weapon_type_id_weapon_types_fkey",
                schema: "_equipment",
                table: "Weapons",
                column: "weapon_type_id",
                principalSchema: "__lists",
                principalTable: "WeaponTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
