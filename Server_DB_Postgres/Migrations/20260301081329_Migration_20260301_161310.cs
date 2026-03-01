using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260301_161310 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "drop_rates__user_id__type__idx",
                schema: "collection",
                table: "drop_rates");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "collection",
                table: "drop_rates");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "collection",
                table: "drop_rates");

            migrationBuilder.CreateIndex(
                name: "drop_rates__user_id__idx",
                schema: "collection",
                table: "drop_rates",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "drop_rates__user_id__idx",
                schema: "collection",
                table: "drop_rates");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                schema: "collection",
                table: "drop_rates",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                schema: "collection",
                table: "drop_rates",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "drop_rates__user_id__type__idx",
                schema: "collection",
                table: "drop_rates",
                columns: new[] { "user_id", "type" });
        }
    }
}
