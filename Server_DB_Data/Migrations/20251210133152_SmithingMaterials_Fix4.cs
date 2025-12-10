using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class SmithingMaterials_Fix4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialDamagePercent",
                schema: "__lists",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    smithing_materials_id = table.Column<int>(type: "integer", nullable: false),
                    damage_type_id = table.Column<int>(type: "integer", nullable: false),
                    percent = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("MaterialDamagePercent_pkey", x => x.id);
                    table.ForeignKey(
                        name: "MaterialDamagePercent_damage_type_id_damage_types_fkey",
                        column: x => x.damage_type_id,
                        principalSchema: "__lists",
                        principalTable: "DamageTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "MaterialDamagePercent_smithing_materials_id_smithing_materials_fkey",
                        column: x => x.smithing_materials_id,
                        principalSchema: "__lists",
                        principalTable: "SmithingMaterials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "MaterialDamagePercent_damage_type_id_idx",
                schema: "__lists",
                table: "MaterialDamagePercent",
                column: "damage_type_id");

            migrationBuilder.CreateIndex(
                name: "MaterialDamagePercent_smithing_materials_id_idx",
                schema: "__lists",
                table: "MaterialDamagePercent",
                column: "smithing_materials_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialDamagePercent",
                schema: "__lists");
        }
    }
}
