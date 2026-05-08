using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260508_085249 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "user_session_inactivation_reasons__code__idx",
                schema: "server",
                table: "user_session_inactivation_reasons");

            migrationBuilder.DropColumn(
                name: "code",
                schema: "server",
                table: "user_session_inactivation_reasons");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "game_data",
                table: "battlefields",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "user_session_inactivation_reasons__name__idx",
                schema: "server",
                table: "user_session_inactivation_reasons",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "user_session_inactivation_reasons__name__idx",
                schema: "server",
                table: "user_session_inactivation_reasons");

            migrationBuilder.AddColumn<int>(
                name: "code",
                schema: "server",
                table: "user_session_inactivation_reasons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "game_data",
                table: "battlefields",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "user_session_inactivation_reasons__code__idx",
                schema: "server",
                table: "user_session_inactivation_reasons",
                column: "code",
                unique: true);
        }
    }
}
