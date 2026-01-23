using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class UserSessionFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "token_hash",
                schema: "users",
                table: "user_sessions",
                newName: "refresh_token_hash");

            migrationBuilder.RenameIndex(
                name: "IX_user_sessions_token_hash",
                schema: "users",
                table: "user_sessions",
                newName: "user_sessions__refresh_token_hash__idx");

            migrationBuilder.AddColumn<long>(
                name: "version",
                schema: "users",
                table: "user_sessions",
                type: "bigint",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.CreateTable(
                name: "user_passkeys",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    descriptor_id = table.Column<byte[]>(type: "bytea", nullable: false),
                    public_key = table.Column<byte[]>(type: "bytea", nullable: false),
                    signature_counter = table.Column<long>(type: "bigint", nullable: false),
                    device_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_passkeys__pkey", x => x.id);
                    table.ForeignKey(
                        name: "user_passkeys__user_id__identity_users__fkey",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "user_passkeys__user_id__idx",
                schema: "users",
                table: "user_passkeys",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_passkeys",
                schema: "users");

            migrationBuilder.DropColumn(
                name: "version",
                schema: "users",
                table: "user_sessions");

            migrationBuilder.RenameColumn(
                name: "refresh_token_hash",
                schema: "users",
                table: "user_sessions",
                newName: "token_hash");

            migrationBuilder.RenameIndex(
                name: "user_sessions__refresh_token_hash__idx",
                schema: "users",
                table: "user_sessions",
                newName: "IX_user_sessions_token_hash");
        }
    }
}
