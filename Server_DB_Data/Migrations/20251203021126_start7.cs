using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class start7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "xcross",
                table: "x_hero_creature_type");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "equipment",
                table: "sword");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "equipment",
                table: "sword");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "heroes",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "heroes",
                table: "heroes");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "_main",
                table: "creature_types");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "_main",
                table: "creature_types");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "xcross",
                table: "x_hero_creature_type",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                schema: "equipment",
                table: "sword",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                schema: "equipment",
                table: "sword",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                schema: "heroes",
                table: "heroes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                schema: "heroes",
                table: "heroes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                schema: "_main",
                table: "creature_types",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                schema: "_main",
                table: "creature_types",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");
        }
    }
}
