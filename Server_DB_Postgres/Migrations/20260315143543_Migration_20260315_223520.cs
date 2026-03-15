using System;
using System.Collections.Generic;
using General.DTO;
using Microsoft.EntityFrameworkCore.Migrations;
using Server_DB_Postgres.Migrations;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260315_223520 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Dictionary<Guid, ItemExp>>(
                name: "experience_heroes",
                schema: "collection",
                table: "equipments",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(Dictionary<Guid, ItemExp>),
                oldType: "jsonb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Dictionary<Guid, ItemExp>>(
                name: "experience_heroes",
                schema: "collection",
                table: "equipments",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(Dictionary<Guid, ItemExp>),
                oldType: "jsonb",
                oldNullable: true);
        }
    }
}
