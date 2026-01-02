using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class IdentityFix4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_authorizations",
                schema: "logs");

            migrationBuilder.CreateTable(
                name: "auth_reg_logs",
                schema: "logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    application_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    success = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_device_id = table.Column<Guid>(type: "uuid", nullable: true),
                    ip = table.Column<IPAddress>(type: "inet", nullable: true),
                    action_is_authentication = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("auth_reg_logs__pkey", x => x.id);
                    table.ForeignKey(
                        name: "auth_reg_logs__application_user_id__asp_net_users__fkey",
                        column: x => x.application_user_id,
                        principalSchema: "auth",
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "auth_reg_logs__user_device_id__user_devices__fkey",
                        column: x => x.user_device_id,
                        principalSchema: "users",
                        principalTable: "user_devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "auth_reg_logs__application_user_id__idx",
                schema: "logs",
                table: "auth_reg_logs",
                column: "application_user_id");

            migrationBuilder.CreateIndex(
                name: "auth_reg_logs__user_device_id__idx",
                schema: "logs",
                table: "auth_reg_logs",
                column: "user_device_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auth_reg_logs",
                schema: "logs");

            migrationBuilder.CreateTable(
                name: "user_authorizations",
                schema: "logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    application_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    user_device_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ip = table.Column<IPAddress>(type: "inet", nullable: true),
                    success = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_authorizations__pkey", x => x.id);
                    table.ForeignKey(
                        name: "user_authorizations__application_user_id__asp_net_users__fkey",
                        column: x => x.application_user_id,
                        principalSchema: "auth",
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "user_authorizations__user_device_id__user_devices__fkey",
                        column: x => x.user_device_id,
                        principalSchema: "users",
                        principalTable: "user_devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "user_authorizations__application_user_id__idx",
                schema: "logs",
                table: "user_authorizations",
                column: "application_user_id");

            migrationBuilder.CreateIndex(
                name: "user_authorizations__user_device_id__idx",
                schema: "logs",
                table: "user_authorizations",
                column: "user_device_id");
        }
    }
}
