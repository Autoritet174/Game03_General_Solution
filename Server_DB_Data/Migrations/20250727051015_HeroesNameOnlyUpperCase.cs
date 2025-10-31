using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations;

public partial class HeroesNameOnlyUpperCase : Migration
{

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
            CREATE OR REPLACE FUNCTION heroes_force_uppercase_name()
            RETURNS TRIGGER AS $$
            BEGIN
                NEW.name := upper(NEW.name);
                RETURN NEW;
            END;
            $$ LANGUAGE plpgsql;

            CREATE TRIGGER trg_heroes_uppercase_name
            BEFORE INSERT OR UPDATE OF name ON heroes
            FOR EACH ROW EXECUTE FUNCTION heroes_force_uppercase_name();
        ");
    }


    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
            DROP TRIGGER IF EXISTS trg_heroes_uppercase_name ON heroes;
            DROP FUNCTION IF EXISTS heroes_force_uppercase_name();
        ");
    }
}