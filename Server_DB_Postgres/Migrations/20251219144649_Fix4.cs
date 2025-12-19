using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ip",
                schema: "users",
                table: "user_devices");

            migrationBuilder.AddColumn<IPAddress>(
                name: "ip",
                schema: "logs",
                table: "user_authorizations",
                type: "inet",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ip",
                schema: "logs",
                table: "user_authorizations");

            migrationBuilder.AddColumn<IPAddress>(
                name: "ip",
                schema: "users",
                table: "user_devices",
                type: "inet",
                nullable: true);
        }
    }
}
