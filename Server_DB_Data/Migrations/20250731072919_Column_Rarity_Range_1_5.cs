using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class Column_Rarity_Range_1_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE main.heroes 
                ADD CONSTRAINT ck_hero_rarity_range 
                CHECK (rarity BETWEEN 1 AND 5)
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE main.heroes 
                DROP CONSTRAINT IF EXISTS ck_hero_rarity_range
                """);
        }
    }
}
