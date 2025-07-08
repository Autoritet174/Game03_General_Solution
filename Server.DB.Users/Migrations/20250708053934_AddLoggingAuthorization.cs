using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.DB.Users.Migrations;

/// <inheritdoc />
public partial class AddLoggingAuthorization : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {

        _ = migrationBuilder.Sql("""
            CREATE TABLE users_authorization_log (
                id UUID NOT NULL PRIMARY KEY,
                user_id UUID NOT NULL,
                created_at TIMESTAMPTZ DEFAULT NOW(),
                ip_address TEXT,
                user_agent TEXT
            );

            CREATE OR REPLACE FUNCTION log_user_auth(
                p_user_id UUID,
                p_ip TEXT,
                p_ua TEXT
            )
            RETURNS void AS $$
            BEGIN
                INSERT INTO user_auth_logs(user_id, ip_address, user_agent)
                VALUES (p_user_id, p_ip, p_ua);
            END;
            $$ LANGUAGE plpgsql;
            
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {

        _ = migrationBuilder.Sql("""
            DROP TRIGGER IF EXISTS set_timestamp_users ON users;
            """);
        _ = migrationBuilder.DropTable(name: "users_authorization_log");
    }
}
