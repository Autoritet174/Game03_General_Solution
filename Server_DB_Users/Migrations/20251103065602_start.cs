using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Users.Migrations;

#pragma warning disable
/// <inheritdoc />
public partial class start : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "_main");

        migrationBuilder.CreateTable(
            name: "users",
            schema: "_main",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                email_verified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                time_zone = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("users_pkey", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "users_bans",
            schema: "_main",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                user_bans_reasons_id = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("users_bans_pkey", x => x.id);
                table.ForeignKey(
                    name: "users_bans_user_id_users_fkey",
                    column: x => x.user_id,
                    principalSchema: "_main",
                    principalTable: "users",
                    principalColumn: "id");
            });

        migrationBuilder.CreateIndex(
            name: "users_email_idx",
            schema: "_main",
            table: "users",
            column: "email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "users_bans_user_id_idx",
            schema: "_main",
            table: "users_bans",
            column: "user_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "users_bans",
            schema: "_main");

        migrationBuilder.DropTable(
            name: "users",
            schema: "_main");
    }
}
