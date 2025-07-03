using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.DB.Users.Migrations;

/// <inheritdoc />
public partial class Function_SetTimestamp_Update_1 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.Sql("""
            CREATE OR REPLACE FUNCTION set_timestamp()
            RETURNS TRIGGER AS $$
            BEGIN
                IF TG_OP = 'INSERT' THEN
                    IF NEW.created_at IS NULL THEN
                        NEW.created_at := NOW();
                    END IF;

                    IF NEW.updated_at IS NULL THEN
                        NEW.updated_at := NOW();
                    END IF;

                ELSIF TG_OP = 'UPDATE' THEN
                    IF NEW.updated_at IS NULL THEN
                        NEW.updated_at := NOW();
                    END IF;
                END IF;

                RETURN NEW;
            END;
            $$ LANGUAGE plpgsql;
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS set_timestamp();");
    }
}
