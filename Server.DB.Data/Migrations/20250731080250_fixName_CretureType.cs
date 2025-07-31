using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.DB.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixName_CretureType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_hero_x_creature_type_creature_types_id_creature_type",
                schema: "relations",
                table: "hero_x_creature_type");

            migrationBuilder.DropPrimaryKey(
                name: "pk_creature_type",
                schema: "main",
                table: "creature_type");

            migrationBuilder.RenameTable(
                name: "creature_type",
                schema: "main",
                newName: "creature_types",
                newSchema: "main");

            migrationBuilder.RenameIndex(
                name: "idx_creature_type_name",
                schema: "main",
                table: "creature_types",
                newName: "idx_creature_types_name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_creature_types",
                schema: "main",
                table: "creature_types",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_hero_x_creature_type_creature_types_id_creature_types",
                schema: "relations",
                table: "hero_x_creature_type",
                column: "creature_types_id",
                principalSchema: "main",
                principalTable: "creature_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_hero_x_creature_type_creature_types_id_creature_types",
                schema: "relations",
                table: "hero_x_creature_type");

            migrationBuilder.DropPrimaryKey(
                name: "pk_creature_types",
                schema: "main",
                table: "creature_types");

            migrationBuilder.RenameTable(
                name: "creature_types",
                schema: "main",
                newName: "creature_type",
                newSchema: "main");

            migrationBuilder.RenameIndex(
                name: "idx_creature_types_name",
                schema: "main",
                table: "creature_type",
                newName: "idx_creature_type_name");

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
        }
    }
}
