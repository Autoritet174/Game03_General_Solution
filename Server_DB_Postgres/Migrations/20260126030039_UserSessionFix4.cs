using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class UserSessionFix4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "inactivation_reason",
                schema: "users",
                table: "user_sessions");

            migrationBuilder.AddColumn<int>(
                name: "user_session_inactivation_reason_id",
                schema: "users",
                table: "user_sessions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "user_session_inactivation_reasons",
                schema: "server",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_session_inactivation_reasons__pkey", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "user_sessions__user_session_inactivation_reason_id__idx",
                schema: "users",
                table: "user_sessions",
                column: "user_session_inactivation_reason_id");

            migrationBuilder.CreateIndex(
                name: "user_session_inactivation_reasons__code__idx",
                schema: "server",
                table: "user_session_inactivation_reasons",
                column: "code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "user_sessions__user_session_inactivation_reason_id__user_session_inactivation_reasons__fkey",
                schema: "users",
                table: "user_sessions",
                column: "user_session_inactivation_reason_id",
                principalSchema: "server",
                principalTable: "user_session_inactivation_reasons",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "user_sessions__user_session_inactivation_reason_id__user_session_inactivation_reasons__fkey",
                schema: "users",
                table: "user_sessions");

            migrationBuilder.DropTable(
                name: "user_session_inactivation_reasons",
                schema: "server");

            migrationBuilder.DropIndex(
                name: "user_sessions__user_session_inactivation_reason_id__idx",
                schema: "users",
                table: "user_sessions");

            migrationBuilder.DropColumn(
                name: "user_session_inactivation_reason_id",
                schema: "users",
                table: "user_sessions");

            migrationBuilder.AddColumn<string>(
                name: "inactivation_reason",
                schema: "users",
                table: "user_sessions",
                type: "text",
                nullable: true);
        }
    }
}
