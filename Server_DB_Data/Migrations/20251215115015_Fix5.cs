using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "X_Hero_CreatureType__HeroId__idx",
                schema: "x_Cross",
                table: "X_Hero_CreatureType");

            migrationBuilder.DropIndex(
                name: "X_EquipmentType_DamageType__EquipmentTypeId__idx",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType");

            migrationBuilder.AlterColumn<int>(
                name: "main_stat",
                schema: "_heroes",
                table: "Heroes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "is_unique",
                schema: "_heroes",
                table: "Heroes",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<int>(
                name: "spend_action_points",
                schema: "__lists",
                table: "EquipmentTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "main_stat",
                schema: "_heroes",
                table: "Heroes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "is_unique",
                schema: "_heroes",
                table: "Heroes",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "spend_action_points",
                schema: "__lists",
                table: "EquipmentTypes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "X_Hero_CreatureType__HeroId__idx",
                schema: "x_Cross",
                table: "X_Hero_CreatureType",
                column: "HeroId");

            migrationBuilder.CreateIndex(
                name: "X_EquipmentType_DamageType__EquipmentTypeId__idx",
                schema: "x_Cross",
                table: "X_EquipmentType_DamageType",
                column: "EquipmentTypeId");
        }
    }
}
