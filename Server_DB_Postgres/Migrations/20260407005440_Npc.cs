using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Npc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dungeons",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    level = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    difficulty = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("dungeons__pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "npcs",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    rarity = table.Column<int>(type: "integer", nullable: false),
                    rank = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    main_stat = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("npcs__pkey", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "dungeons__name__idx",
                schema: "game_data",
                table: "dungeons",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "npcs__name__idx",
                schema: "game_data",
                table: "npcs",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dungeons",
                schema: "game_data");

            migrationBuilder.DropTable(
                name: "npcs",
                schema: "game_data");
        }
    }
}
