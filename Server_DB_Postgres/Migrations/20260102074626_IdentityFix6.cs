using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class IdentityFix6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "asp_net_role_claims__role_id__asp_net_roles__fkey",
                schema: "auth",
                table: "asp_net_role_claims");

            migrationBuilder.DropForeignKey(
                name: "asp_net_user_claims__user_id__asp_net_users__fkey",
                schema: "auth",
                table: "asp_net_user_claims");

            migrationBuilder.DropForeignKey(
                name: "asp_net_user_logins__user_id__asp_net_users__fkey",
                schema: "auth",
                table: "asp_net_user_logins");

            migrationBuilder.DropForeignKey(
                name: "asp_net_user_roles__role_id__asp_net_roles__fkey",
                schema: "auth",
                table: "asp_net_user_roles");

            migrationBuilder.DropForeignKey(
                name: "asp_net_user_roles__user_id__asp_net_users__fkey",
                schema: "auth",
                table: "asp_net_user_roles");

            migrationBuilder.DropForeignKey(
                name: "asp_net_user_tokens__user_id__asp_net_users__fkey",
                schema: "auth",
                table: "asp_net_user_tokens");

            migrationBuilder.DropForeignKey(
                name: "auth_reg_logs__user_id__asp_net_users__fkey",
                schema: "logs",
                table: "auth_reg_logs");

            migrationBuilder.DropForeignKey(
                name: "equipments__user_id__asp_net_users__fkey",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropForeignKey(
                name: "heroes__user_id__asp_net_users__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "user_bans__user_id__asp_net_users__fkey",
                schema: "users",
                table: "user_bans");

            migrationBuilder.DropPrimaryKey(
                name: "asp_net_users__pkey",
                schema: "auth",
                table: "asp_net_users");

            migrationBuilder.DropPrimaryKey(
                name: "asp_net_user_tokens__pkey",
                schema: "auth",
                table: "asp_net_user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "asp_net_user_roles__pkey",
                schema: "auth",
                table: "asp_net_user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "asp_net_user_logins__pkey",
                schema: "auth",
                table: "asp_net_user_logins");

            migrationBuilder.DropPrimaryKey(
                name: "asp_net_user_claims__pkey",
                schema: "auth",
                table: "asp_net_user_claims");

            migrationBuilder.DropPrimaryKey(
                name: "asp_net_roles__pkey",
                schema: "auth",
                table: "asp_net_roles");

            migrationBuilder.DropPrimaryKey(
                name: "asp_net_role_claims__pkey",
                schema: "auth",
                table: "asp_net_role_claims");

            migrationBuilder.RenameTable(
                name: "asp_net_users",
                schema: "auth",
                newName: "identity_users",
                newSchema: "users");

            migrationBuilder.RenameTable(
                name: "asp_net_user_tokens",
                schema: "auth",
                newName: "identity_user_tokens",
                newSchema: "users");

            migrationBuilder.RenameTable(
                name: "asp_net_user_roles",
                schema: "auth",
                newName: "identity_user_roles",
                newSchema: "users");

            migrationBuilder.RenameTable(
                name: "asp_net_user_logins",
                schema: "auth",
                newName: "identity_user_logins",
                newSchema: "users");

            migrationBuilder.RenameTable(
                name: "asp_net_user_claims",
                schema: "auth",
                newName: "identity_user_claims",
                newSchema: "users");

            migrationBuilder.RenameTable(
                name: "asp_net_roles",
                schema: "auth",
                newName: "identity_roles",
                newSchema: "users");

            migrationBuilder.RenameTable(
                name: "asp_net_role_claims",
                schema: "auth",
                newName: "identity_role_claims",
                newSchema: "users");

            migrationBuilder.RenameIndex(
                name: "asp_net_users__normalized_user_name__idx",
                schema: "users",
                table: "identity_users",
                newName: "identity_users__normalized_user_name__idx");

            migrationBuilder.RenameIndex(
                name: "asp_net_users__normalized_email__idx",
                schema: "users",
                table: "identity_users",
                newName: "identity_users__normalized_email__idx");

            migrationBuilder.RenameIndex(
                name: "asp_net_user_roles__role_id__idx",
                schema: "users",
                table: "identity_user_roles",
                newName: "identity_user_roles__role_id__idx");

            migrationBuilder.RenameIndex(
                name: "asp_net_user_logins__user_id__idx",
                schema: "users",
                table: "identity_user_logins",
                newName: "identity_user_logins__user_id__idx");

            migrationBuilder.RenameIndex(
                name: "asp_net_user_claims__user_id__idx",
                schema: "users",
                table: "identity_user_claims",
                newName: "identity_user_claims__user_id__idx");

            migrationBuilder.RenameIndex(
                name: "asp_net_roles__normalized_name__idx",
                schema: "users",
                table: "identity_roles",
                newName: "identity_roles__normalized_name__idx");

            migrationBuilder.RenameIndex(
                name: "asp_net_role_claims__role_id__idx",
                schema: "users",
                table: "identity_role_claims",
                newName: "identity_role_claims__role_id__idx");

            migrationBuilder.AddPrimaryKey(
                name: "identity_users__pkey",
                schema: "users",
                table: "identity_users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "identity_user_tokens__pkey",
                schema: "users",
                table: "identity_user_tokens",
                columns: new[] { "user_id", "login_provider", "name" });

            migrationBuilder.AddPrimaryKey(
                name: "identity_user_roles__pkey",
                schema: "users",
                table: "identity_user_roles",
                columns: new[] { "user_id", "role_id" });

            migrationBuilder.AddPrimaryKey(
                name: "identity_user_logins__pkey",
                schema: "users",
                table: "identity_user_logins",
                columns: new[] { "login_provider", "provider_key" });

            migrationBuilder.AddPrimaryKey(
                name: "identity_user_claims__pkey",
                schema: "users",
                table: "identity_user_claims",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "identity_roles__pkey",
                schema: "users",
                table: "identity_roles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "identity_role_claims__pkey",
                schema: "users",
                table: "identity_role_claims",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "auth_reg_logs__user_id__identity_users__fkey",
                schema: "logs",
                table: "auth_reg_logs",
                column: "user_id",
                principalSchema: "users",
                principalTable: "identity_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "equipments__user_id__identity_users__fkey",
                schema: "collection",
                table: "equipments",
                column: "user_id",
                principalSchema: "users",
                principalTable: "identity_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "heroes__user_id__identity_users__fkey",
                schema: "collection",
                table: "heroes",
                column: "user_id",
                principalSchema: "users",
                principalTable: "identity_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "identity_role_claims__role_id__identity_roles__fkey",
                schema: "users",
                table: "identity_role_claims",
                column: "role_id",
                principalSchema: "users",
                principalTable: "identity_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "identity_user_claims__user_id__identity_users__fkey",
                schema: "users",
                table: "identity_user_claims",
                column: "user_id",
                principalSchema: "users",
                principalTable: "identity_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "identity_user_logins__user_id__identity_users__fkey",
                schema: "users",
                table: "identity_user_logins",
                column: "user_id",
                principalSchema: "users",
                principalTable: "identity_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "identity_user_roles__role_id__identity_roles__fkey",
                schema: "users",
                table: "identity_user_roles",
                column: "role_id",
                principalSchema: "users",
                principalTable: "identity_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "identity_user_roles__user_id__identity_users__fkey",
                schema: "users",
                table: "identity_user_roles",
                column: "user_id",
                principalSchema: "users",
                principalTable: "identity_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "identity_user_tokens__user_id__identity_users__fkey",
                schema: "users",
                table: "identity_user_tokens",
                column: "user_id",
                principalSchema: "users",
                principalTable: "identity_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "user_bans__user_id__identity_users__fkey",
                schema: "users",
                table: "user_bans",
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
                name: "auth_reg_logs__user_id__identity_users__fkey",
                schema: "logs",
                table: "auth_reg_logs");

            migrationBuilder.DropForeignKey(
                name: "equipments__user_id__identity_users__fkey",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropForeignKey(
                name: "heroes__user_id__identity_users__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "identity_role_claims__role_id__identity_roles__fkey",
                schema: "users",
                table: "identity_role_claims");

            migrationBuilder.DropForeignKey(
                name: "identity_user_claims__user_id__identity_users__fkey",
                schema: "users",
                table: "identity_user_claims");

            migrationBuilder.DropForeignKey(
                name: "identity_user_logins__user_id__identity_users__fkey",
                schema: "users",
                table: "identity_user_logins");

            migrationBuilder.DropForeignKey(
                name: "identity_user_roles__role_id__identity_roles__fkey",
                schema: "users",
                table: "identity_user_roles");

            migrationBuilder.DropForeignKey(
                name: "identity_user_roles__user_id__identity_users__fkey",
                schema: "users",
                table: "identity_user_roles");

            migrationBuilder.DropForeignKey(
                name: "identity_user_tokens__user_id__identity_users__fkey",
                schema: "users",
                table: "identity_user_tokens");

            migrationBuilder.DropForeignKey(
                name: "user_bans__user_id__identity_users__fkey",
                schema: "users",
                table: "user_bans");

            migrationBuilder.DropPrimaryKey(
                name: "identity_users__pkey",
                schema: "users",
                table: "identity_users");

            migrationBuilder.DropPrimaryKey(
                name: "identity_user_tokens__pkey",
                schema: "users",
                table: "identity_user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "identity_user_roles__pkey",
                schema: "users",
                table: "identity_user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "identity_user_logins__pkey",
                schema: "users",
                table: "identity_user_logins");

            migrationBuilder.DropPrimaryKey(
                name: "identity_user_claims__pkey",
                schema: "users",
                table: "identity_user_claims");

            migrationBuilder.DropPrimaryKey(
                name: "identity_roles__pkey",
                schema: "users",
                table: "identity_roles");

            migrationBuilder.DropPrimaryKey(
                name: "identity_role_claims__pkey",
                schema: "users",
                table: "identity_role_claims");

            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.RenameTable(
                name: "identity_users",
                schema: "users",
                newName: "asp_net_users",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "identity_user_tokens",
                schema: "users",
                newName: "asp_net_user_tokens",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "identity_user_roles",
                schema: "users",
                newName: "asp_net_user_roles",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "identity_user_logins",
                schema: "users",
                newName: "asp_net_user_logins",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "identity_user_claims",
                schema: "users",
                newName: "asp_net_user_claims",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "identity_roles",
                schema: "users",
                newName: "asp_net_roles",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "identity_role_claims",
                schema: "users",
                newName: "asp_net_role_claims",
                newSchema: "auth");

            migrationBuilder.RenameIndex(
                name: "identity_users__normalized_user_name__idx",
                schema: "auth",
                table: "asp_net_users",
                newName: "asp_net_users__normalized_user_name__idx");

            migrationBuilder.RenameIndex(
                name: "identity_users__normalized_email__idx",
                schema: "auth",
                table: "asp_net_users",
                newName: "asp_net_users__normalized_email__idx");

            migrationBuilder.RenameIndex(
                name: "identity_user_roles__role_id__idx",
                schema: "auth",
                table: "asp_net_user_roles",
                newName: "asp_net_user_roles__role_id__idx");

            migrationBuilder.RenameIndex(
                name: "identity_user_logins__user_id__idx",
                schema: "auth",
                table: "asp_net_user_logins",
                newName: "asp_net_user_logins__user_id__idx");

            migrationBuilder.RenameIndex(
                name: "identity_user_claims__user_id__idx",
                schema: "auth",
                table: "asp_net_user_claims",
                newName: "asp_net_user_claims__user_id__idx");

            migrationBuilder.RenameIndex(
                name: "identity_roles__normalized_name__idx",
                schema: "auth",
                table: "asp_net_roles",
                newName: "asp_net_roles__normalized_name__idx");

            migrationBuilder.RenameIndex(
                name: "identity_role_claims__role_id__idx",
                schema: "auth",
                table: "asp_net_role_claims",
                newName: "asp_net_role_claims__role_id__idx");

            migrationBuilder.AddPrimaryKey(
                name: "asp_net_users__pkey",
                schema: "auth",
                table: "asp_net_users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "asp_net_user_tokens__pkey",
                schema: "auth",
                table: "asp_net_user_tokens",
                columns: new[] { "user_id", "login_provider", "name" });

            migrationBuilder.AddPrimaryKey(
                name: "asp_net_user_roles__pkey",
                schema: "auth",
                table: "asp_net_user_roles",
                columns: new[] { "user_id", "role_id" });

            migrationBuilder.AddPrimaryKey(
                name: "asp_net_user_logins__pkey",
                schema: "auth",
                table: "asp_net_user_logins",
                columns: new[] { "login_provider", "provider_key" });

            migrationBuilder.AddPrimaryKey(
                name: "asp_net_user_claims__pkey",
                schema: "auth",
                table: "asp_net_user_claims",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "asp_net_roles__pkey",
                schema: "auth",
                table: "asp_net_roles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "asp_net_role_claims__pkey",
                schema: "auth",
                table: "asp_net_role_claims",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "asp_net_role_claims__role_id__asp_net_roles__fkey",
                schema: "auth",
                table: "asp_net_role_claims",
                column: "role_id",
                principalSchema: "auth",
                principalTable: "asp_net_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "asp_net_user_claims__user_id__asp_net_users__fkey",
                schema: "auth",
                table: "asp_net_user_claims",
                column: "user_id",
                principalSchema: "auth",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "asp_net_user_logins__user_id__asp_net_users__fkey",
                schema: "auth",
                table: "asp_net_user_logins",
                column: "user_id",
                principalSchema: "auth",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "asp_net_user_roles__role_id__asp_net_roles__fkey",
                schema: "auth",
                table: "asp_net_user_roles",
                column: "role_id",
                principalSchema: "auth",
                principalTable: "asp_net_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "asp_net_user_roles__user_id__asp_net_users__fkey",
                schema: "auth",
                table: "asp_net_user_roles",
                column: "user_id",
                principalSchema: "auth",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "asp_net_user_tokens__user_id__asp_net_users__fkey",
                schema: "auth",
                table: "asp_net_user_tokens",
                column: "user_id",
                principalSchema: "auth",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "auth_reg_logs__user_id__asp_net_users__fkey",
                schema: "logs",
                table: "auth_reg_logs",
                column: "user_id",
                principalSchema: "auth",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "equipments__user_id__asp_net_users__fkey",
                schema: "collection",
                table: "equipments",
                column: "user_id",
                principalSchema: "auth",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "heroes__user_id__asp_net_users__fkey",
                schema: "collection",
                table: "heroes",
                column: "user_id",
                principalSchema: "auth",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "user_bans__user_id__asp_net_users__fkey",
                schema: "users",
                table: "user_bans",
                column: "user_id",
                principalSchema: "auth",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
