using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class EquipmentFix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropIndex(
                name: "heroes__equipment10id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment11id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment12id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment1id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment2id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment3id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment4id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment5id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment6id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment7id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment8id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropIndex(
                name: "heroes__equipment9id__idx",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment10id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment11id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment12id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment1id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment2id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment3id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment4id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment5id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment6id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment7id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment8id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "equipment9id",
                schema: "collection",
                table: "heroes");

            migrationBuilder.AddColumn<Guid>(
                name: "hero_id",
                schema: "collection",
                table: "equipments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "slot_type_id",
                schema: "collection",
                table: "equipments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "slot_type1id",
                schema: "game_data",
                table: "base_equipments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "slot_type2id",
                schema: "game_data",
                table: "base_equipments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "equipments__hero_id__idx",
                schema: "collection",
                table: "equipments",
                column: "hero_id");

            migrationBuilder.CreateIndex(
                name: "equipments__slot_type_id__hero_id__idx",
                schema: "collection",
                table: "equipments",
                columns: new[] { "slot_type_id", "hero_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "base_equipments__slot_type1id__idx",
                schema: "game_data",
                table: "base_equipments",
                column: "slot_type1id");

            migrationBuilder.CreateIndex(
                name: "base_equipments__slot_type2id__idx",
                schema: "game_data",
                table: "base_equipments",
                column: "slot_type2id");

            migrationBuilder.AddForeignKey(
                name: "base_equipments__slot_type1id__slot_types__fkey",
                schema: "game_data",
                table: "base_equipments",
                column: "slot_type1id",
                principalSchema: "game_data",
                principalTable: "slot_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "base_equipments__slot_type2id__slot_types__fkey",
                schema: "game_data",
                table: "base_equipments",
                column: "slot_type2id",
                principalSchema: "game_data",
                principalTable: "slot_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "equipments__hero_id__heroes__fkey",
                schema: "collection",
                table: "equipments",
                column: "hero_id",
                principalSchema: "collection",
                principalTable: "heroes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "equipments__slot_type_id__slot_types__fkey",
                schema: "collection",
                table: "equipments",
                column: "slot_type_id",
                principalSchema: "game_data",
                principalTable: "slot_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "base_equipments__slot_type1id__slot_types__fkey",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropForeignKey(
                name: "base_equipments__slot_type2id__slot_types__fkey",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropForeignKey(
                name: "equipments__hero_id__heroes__fkey",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropForeignKey(
                name: "equipments__slot_type_id__slot_types__fkey",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "equipments__hero_id__idx",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "equipments__slot_type_id__hero_id__idx",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "base_equipments__slot_type1id__idx",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropIndex(
                name: "base_equipments__slot_type2id__idx",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "hero_id",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "slot_type_id",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "slot_type1id",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "slot_type2id",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.AddColumn<Guid>(
                name: "equipment10id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment11id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment12id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment1id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment2id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment3id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment4id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment5id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment6id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment7id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment8id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "equipment9id",
                schema: "collection",
                table: "heroes",
                type: "uuid",
                nullable: true);

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
                name: "heroes__equipment1id__idx",
                schema: "collection",
                table: "heroes",
                column: "equipment1id");

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
        }
    }
}
