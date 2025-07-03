using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.DB.Users.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.CreateTable(
            name: "users",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                email_verified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                password_hash = table.Column<string>(type: "character varying(84)", maxLength: 84, nullable: false),
                time_zone = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("pk_users_id", x => x.id);
            });

        _ = migrationBuilder.CreateIndex(
            name: "Email",
            table: "users",
            column: "email",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
            name: "users");
    }
}
