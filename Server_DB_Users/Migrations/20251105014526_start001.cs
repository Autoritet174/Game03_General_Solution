using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Users.Migrations;

#pragma warning disable
/// <inheritdoc />
public partial class start001 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "is_admin",
            schema: "_main",
            table: "users",
            type: "boolean",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "is_admin",
            schema: "_main",
            table: "users");
    }
}
