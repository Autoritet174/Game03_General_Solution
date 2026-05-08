using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260508_232329 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "registration_logs__user_id__identity_users__fkey",
                schema: "logs",
                table: "registration_logs");

            migrationBuilder.AddForeignKey(
                name: "registration_logs__user_id__identity_users__fkey",
                schema: "logs",
                table: "registration_logs",
                column: "user_id",
                principalSchema: "users",
                principalTable: "identity_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "registration_logs__user_id__identity_users__fkey",
                schema: "logs",
                table: "registration_logs");

            migrationBuilder.AddForeignKey(
                name: "registration_logs__user_id__identity_users__fkey",
                schema: "logs",
                table: "registration_logs",
                column: "user_id",
                principalSchema: "users",
                principalTable: "identity_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
