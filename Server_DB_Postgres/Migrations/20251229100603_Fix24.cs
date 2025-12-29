using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "base_equipments__equipment_type_id__equipment_types__fkey",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropForeignKey(
                name: "base_equipments__smithing_material_id__smithing_materials__fkey",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropForeignKey(
                name: "equipment_types__slot_type_id__slot_types__fkey",
                schema: "game_data",
                table: "equipment_types");

            migrationBuilder.DropForeignKey(
                name: "equipments__base_equipment_id__base_equipments__fkey",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropForeignKey(
                name: "equipments__user_id__users__fkey",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropForeignKey(
                name: "heroes__base_hero_id__base_heroes__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment10id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment11id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment12id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment13id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment14id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment15id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment16id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment17id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment18id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment19id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment1id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment20id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment21id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment22id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment23id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment24id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment2id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment3id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment4id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment5id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment6id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment7id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment8id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment9id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__user_id__users__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "material_damage_percents__damage_type_id__damage_types__fkey",
                schema: "game_data",
                table: "material_damage_percents");

            migrationBuilder.DropForeignKey(
                name: "material_damage_percents__smithing_materials_id__smithing_materials__fkey",
                schema: "game_data",
                table: "material_damage_percents");

            migrationBuilder.DropForeignKey(
                name: "user_authorizations__user_device_id__user_devices__fkey",
                schema: "logs",
                table: "user_authorizations");

            migrationBuilder.DropForeignKey(
                name: "user_authorizations__user_id__users__fkey",
                schema: "logs",
                table: "user_authorizations");

            migrationBuilder.DropForeignKey(
                name: "user_bans__user_ban_reason_id__user_ban_reasons__fkey",
                schema: "users",
                table: "user_bans");

            migrationBuilder.DropForeignKey(
                name: "user_bans__user_id__users__fkey",
                schema: "users",
                table: "user_bans");

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

            migrationBuilder.DropIndex(
                name: "heroes__equipment13id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment14id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment15id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment16id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment17id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment18id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment19id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment20id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment21id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment22id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment23id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment24id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment13id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment14id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment15id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment16id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment17id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment18id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment19id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment20id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment21id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment22id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment23id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment24id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.AddForeignKey(
                name: "base_equipments__equipment_type_id__equipment_types__fkey",
                schema: "game_data",
                table: "base_equipments",
                column: "equipment_type_id",
                principalSchema: "game_data",
                principalTable: "equipment_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "base_equipments__smithing_material_id__smithing_materials__fkey",
                schema: "game_data",
                table: "base_equipments",
                column: "smithing_material_id",
                principalSchema: "game_data",
                principalTable: "smithing_materials",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "equipment_types__slot_type_id__slot_types__fkey",
                schema: "game_data",
                table: "equipment_types",
                column: "slot_type_id",
                principalSchema: "game_data",
                principalTable: "slot_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "equipments__base_equipment_id__base_equipments__fkey",
                schema: "collection",
                table: "equipments",
                column: "base_equipment_id",
                principalSchema: "game_data",
                principalTable: "base_equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "equipments__user_id__users__fkey",
                schema: "collection",
                table: "equipments",
                column: "user_id",
                principalSchema: "users",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "heroes__base_hero_id__base_heroes__fkey",
                schema: "collection",
                table: "heroes",
                column: "base_hero_id",
                principalSchema: "game_data",
                principalTable: "base_heroes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment10id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment10id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment11id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment11id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment12id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment12id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment1id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment1id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment2id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment2id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment3id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment3id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment4id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment4id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment5id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment5id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment6id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment6id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment7id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment7id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment8id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment8id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment9id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment9id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "heroes__user_id__users__fkey",
                schema: "collection",
                table: "heroes",
                column: "user_id",
                principalSchema: "users",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "material_damage_percents__damage_type_id__damage_types__fkey",
                schema: "game_data",
                table: "material_damage_percents",
                column: "damage_type_id",
                principalSchema: "game_data",
                principalTable: "damage_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "material_damage_percents__smithing_materials_id__smithing_materials__fkey",
                schema: "game_data",
                table: "material_damage_percents",
                column: "smithing_materials_id",
                principalSchema: "game_data",
                principalTable: "smithing_materials",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "user_authorizations__user_device_id__user_devices__fkey",
                schema: "logs",
                table: "user_authorizations",
                column: "user_device_id",
                principalSchema: "users",
                principalTable: "user_devices",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "user_authorizations__user_id__users__fkey",
                schema: "logs",
                table: "user_authorizations",
                column: "user_id",
                principalSchema: "users",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "user_bans__user_ban_reason_id__user_ban_reasons__fkey",
                schema: "users",
                table: "user_bans",
                column: "user_ban_reason_id",
                principalSchema: "server",
                principalTable: "user_ban_reasons",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "user_bans__user_id__users__fkey",
                schema: "users",
                table: "user_bans",
                column: "user_id",
                principalSchema: "users",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "base_equipments__equipment_type_id__equipment_types__fkey",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropForeignKey(
                name: "base_equipments__smithing_material_id__smithing_materials__fkey",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropForeignKey(
                name: "equipment_types__slot_type_id__slot_types__fkey",
                schema: "game_data",
                table: "equipment_types");

            migrationBuilder.DropForeignKey(
                name: "equipments__base_equipment_id__base_equipments__fkey",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropForeignKey(
                name: "equipments__user_id__users__fkey",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropForeignKey(
                name: "heroes__base_hero_id__base_heroes__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment10id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment11id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment12id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment1id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment2id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment3id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment4id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment5id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment6id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment7id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment8id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__equipment9id__equipments__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "heroes__user_id__users__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "material_damage_percents__damage_type_id__damage_types__fkey",
                schema: "game_data",
                table: "material_damage_percents");

            migrationBuilder.DropForeignKey(
                name: "material_damage_percents__smithing_materials_id__smithing_materials__fkey",
                schema: "game_data",
                table: "material_damage_percents");

            migrationBuilder.DropForeignKey(
                name: "user_authorizations__user_device_id__user_devices__fkey",
                schema: "logs",
                table: "user_authorizations");

            migrationBuilder.DropForeignKey(
                name: "user_authorizations__user_id__users__fkey",
                schema: "logs",
                table: "user_authorizations");

            migrationBuilder.DropForeignKey(
                name: "user_bans__user_ban_reason_id__user_ban_reasons__fkey",
                schema: "users",
                table: "user_bans");

            migrationBuilder.DropForeignKey(
                name: "user_bans__user_id__users__fkey",
                schema: "users",
                table: "user_bans");

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

            migrationBuilder.AddColumn<Guid>(
                name: "equipment13id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment14id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment15id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment16id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment17id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment18id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment19id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment20id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment21id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment22id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment23id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment24id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "base_equipments__equipment_type_id__equipment_types__fkey",
                schema: "game_data",
                table: "base_equipments",
                column: "equipment_type_id",
                principalSchema: "game_data",
                principalTable: "equipment_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "base_equipments__smithing_material_id__smithing_materials__fkey",
                schema: "game_data",
                table: "base_equipments",
                column: "smithing_material_id",
                principalSchema: "game_data",
                principalTable: "smithing_materials",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "equipment_types__slot_type_id__slot_types__fkey",
                schema: "game_data",
                table: "equipment_types",
                column: "slot_type_id",
                principalSchema: "game_data",
                principalTable: "slot_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "equipments__base_equipment_id__base_equipments__fkey",
                schema: "collection",
                table: "equipments",
                column: "base_equipment_id",
                principalSchema: "game_data",
                principalTable: "base_equipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "equipments__user_id__users__fkey",
                schema: "collection",
                table: "equipments",
                column: "user_id",
                principalSchema: "users",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "heroes__base_hero_id__base_heroes__fkey",
                schema: "collection",
                table: "heroes",
                column: "base_hero_id",
                principalSchema: "game_data",
                principalTable: "base_heroes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment10id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment10id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment11id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment11id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment12id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment12id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment13id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment13id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment14id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment14id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment15id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment15id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment16id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment16id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment17id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment17id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment18id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment18id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment19id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment19id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment1id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment1id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment20id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment20id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment21id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment21id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment22id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment22id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment23id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment23id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment24id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment24id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment2id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment2id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment3id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment3id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment4id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment4id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment5id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment5id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment6id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment6id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment7id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment7id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment8id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment8id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__equipment9id__equipments__fkey",
                schema: "collection",
                table: "heroes",
                column: "equipment9id",
                principalSchema: "collection",
                principalTable: "equipments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "heroes__user_id__users__fkey",
                schema: "collection",
                table: "heroes",
                column: "user_id",
                principalSchema: "users",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "material_damage_percents__damage_type_id__damage_types__fkey",
                schema: "game_data",
                table: "material_damage_percents",
                column: "damage_type_id",
                principalSchema: "game_data",
                principalTable: "damage_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "material_damage_percents__smithing_materials_id__smithing_materials__fkey",
                schema: "game_data",
                table: "material_damage_percents",
                column: "smithing_materials_id",
                principalSchema: "game_data",
                principalTable: "smithing_materials",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "user_authorizations__user_device_id__user_devices__fkey",
                schema: "logs",
                table: "user_authorizations",
                column: "user_device_id",
                principalSchema: "users",
                principalTable: "user_devices",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "user_authorizations__user_id__users__fkey",
                schema: "logs",
                table: "user_authorizations",
                column: "user_id",
                principalSchema: "users",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "user_bans__user_ban_reason_id__user_ban_reasons__fkey",
                schema: "users",
                table: "user_bans",
                column: "user_ban_reason_id",
                principalSchema: "server",
                principalTable: "user_ban_reasons",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "user_bans__user_id__users__fkey",
                schema: "users",
                table: "user_bans",
                column: "user_id",
                principalSchema: "users",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "x_equipment_type_damage_type__damage_type_id__damage_types__fkey",
                schema: "game_data",
                table: "x_equipment_type_damage_type",
                column: "damage_type_id",
                principalSchema: "game_data",
                principalTable: "damage_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "x_equipment_type_damage_type__equipment_type_id__equipment_types__fkey",
                schema: "game_data",
                table: "x_equipment_type_damage_type",
                column: "equipment_type_id",
                principalSchema: "game_data",
                principalTable: "equipment_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "x_hero_creature_type__base_hero_id__base_heroes__fkey",
                schema: "game_data",
                table: "x_hero_creature_type",
                column: "base_hero_id",
                principalSchema: "game_data",
                principalTable: "base_heroes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "x_hero_creature_type__creature_type_id__creature_types__fkey",
                schema: "game_data",
                table: "x_hero_creature_type",
                column: "creature_type_id",
                principalSchema: "game_data",
                principalTable: "creature_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
