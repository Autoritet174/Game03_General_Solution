using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.DB.Users.Migrations;

/// <inheritdoc />
public partial class ChangeNamesCulumns2 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AlterColumn<DateTime>(
            name: "deleted_at",
            table: "users",
            type: "timestamp with time zone",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone",
            oldDefaultValueSql: "NOW()");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
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
}
