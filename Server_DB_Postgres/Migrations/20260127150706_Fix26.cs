using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "user_session_inactivation_reason_id",
                schema: "users",
                table: "user_sessions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "user_session_inactivation_reason_id",
                schema: "users",
                table: "user_sessions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
