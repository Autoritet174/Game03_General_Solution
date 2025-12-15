using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class Equips_Rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(name: "EquipmentTypes", schema: "__lists", newName: "Equips", newSchema: "__lists");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
