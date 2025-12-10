using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class SmithingMaterials_Fix3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "SmithingMaterials_damage_type_id2_damage_types_fkey",
                schema: "__lists",
                table: "SmithingMaterials");

            migrationBuilder.DropForeignKey(
                name: "SmithingMaterials_damage_type_id3_damage_types_fkey",
                schema: "__lists",
                table: "SmithingMaterials");

            migrationBuilder.DropForeignKey(
                name: "SmithingMaterials_damage_type_id4_damage_types_fkey",
                schema: "__lists",
                table: "SmithingMaterials");

            migrationBuilder.DropForeignKey(
                name: "SmithingMaterials_damage_type_id_damage_types_fkey",
                schema: "__lists",
                table: "SmithingMaterials");

            migrationBuilder.DropIndex(
                name: "SmithingMaterials_damage_type_id_idx",
                schema: "__lists",
                table: "SmithingMaterials");

            migrationBuilder.DropIndex(
                name: "SmithingMaterials_damage_type_id2_idx",
                schema: "__lists",
                table: "SmithingMaterials");

            migrationBuilder.DropIndex(
                name: "SmithingMaterials_damage_type_id3_idx",
                schema: "__lists",
                table: "SmithingMaterials");

            migrationBuilder.DropIndex(
                name: "SmithingMaterials_damage_type_id4_idx",
                schema: "__lists",
                table: "SmithingMaterials");

            migrationBuilder.DropColumn(
                name: "coef_damage",
                schema: "__lists",
                table: "SmithingMaterials");

            migrationBuilder.DropColumn(
                name: "coef_damage2",
                schema: "__lists",
                table: "SmithingMaterials");

            migrationBuilder.DropColumn(
                name: "coef_damage3",
                schema: "__lists",
                table: "SmithingMaterials");

            migrationBuilder.DropColumn(
                name: "coef_damage4",
                schema: "__lists",
                table: "SmithingMaterials");

            migrationBuilder.DropColumn(
                name: "damage_type_id",
                schema: "__lists",
                table: "SmithingMaterials");

            migrationBuilder.DropColumn(
                name: "damage_type_id2",
                schema: "__lists",
                table: "SmithingMaterials");

            migrationBuilder.DropColumn(
                name: "damage_type_id3",
                schema: "__lists",
                table: "SmithingMaterials");

            migrationBuilder.DropColumn(
                name: "damage_type_id4",
                schema: "__lists",
                table: "SmithingMaterials");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "coef_damage",
                schema: "__lists",
                table: "SmithingMaterials",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "coef_damage2",
                schema: "__lists",
                table: "SmithingMaterials",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "coef_damage3",
                schema: "__lists",
                table: "SmithingMaterials",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "coef_damage4",
                schema: "__lists",
                table: "SmithingMaterials",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "damage_type_id",
                schema: "__lists",
                table: "SmithingMaterials",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "damage_type_id2",
                schema: "__lists",
                table: "SmithingMaterials",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "damage_type_id3",
                schema: "__lists",
                table: "SmithingMaterials",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "damage_type_id4",
                schema: "__lists",
                table: "SmithingMaterials",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "SmithingMaterials_damage_type_id_idx",
                schema: "__lists",
                table: "SmithingMaterials",
                column: "damage_type_id");

            migrationBuilder.CreateIndex(
                name: "SmithingMaterials_damage_type_id2_idx",
                schema: "__lists",
                table: "SmithingMaterials",
                column: "damage_type_id2");

            migrationBuilder.CreateIndex(
                name: "SmithingMaterials_damage_type_id3_idx",
                schema: "__lists",
                table: "SmithingMaterials",
                column: "damage_type_id3");

            migrationBuilder.CreateIndex(
                name: "SmithingMaterials_damage_type_id4_idx",
                schema: "__lists",
                table: "SmithingMaterials",
                column: "damage_type_id4");

            migrationBuilder.AddForeignKey(
                name: "SmithingMaterials_damage_type_id2_damage_types_fkey",
                schema: "__lists",
                table: "SmithingMaterials",
                column: "damage_type_id2",
                principalSchema: "__lists",
                principalTable: "DamageTypes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "SmithingMaterials_damage_type_id3_damage_types_fkey",
                schema: "__lists",
                table: "SmithingMaterials",
                column: "damage_type_id3",
                principalSchema: "__lists",
                principalTable: "DamageTypes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "SmithingMaterials_damage_type_id4_damage_types_fkey",
                schema: "__lists",
                table: "SmithingMaterials",
                column: "damage_type_id4",
                principalSchema: "__lists",
                principalTable: "DamageTypes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "SmithingMaterials_damage_type_id_damage_types_fkey",
                schema: "__lists",
                table: "SmithingMaterials",
                column: "damage_type_id",
                principalSchema: "__lists",
                principalTable: "DamageTypes",
                principalColumn: "id");
        }
    }
}
