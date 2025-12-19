using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Fix5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "user_devices__device_unique_identifier__idx",
                schema: "users",
                table: "user_devices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "user_devices__device_unique_identifier__idx",
                schema: "users",
                table: "user_devices",
                column: "device_unique_identifier",
                unique: true);
        }
    }
}
