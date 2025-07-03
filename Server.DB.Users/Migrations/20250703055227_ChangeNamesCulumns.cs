using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.DB.Users.Migrations;

/// <inheritdoc />
public partial class ChangeNamesCulumns : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.RenameColumn(
            name: "DeletedAt",
            table: "users",
            newName: "deleted_at");

        _ = migrationBuilder.RenameIndex(
            name: "Email",
            table: "users",
            newName: "email");

        _ = migrationBuilder.AlterColumn<DateTime>(
            name: "deleted_at",
            table: "users",
            type: "timestamp with time zone",
            nullable: false,
            defaultValueSql: "NOW()",
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone",
            oldNullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.RenameColumn(
            name: "deleted_at",
            table: "users",
            newName: "DeletedAt");

        _ = migrationBuilder.RenameIndex(
            name: "email",
            table: "users",
            newName: "Email");

        _ = migrationBuilder.AlterColumn<DateTime>(
            name: "DeletedAt",
            table: "users",
            type: "timestamp with time zone",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone",
            oldDefaultValueSql: "NOW()");
    }
}
