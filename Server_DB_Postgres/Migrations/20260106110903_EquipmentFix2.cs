using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class EquipmentFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "equipments__slot_type_id__slot_types__fkey",
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
                name: "slot_type1id",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.DropColumn(
                name: "slot_type2id",
                schema: "game_data",
                table: "base_equipments");

            migrationBuilder.RenameColumn(
                name: "slot_type_id",
                schema: "collection",
                table: "equipments",
                newName: "slot_id");

            migrationBuilder.RenameIndex(
                name: "equipments__slot_type_id__hero_id__idx",
                schema: "collection",
                table: "equipments",
                newName: "equipments__slot_id__hero_id__idx");

            migrationBuilder.CreateTable(
                name: "slots",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    slot_type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("slots__pkey", x => x.id);
                    table.ForeignKey(
                        name: "slots__slot_type_id__slot_types__fkey",
                        column: x => x.slot_type_id,
                        principalSchema: "game_data",
                        principalTable: "slot_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "slots__name__idx",
                schema: "game_data",
                table: "slots",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "slots__slot_type_id__idx",
                schema: "game_data",
                table: "slots",
                column: "slot_type_id");

            migrationBuilder.AddForeignKey(
                name: "equipments__slot_id__slots__fkey",
                schema: "collection",
                table: "equipments",
                column: "slot_id",
                principalSchema: "game_data",
                principalTable: "slots",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "equipments__slot_id__slots__fkey",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropTable(
                name: "slots",
                schema: "game_data");

            migrationBuilder.RenameColumn(
                name: "slot_id",
                schema: "collection",
                table: "equipments",
                newName: "slot_type_id");

            migrationBuilder.RenameIndex(
                name: "equipments__slot_id__hero_id__idx",
                schema: "collection",
                table: "equipments",
                newName: "equipments__slot_type_id__hero_id__idx");

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
                name: "equipments__slot_type_id__slot_types__fkey",
                schema: "collection",
                table: "equipments",
                column: "slot_type_id",
                principalSchema: "game_data",
                principalTable: "slot_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
