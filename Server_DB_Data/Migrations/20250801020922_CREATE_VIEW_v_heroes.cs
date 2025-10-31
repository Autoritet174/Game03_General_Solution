using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class CREATE_VIEW_v_heroes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE OR REPLACE VIEW main.v_heroes AS
                SELECT hero.name,
                    hero.rarity,
                    string_agg(ct.name::text, ', '::text) AS creature_types
                FROM main.heroes hero
                    LEFT JOIN relations.hero_x_creature_type hct ON hero.id = hct.hero_id
                    LEFT JOIN main.creature_types ct ON hct.creature_type_id = ct.id
                GROUP BY hero.id;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP VIEW IF EXISTS main.v_heroes
                """);
        }
    }
}
