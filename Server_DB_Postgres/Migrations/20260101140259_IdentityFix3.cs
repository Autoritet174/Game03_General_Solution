using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class IdentityFix3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "user_authorizations__application_user_id__asp_net_users__fkey",
                schema: "logs",
                table: "user_authorizations");

            migrationBuilder.DropForeignKey(
                name: "user_bans__application_user_id__asp_net_users__fkey",
                schema: "users",
                table: "user_bans");

            migrationBuilder.AddForeignKey(
                name: "user_authorizations__application_user_id__asp_net_users__fkey",
                schema: "logs",
                table: "user_authorizations",
                column: "application_user_id",
                principalSchema: "auth",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "user_bans__application_user_id__asp_net_users__fkey",
                schema: "users",
                table: "user_bans",
                column: "application_user_id",
                principalSchema: "auth",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "user_authorizations__application_user_id__asp_net_users__fkey",
                schema: "logs",
                table: "user_authorizations");

            migrationBuilder.DropForeignKey(
                name: "user_bans__application_user_id__asp_net_users__fkey",
                schema: "users",
                table: "user_bans");

            migrationBuilder.AddForeignKey(
                name: "user_authorizations__application_user_id__asp_net_users__fkey",
                schema: "logs",
                table: "user_authorizations",
                column: "application_user_id",
                principalSchema: "auth",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "user_bans__application_user_id__asp_net_users__fkey",
                schema: "users",
                table: "user_bans",
                column: "application_user_id",
                principalSchema: "auth",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
