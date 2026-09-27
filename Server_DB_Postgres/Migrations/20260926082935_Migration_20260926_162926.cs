using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server_DB_Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20260926_162926 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "version",
                schema: "users",
                table: "user_sessions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 1L);

            migrationBuilder.AlterColumn<long>(
                name: "version",
                schema: "users",
                table: "user_bans",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 1L);

            migrationBuilder.AlterColumn<long>(
                name: "version",
                schema: "users",
                table: "user_accesskeys",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 1L);

            migrationBuilder.AlterColumn<long>(
                name: "version",
                schema: "logs",
                table: "registration_logs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 1L);

            migrationBuilder.AlterColumn<long>(
                name: "version",
                schema: "users",
                table: "identity_users",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 1L);

            migrationBuilder.AlterColumn<long>(
                name: "version",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 1L);

            migrationBuilder.AlterColumn<long>(
                name: "version",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 1L);

            migrationBuilder.AlterColumn<long>(
                name: "version",
                schema: "collection",
                table: "drop_rates",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 1L);

            migrationBuilder.AlterColumn<long>(
                name: "version",
                schema: "logs",
                table: "authentication_logs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 1L);

            migrationBuilder.CreateTable(
                name: "abilities",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    cost = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("abilities__pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "x_base_heroes_abilities",
                schema: "game_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    base_hero_id = table.Column<int>(type: "integer", nullable: false),
                    ability_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("x_base_heroes_abilities__pkey", x => x.id);
                    table.ForeignKey(
                        name: "x_base_heroes_abilities__ability_id__abilities__fkey",
                        column: x => x.ability_id,
                        principalSchema: "game_data",
                        principalTable: "abilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "x_base_heroes_abilities__base_hero_id__base_heroes__fkey",
                        column: x => x.base_hero_id,
                        principalSchema: "game_data",
                        principalTable: "base_heroes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "abilities__name__idx",
                schema: "game_data",
                table: "abilities",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "x_base_heroes_abilities__ability_id__idx",
                schema: "game_data",
                table: "x_base_heroes_abilities",
                column: "ability_id");

            migrationBuilder.CreateIndex(
                name: "x_base_heroes_abilities__base_hero_id__idx",
                schema: "game_data",
                table: "x_base_heroes_abilities",
                column: "base_hero_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "x_base_heroes_abilities",
                schema: "game_data");

            migrationBuilder.DropTable(
                name: "abilities",
                schema: "game_data");

            migrationBuilder.AlterColumn<long>(
                name: "version",
                schema: "users",
                table: "user_sessions",
                type: "bigint",
                nullable: false,
                defaultValue: 1L,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "version",
                schema: "users",
                table: "user_bans",
                type: "bigint",
                nullable: false,
                defaultValue: 1L,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "version",
                schema: "users",
                table: "user_accesskeys",
                type: "bigint",
                nullable: false,
                defaultValue: 1L,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "version",
                schema: "logs",
                table: "registration_logs",
                type: "bigint",
                nullable: false,
                defaultValue: 1L,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "version",
                schema: "users",
                table: "identity_users",
                type: "bigint",
                nullable: false,
                defaultValue: 1L,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "version",
                schema: "collection",
                table: "heroes",
                type: "bigint",
                nullable: false,
                defaultValue: 1L,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "version",
                schema: "collection",
                table: "equipments",
                type: "bigint",
                nullable: false,
                defaultValue: 1L,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "version",
                schema: "collection",
                table: "drop_rates",
                type: "bigint",
                nullable: false,
                defaultValue: 1L,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "version",
                schema: "logs",
                table: "authentication_logs",
                type: "bigint",
                nullable: false,
                defaultValue: 1L,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
