using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class IdentityFix5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "auth_reg_logs__application_user_id__asp_net_users__fkey",
                schema: "logs",
                table: "auth_reg_logs");

            migrationBuilder.DropForeignKey(
                name: "equipments__user_id__users__fkey",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropForeignKey(
                name: "heroes__user_id__users__fkey",
                schema: "collection",
                table: "heroes");

            migrationBuilder.DropForeignKey(
                name: "user_bans__application_user_id__asp_net_users__fkey",
                schema: "users",
                table: "user_bans");

            migrationBuilder.DropForeignKey(
                name: "user_bans__user_id__users__fkey",
                schema: "users",
                table: "user_bans");

            migrationBuilder.DropTable(
                name: "users",
                schema: "users");

            migrationBuilder.DropIndex(
                name: "user_bans__application_user_id__idx",
                schema: "users",
                table: "user_bans");

            migrationBuilder.DropColumn(
                name: "application_user_id",
                schema: "users",
                table: "user_bans");

            migrationBuilder.RenameColumn(
                name: "application_user_id",
                schema: "logs",
                table: "auth_reg_logs",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "auth_reg_logs__application_user_id__idx",
                schema: "logs",
                table: "auth_reg_logs",
                newName: "auth_reg_logs__user_id__idx");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                schema: "users",
                table: "user_bans",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "logs",
                table: "auth_reg_logs",
                newName: "application_user_id");

            migrationBuilder.RenameIndex(
                name: "auth_reg_logs__user_id__idx",
                schema: "logs",
                table: "auth_reg_logs",
                newName: "auth_reg_logs__application_user_id__idx");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                schema: "users",
                table: "user_bans",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "application_user_id",
                schema: "users",
                table: "user_bans",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "users",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_verified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_admin = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    password_hash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    time_zone = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users__pkey", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "user_bans__application_user_id__idx",
                schema: "users",
                table: "user_bans",
                column: "application_user_id");

            migrationBuilder.CreateIndex(
                name: "users__email__idx",
                schema: "users",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "auth_reg_logs__application_user_id__asp_net_users__fkey",
                schema: "logs",
                table: "auth_reg_logs",
                column: "application_user_id",
                principalSchema: "auth",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "equipments__user_id__users__fkey",
                schema: "collection",
                table: "equipments",
                column: "user_id",
                principalSchema: "users",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "heroes__user_id__users__fkey",
                schema: "collection",
                table: "heroes",
                column: "user_id",
                principalSchema: "users",
                principalTable: "users",
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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "user_bans__user_id__users__fkey",
                schema: "users",
                table: "user_bans",
                column: "user_id",
                principalSchema: "users",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
