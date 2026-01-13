using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class UserSessionFix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "user_device_id",
                schema: "users",
                table: "user_sessions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "user_sessions__user_device_id__idx",
                schema: "users",
                table: "user_sessions",
                column: "user_device_id");

            migrationBuilder.AddForeignKey(
                name: "user_sessions__user_device_id__user_devices__fkey",
                schema: "users",
                table: "user_sessions",
                column: "user_device_id",
                principalSchema: "users",
                principalTable: "user_devices",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "user_sessions__user_device_id__user_devices__fkey",
                schema: "users",
                table: "user_sessions");

            migrationBuilder.DropIndex(
                name: "user_sessions__user_device_id__idx",
                schema: "users",
                table: "user_sessions");

            migrationBuilder.DropColumn(
                name: "user_device_id",
                schema: "users",
                table: "user_sessions");
        }
    }
}
