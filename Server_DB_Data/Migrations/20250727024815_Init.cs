using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "heroes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users_id", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "name",
                table: "heroes",
                column: "name",
                unique: true);

            _ = migrationBuilder.Sql("""
            CREATE OR REPLACE FUNCTION set_timestamp()
            RETURNS TRIGGER AS $$
            BEGIN
                IF TG_OP = 'INSERT' THEN
                    NEW.created_at := COALESCE(NEW.created_at, NOW());
                    NEW.updated_at := COALESCE(NEW.updated_at, NOW());
                ELSIF TG_OP = 'UPDATE' THEN
                    NEW.created_at := OLD.created_at;
                    IF NEW.updated_at IS NULL OR NEW.updated_at = OLD.updated_at THEN
                        NEW.updated_at := NOW();
                    END IF;
                END IF;
                RETURN NEW;
            END;
            $$ LANGUAGE plpgsql;
            """);

            _ = migrationBuilder.Sql("""
            CREATE TRIGGER set_timestamp_heroes
            BEFORE INSERT OR UPDATE ON heroes
            FOR EACH ROW
            EXECUTE FUNCTION set_timestamp();
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.Sql("""
            DROP TRIGGER IF EXISTS set_timestamp_users ON users;
            """);

            _ = migrationBuilder.Sql("""
            DROP FUNCTION IF EXISTS set_timestamp();
            """);

            migrationBuilder.DropTable(
                name: "heroes");
        }
    }
}
