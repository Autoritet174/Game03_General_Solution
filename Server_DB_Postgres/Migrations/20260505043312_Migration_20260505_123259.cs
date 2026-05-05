using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260505_123259 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "x_heroes_creature_types__pkey",
                schema: "game_data",
                table: "x_heroes_creature_types");

            migrationBuilder.DropPrimaryKey(
                name: "x_equipment_types_damage_types__pkey",
                schema: "game_data",
                table: "x_equipment_types_damage_types");

            migrationBuilder.AlterColumn<int>(
                name: "creature_type_id",
                schema: "game_data",
                table: "x_heroes_creature_types",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "base_hero_id",
                schema: "game_data",
                table: "x_heroes_creature_types",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<int>(
                name: "id",
                schema: "game_data",
                table: "x_heroes_creature_types",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "damage_type_id",
                schema: "game_data",
                table: "x_equipment_types_damage_types",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "equipment_type_id",
                schema: "game_data",
                table: "x_equipment_types_damage_types",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<int>(
                name: "id",
                schema: "game_data",
                table: "x_equipment_types_damage_types",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "x_heroes_creature_types__pkey",
                schema: "game_data",
                table: "x_heroes_creature_types",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "x_equipment_types_damage_types__pkey",
                schema: "game_data",
                table: "x_equipment_types_damage_types",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "x_heroes_creature_types__base_hero_id__idx",
                schema: "game_data",
                table: "x_heroes_creature_types",
                column: "base_hero_id");

            migrationBuilder.CreateIndex(
                name: "x_equipment_types_damage_types__equipment_type_id__idx",
                schema: "game_data",
                table: "x_equipment_types_damage_types",
                column: "equipment_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "x_heroes_creature_types__pkey",
                schema: "game_data",
                table: "x_heroes_creature_types");

            migrationBuilder.DropIndex(
                name: "x_heroes_creature_types__base_hero_id__idx",
                schema: "game_data",
                table: "x_heroes_creature_types");

            migrationBuilder.DropPrimaryKey(
                name: "x_equipment_types_damage_types__pkey",
                schema: "game_data",
                table: "x_equipment_types_damage_types");

            migrationBuilder.DropIndex(
                name: "x_equipment_types_damage_types__equipment_type_id__idx",
                schema: "game_data",
                table: "x_equipment_types_damage_types");

            migrationBuilder.DropColumn(
                name: "id",
                schema: "game_data",
                table: "x_heroes_creature_types");

            migrationBuilder.DropColumn(
                name: "id",
                schema: "game_data",
                table: "x_equipment_types_damage_types");

            migrationBuilder.AlterColumn<int>(
                name: "creature_type_id",
                schema: "game_data",
                table: "x_heroes_creature_types",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "base_hero_id",
                schema: "game_data",
                table: "x_heroes_creature_types",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<int>(
                name: "equipment_type_id",
                schema: "game_data",
                table: "x_equipment_types_damage_types",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<int>(
                name: "damage_type_id",
                schema: "game_data",
                table: "x_equipment_types_damage_types",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 1);

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
        }
    }
}
