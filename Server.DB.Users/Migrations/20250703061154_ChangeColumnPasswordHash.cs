using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.DB.Users.Migrations;

/// <inheritdoc />
public partial class ChangeColumnPasswordHash : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AlterColumn<string>(
            name: "password_hash",
            table: "users",
            type: "character varying(255)",
            maxLength: 255,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(84)",
            oldMaxLength: 84);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AlterColumn<string>(
            name: "password_hash",
            table: "users",
            type: "character varying(84)",
            maxLength: 84,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(255)",
            oldMaxLength: 255);
    }
}
