using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260301_154558 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "equipments__user_id__idx",
                schema: "collection",
                table: "equipments");

            migrationBuilder.CreateTable(
                name: "drop_rates",
                schema: "collection",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    version = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    counts = table.Column<int[]>(type: "integer[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("drop_rates__pkey", x => x.id);
                    table.ForeignKey(
                        name: "drop_rates__user_id__identity_users__fkey",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "equipments__hero_id__idx",
                schema: "collection",
                table: "equipments",
                column: "hero_id");

            migrationBuilder.CreateIndex(
                name: "equipments__user_id__idx",
                schema: "collection",
                table: "equipments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "drop_rates__user_id__type__idx",
                schema: "collection",
                table: "drop_rates",
                columns: new[] { "user_id", "type" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "drop_rates",
                schema: "collection");

            migrationBuilder.DropIndex(
                name: "equipments__hero_id__idx",
                schema: "collection",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "equipments__user_id__idx",
                schema: "collection",
                table: "equipments");

            migrationBuilder.CreateIndex(
                name: "equipments__user_id__idx",
                schema: "collection",
                table: "equipments",
                column: "user_id",
                filter: "hero_id IS NULL");
        }
    }
}
