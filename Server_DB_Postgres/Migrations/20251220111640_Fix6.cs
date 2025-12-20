using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "health",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "attack",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<long>(
                name: "experience_now",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "group_name",
                schema: "collection",
                table: "heroes",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "level",
                schema: "collection",
                table: "heroes",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "rarity",
                schema: "collection",
                table: "heroes",
                type: "integer",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "experience_now",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "group_name",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "level",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "rarity",
                schema: "collection",
                table: "heroes");

            migrationBuilder.AlterColumn<int>(
                name: "health",
                schema: "collection",
                table: "heroes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "attack",
                schema: "collection",
                table: "heroes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
