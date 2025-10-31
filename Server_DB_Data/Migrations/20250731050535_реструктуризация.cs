using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class реструктуризация : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_hero_creature_type_creature_types_id_creature_types",
                table: "hero_creature_type");

            migrationBuilder.DropForeignKey(
                name: "fk_hero_creature_type_heroes_id_heroes",
                table: "hero_creature_type");

            migrationBuilder.DropPrimaryKey(
                name: "pk_hero_creature_type",
                table: "hero_creature_type");

            migrationBuilder.DropPrimaryKey(
                name: "pk_creature_types",
                table: "creature_types");

            migrationBuilder.EnsureSchema(
                name: "main");

            migrationBuilder.EnsureSchema(
                name: "relations");

            migrationBuilder.RenameTable(
                name: "heroes",
                newName: "heroes",
                newSchema: "main");

            migrationBuilder.RenameTable(
                name: "hero_creature_type",
                newName: "hero_x_creature_type",
                newSchema: "relations");

            migrationBuilder.RenameTable(
                name: "creature_types",
                newName: "creature_type",
                newSchema: "main");

            migrationBuilder.RenameIndex(
                name: "idx_hero_creature_type_heroes_id",
                schema: "relations",
                table: "hero_x_creature_type",
                newName: "idx_hero_x_creature_type_heroes_id");

            migrationBuilder.RenameIndex(
                name: "idx_creature_types_name",
                schema: "main",
                table: "creature_type",
                newName: "idx_creature_type_name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_hero_x_creature_type",
                schema: "relations",
                table: "hero_x_creature_type",
                columns: new[] { "creature_types_id", "heroes_id" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "pk_hero_x_creature_type",
                schema: "relations",
                table: "hero_x_creature_type");

            migrationBuilder.DropPrimaryKey(
                name: "pk_creature_type",
                schema: "main",
                table: "creature_type");

            migrationBuilder.RenameTable(
                name: "heroes",
                schema: "main",
                newName: "heroes");

            migrationBuilder.RenameTable(
                name: "hero_x_creature_type",
                schema: "relations",
                newName: "hero_creature_type");

            migrationBuilder.RenameTable(
                name: "creature_type",
                schema: "main",
                newName: "creature_types");

            migrationBuilder.RenameIndex(
                name: "idx_hero_x_creature_type_heroes_id",
                table: "hero_creature_type",
                newName: "idx_hero_creature_type_heroes_id");

            migrationBuilder.RenameIndex(
                name: "idx_creature_type_name",
                table: "creature_types",
                newName: "idx_creature_types_name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_hero_creature_type",
                table: "hero_creature_type",
                columns: new[] { "creature_types_id", "heroes_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_creature_types",
                table: "creature_types",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_hero_creature_type_creature_types_id_creature_types",
                table: "hero_creature_type",
                column: "creature_types_id",
                principalTable: "creature_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_hero_creature_type_heroes_id_heroes",
                table: "hero_creature_type",
                column: "heroes_id",
                principalTable: "heroes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
