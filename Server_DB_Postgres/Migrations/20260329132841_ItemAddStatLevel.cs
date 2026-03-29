using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Server_DB_Postgres.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class ItemAddStatLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "experience_heroes",
                schema: "collection",
                table: "equipments");

            migrationBuilder.AddColumn<int>(
                name: "level",
                schema: "collection",
                table: "equipments",
                type: "integer",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "level",
                schema: "collection",
                table: "equipments");

            migrationBuilder.AddColumn<Dictionary<Guid, ItemExp>>(
                name: "experience_heroes",
                schema: "collection",
                table: "equipments",
                type: "jsonb",
                nullable: true);
        }
    }
}
