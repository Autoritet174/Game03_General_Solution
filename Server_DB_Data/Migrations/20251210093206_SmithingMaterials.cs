using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class SmithingMaterials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmithingMaterials",
                schema: "__lists",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    name_ru = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    damage_type_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SmithingMaterials_pkey", x => x.id);
                    table.ForeignKey(
                        name: "SmithingMaterials_damage_type_id_damage_types_fkey",
                        column: x => x.damage_type_id,
                        principalSchema: "__lists",
                        principalTable: "DamageTypes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "SmithingMaterials_damage_type_id_idx",
                schema: "__lists",
                table: "SmithingMaterials",
                column: "damage_type_id");

            migrationBuilder.CreateIndex(
                name: "SmithingMaterials_name_idx",
                schema: "__lists",
                table: "SmithingMaterials",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmithingMaterials",
                schema: "__lists");
        }
    }
}
