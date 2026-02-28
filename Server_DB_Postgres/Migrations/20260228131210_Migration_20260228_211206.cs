using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260228_211206 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "resist_damage_magical1000",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "resist_damage_physical1000",
                schema: "collection",
                table: "heroes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "resist_damage_magical1000",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "resist_damage_physical1000",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
