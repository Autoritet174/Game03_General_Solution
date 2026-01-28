using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class LogFix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auth_reg_logs",
                schema: "logs");

            migrationBuilder.CreateTable(
                name: "authentication_logs",
                schema: "logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    success = table.Column<bool>(type: "boolean", nullable: false),
                    user_device_id = table.Column<Guid>(type: "uuid", nullable: true),
                    ip = table.Column<IPAddress>(type: "inet", nullable: true),
                    user_session_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("authentication_logs__pkey", x => x.id);
                    table.ForeignKey(
                        name: "authentication_logs__user_device_id__user_devices__fkey",
                        column: x => x.user_device_id,
                        principalSchema: "users",
                        principalTable: "user_devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "authentication_logs__user_id__identity_users__fkey",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "authentication_logs__user_session_id__user_sessions__fkey",
                        column: x => x.user_session_id,
                        principalSchema: "users",
                        principalTable: "user_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "registration_logs",
                schema: "logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    success = table.Column<bool>(type: "boolean", nullable: false),
                    user_device_id = table.Column<Guid>(type: "uuid", nullable: true),
                    ip = table.Column<IPAddress>(type: "inet", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("registration_logs__pkey", x => x.id);
                    table.ForeignKey(
                        name: "registration_logs__user_device_id__user_devices__fkey",
                        column: x => x.user_device_id,
                        principalSchema: "users",
                        principalTable: "user_devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "registration_logs__user_id__identity_users__fkey",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "authentication_logs__user_device_id__idx",
                schema: "logs",
                table: "authentication_logs",
                column: "user_device_id");

            migrationBuilder.CreateIndex(
                name: "authentication_logs__user_id__idx",
                schema: "logs",
                table: "authentication_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "authentication_logs__user_session_id__idx",
                schema: "logs",
                table: "authentication_logs",
                column: "user_session_id");

            migrationBuilder.CreateIndex(
                name: "registration_logs__user_device_id__idx",
                schema: "logs",
                table: "registration_logs",
                column: "user_device_id");

            migrationBuilder.CreateIndex(
                name: "registration_logs__user_id__idx",
                schema: "logs",
                table: "registration_logs",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "authentication_logs",
                schema: "logs");

            migrationBuilder.DropTable(
                name: "registration_logs",
                schema: "logs");

            migrationBuilder.CreateTable(
                name: "auth_reg_logs",
                schema: "logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_device_id = table.Column<Guid>(type: "uuid", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    action_is_authentication = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ip = table.Column<IPAddress>(type: "inet", nullable: true),
                    success = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("auth_reg_logs__pkey", x => x.id);
                    table.ForeignKey(
                        name: "auth_reg_logs__user_device_id__user_devices__fkey",
                        column: x => x.user_device_id,
                        principalSchema: "users",
                        principalTable: "user_devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "auth_reg_logs__user_id__identity_users__fkey",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "auth_reg_logs__user_device_id__idx",
                schema: "logs",
                table: "auth_reg_logs",
                column: "user_device_id");

            migrationBuilder.CreateIndex(
                name: "auth_reg_logs__user_id__idx",
                schema: "logs",
                table: "auth_reg_logs",
                column: "user_id");
        }
    }
}
