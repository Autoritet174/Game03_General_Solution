using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260504_113602 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "x_battlefield_npcs__pkey",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.DropColumn(
                name: "rank",
                schema: "game_data",
                table: "npcs");

            migrationBuilder.AlterColumn<int>(
                name: "npc_id",
                schema: "game_data",
                table: "x_battlefield_npcs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "battlefield_id",
                schema: "game_data",
                table: "x_battlefield_npcs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<int>(
                name: "id",
                schema: "game_data",
                table: "x_battlefield_npcs",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<bool>(
                name: "guarant_spawn",
                schema: "game_data",
                table: "x_battlefield_npcs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "possible_rank",
                schema: "game_data",
                table: "x_battlefield_npcs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "probability_spawn",
                schema: "game_data",
                table: "x_battlefield_npcs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "x_battlefield_npcs__pkey",
                schema: "game_data",
                table: "x_battlefield_npcs",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "x_battlefield_npcs__battlefield_id__idx",
                schema: "game_data",
                table: "x_battlefield_npcs",
                column: "battlefield_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "x_battlefield_npcs__pkey",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.DropIndex(
                name: "x_battlefield_npcs__battlefield_id__idx",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.DropColumn(
                name: "id",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.DropColumn(
                name: "guarant_spawn",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.DropColumn(
                name: "possible_rank",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.DropColumn(
                name: "probability_spawn",
                schema: "game_data",
                table: "x_battlefield_npcs");

            migrationBuilder.AlterColumn<int>(
                name: "npc_id",
                schema: "game_data",
                table: "x_battlefield_npcs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "battlefield_id",
                schema: "game_data",
                table: "x_battlefield_npcs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<int>(
                name: "rank",
                schema: "game_data",
                table: "npcs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "x_battlefield_npcs__pkey",
                schema: "game_data",
                table: "x_battlefield_npcs",
                columns: new[] { "battlefield_id", "npc_id" });
        }
    }
}
