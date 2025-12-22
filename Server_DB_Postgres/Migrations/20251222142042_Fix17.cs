using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "group_name",
                schema: "collection",
                table: "equipments",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "group_name",
                schema: "collection",
                table: "equipments");
        }
    }
}
