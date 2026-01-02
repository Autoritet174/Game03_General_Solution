using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class IdentityFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "user_authorizations__user_id__users__fkey",
                schema: "logs",
                table: "user_authorizations");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "logs",
                table: "user_authorizations",
                newName: "application_user_id");

            migrationBuilder.RenameIndex(
                name: "user_authorizations__user_id__idx",
                schema: "logs",
                table: "user_authorizations",
                newName: "user_authorizations__application_user_id__idx");

            migrationBuilder.AddForeignKey(
                name: "user_authorizations__application_user_id__asp_net_users__fkey",
                schema: "logs",
                table: "user_authorizations",
                column: "application_user_id",
                principalSchema: "auth",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "user_authorizations__application_user_id__asp_net_users__fkey",
                schema: "logs",
                table: "user_authorizations");

            migrationBuilder.RenameColumn(
                name: "application_user_id",
                schema: "logs",
                table: "user_authorizations",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "user_authorizations__application_user_id__idx",
                schema: "logs",
                table: "user_authorizations",
                newName: "user_authorizations__user_id__idx");

            migrationBuilder.AddForeignKey(
                name: "user_authorizations__user_id__users__fkey",
                schema: "logs",
                table: "user_authorizations",
                column: "user_id",
                principalSchema: "users",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
