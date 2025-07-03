using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.DB.Users.Migrations;

/// <inheritdoc />
public partial class CreateTrigger_SetTimestamp : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION set_timestamp()
                RETURNS TRIGGER AS $$
                BEGIN
                    IF TG_OP = 'INSERT' THEN
                        NEW.created_at := NOW();
                        NEW.updated_at := NOW();
                    ELSIF TG_OP = 'UPDATE' THEN
                        NEW.created_at := OLD.created_at;
                        NEW.updated_at := NOW();
                    END IF;
                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;
            ");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS set_timestamp();
            ");
    }
}
