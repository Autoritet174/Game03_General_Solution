using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.DB.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable_HeroCreatureType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_hero_x_creature_type_creature_types_id_creature_type",
                schema: "relations",
                table: "hero_x_creature_type");

            migrationBuilder.DropForeignKey(
                name: "fk_hero_x_creature_type_heroes_id_heroes",
                schema: "relations",
                table: "hero_x_creature_type");

            migrationBuilder.DropPrimaryKey(
                name: "pk_creature_type",
                schema: "main",
                table: "creature_type");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                schema: "main",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                schema: "main",
                table: "creature_type");

            migrationBuilder.RenameTable(
                name: "creature_type",
                schema: "main",
                newName: "creature_types",
                newSchema: "main");

            migrationBuilder.RenameColumn(
                name: "heroes_id",
                schema: "relations",
                table: "hero_x_creature_type",
                newName: "creature_type_id");

            migrationBuilder.RenameColumn(
                name: "creature_types_id",
                schema: "relations",
                table: "hero_x_creature_type",
                newName: "hero_id");

            migrationBuilder.RenameIndex(
                name: "idx_hero_x_creature_type_heroes_id",
                schema: "relations",
                table: "hero_x_creature_type",
                newName: "idx_hero_x_creature_type_creature_type_id");

            migrationBuilder.RenameIndex(
                name: "idx_creature_type_name",
                schema: "main",
                table: "creature_types",
                newName: "idx_creature_types_name");

            migrationBuilder.AddColumn<int>(
                name: "rarity",
                schema: "main",
                table: "heroes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "relations",
                table: "hero_x_creature_type",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddPrimaryKey(
                name: "pk_creature_types",
                schema: "main",
                table: "creature_types",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "idx_hero_x_creature_type_hero_id",
                schema: "relations",
                table: "hero_x_creature_type",
                column: "hero_id");

            migrationBuilder.AddForeignKey(
                name: "fk_hero_x_creature_type_creature_type_id_creature_types",
                schema: "relations",
                table: "hero_x_creature_type",
                column: "creature_type_id",
                principalSchema: "main",
                principalTable: "creature_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_hero_x_creature_type_hero_id_heroes",
                schema: "relations",
                table: "hero_x_creature_type",
                column: "hero_id",
                principalSchema: "main",
                principalTable: "heroes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "fk_hero_x_creature_type_creature_type_id_creature_types",
            //    schema: "relations",
            //    table: "hero_x_creature_type");

            //migrationBuilder.DropForeignKey(
            //    name: "fk_hero_x_creature_type_hero_id_heroes",
            //    schema: "relations",
            //    table: "hero_x_creature_type");

            //migrationBuilder.DropIndex(
            //    name: "idx_hero_x_creature_type_hero_id",
            //    schema: "relations",
            //    table: "hero_x_creature_type");

            //migrationBuilder.DropPrimaryKey(
            //    name: "pk_creature_types",
            //    schema: "main",
            //    table: "creature_types");

            //migrationBuilder.DropColumn(
            //    name: "rarity",
            //    schema: "main",
            //    table: "heroes");

            //migrationBuilder.DropColumn(
            //    name: "created_at",
            //    schema: "relations",
            //    table: "hero_x_creature_type");

            migrationBuilder.RenameTable(
                name: "creature_types",
                schema: "main",
                newName: "creature_type",
                newSchema: "main");

            migrationBuilder.RenameColumn(
                name: "creature_type_id",
                schema: "relations",
                table: "hero_x_creature_type",
                newName: "heroes_id");

            migrationBuilder.RenameColumn(
                name: "hero_id",
                schema: "relations",
                table: "hero_x_creature_type",
                newName: "creature_types_id");

            migrationBuilder.RenameIndex(
                name: "idx_hero_x_creature_type_creature_type_id",
                schema: "relations",
                table: "hero_x_creature_type",
                newName: "idx_hero_x_creature_type_heroes_id");

            migrationBuilder.RenameIndex(
                name: "idx_creature_types_name",
                schema: "main",
                table: "creature_type",
                newName: "idx_creature_type_name");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                schema: "main",
                table: "heroes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                schema: "main",
                table: "creature_type",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_creature_type",
                schema: "main",
                table: "creature_type",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_hero_x_creature_type_creature_types_id_creature_type",
                schema: "relations",
                table: "hero_x_creature_type",
                column: "creature_types_id",
                principalSchema: "main",
                principalTable: "creature_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_hero_x_creature_type_heroes_id_heroes",
                schema: "relations",
                table: "hero_x_creature_type",
                column: "heroes_id",
                principalSchema: "main",
                principalTable: "heroes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
