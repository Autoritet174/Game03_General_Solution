using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server_DB_Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "__lists");

            migrationBuilder.EnsureSchema(
                name: "_heroes");

            migrationBuilder.EnsureSchema(
                name: "_equipment");

            migrationBuilder.EnsureSchema(
                name: "x_Cross");

            migrationBuilder.CreateTable(
                name: "CreatureTypes",
                schema: "__lists",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CreatureTypes_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DamageTypes",
                schema: "__lists",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    name_ru = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DevHintRu = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("DamageTypes_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Heroes",
                schema: "_heroes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    rarity = table.Column<int>(type: "integer", nullable: false),
                    is_unique = table.Column<bool>(type: "boolean", nullable: false),
                    health = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    damage = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Heroes_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "WeaponTypes",
                schema: "__lists",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("WeaponTypes_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "X_Hero_CreatureType",
                schema: "x_Cross",
                columns: table => new
                {
                    HeroId = table.Column<int>(type: "integer", nullable: false),
                    CreatureTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("X_Hero_CreatureType_pkey", x => new { x.HeroId, x.CreatureTypeId });
                    table.ForeignKey(
                        name: "X_Hero_CreatureType_creature_type_id_creature_types_fkey",
                        column: x => x.CreatureTypeId,
                        principalSchema: "__lists",
                        principalTable: "CreatureTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "X_Hero_CreatureType_hero_id_heroes_fkey",
                        column: x => x.HeroId,
                        principalSchema: "_heroes",
                        principalTable: "Heroes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                schema: "_equipment",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    rarity = table.Column<int>(type: "integer", nullable: false),
                    is_unique = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    damage = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    weapon_type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Weapons_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Weapons_weapon_type_id_weapon_types_fkey",
                        column: x => x.weapon_type_id,
                        principalSchema: "__lists",
                        principalTable: "WeaponTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "X_WeaponType_DamageType",
                schema: "x_Cross",
                columns: table => new
                {
                    WeaponTypeId = table.Column<int>(type: "integer", nullable: false),
                    DamageTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("X_WeaponType_DamageType_pkey", x => new { x.WeaponTypeId, x.DamageTypeId });
                    table.ForeignKey(
                        name: "X_WeaponType_DamageType_damage_type_id_damage_types_fkey",
                        column: x => x.DamageTypeId,
                        principalSchema: "__lists",
                        principalTable: "DamageTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "X_WeaponType_DamageType_weapon_type_id_weapon_types_fkey",
                        column: x => x.WeaponTypeId,
                        principalSchema: "__lists",
                        principalTable: "WeaponTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "__lists",
                table: "CreatureTypes",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Humanoid" },
                    { 2, "Orc" },
                    { 3, "Goblin" },
                    { 4, "Zombie" },
                    { 5, "Vampire" },
                    { 6, "Skeleton" },
                    { 7, "Bear" },
                    { 8, "Wolf" }
                });

            migrationBuilder.InsertData(
                schema: "__lists",
                table: "DamageTypes",
                columns: new[] { "id", "DevHintRu", "name", "name_ru" },
                values: new object[,]
                {
                    { 1, "Копьё Пика Рапира Шпага Трезубец Лук Арбалет Дротики Меч(колющий)", "Piercing", "Колющий" },
                    { 2, "Булава Палица Боевой молот Моргенштерн ", "Blunt", "Дробящий" },
                    { 3, "Сабля Ятаган Катана Скимитар Боевой веер Коса Бумеранг Чакрам Меч(режущий)", "Cutting", "Режущий" },
                    { 4, "Топор Секира Бердыш Алебарда", "Chopping", "Рубящий" },
                    { 5, "\"Проявления: Волшебные стрелы (Magic Missile), телекинетические удары, невидимые клинки силы.\r\n\r\nУязвимы: Призраки, астральные существа (иногда).\r\n\r\nУстойчивы/Иммунны: Практически нет, это магическая энергия.\r\n\r\nОсобенность: Редко имеет сопротивление. Наносит чистый, не-элементальный магический урон. Часто не может быть отражен или заблокирован обычным щитом, только магическими средствами. Может отбрасывать цели.\"", "Force", "Силовой" },
                    { 6, "\"Проявления: Огненные шары, пламя дракона, горение.\r\n\r\nУязвимы: Духи льда, растения, нежить (часто), насекомые, ледяные элементали.\r\n\r\nУстойчивы/Иммунны: Демоны, огненные элементали, лавовые големы, красные драконы.\r\n\r\nОсобенность: Часто накладывает эффект горения (DoT), может разрушать ледяные преграды, поджигать местность. Противоположен Ледяному.\r\n\r\n\"", "Fire", "Огненный" },
                    { 7, "\"Проявления: Ледяные стрелы, стужа, обморожение, ледяная тюрьма.\r\n\r\nУязвимы: Огненные существа, драконы, рептилии, лавовые големы.\r\n\r\nУстойчивы/Иммунны: Ледяные элементали, белые драконы, арктические существа.\r\n\r\nОсобенность: Часто накладывает эффект замедления или оковы (обездвиживания). Может создавать ледяной наст на воде. Противоположен Огненному.\"", "Frost", "Ледяной" },
                    { 8, "\"Проявления: Молнии, электрические разряды, шок, громовые волны.\r\n\r\nУязвимы: Механизмы (роботы, големы), существа в металлической броне, водные/слизневые существа (проводят ток).\r\n\r\nУстойчивы/Иммунны: Воздушные элементали, существа из изоляционных материалов (камень, дерево — не всегда).\r\n\r\nОсобенность: Высокий шанс наложить эффект стана (паралич, оглушение). Урон может цепляться на несколько ближайших целей. Часто игнорирует часть брони.\"", "Electric", "Электрический" },
                    { 9, "\"Проявления: Ядовитые облака, укусы, отравленные клинки, токсичные выбросы.\r\n\r\nУязвимы: Органические, живые существа (люди, звери, растения).\r\n\r\nУстойчивы/Иммунны: Нежить, конструкты, каменные/земляные элементали, ядовитые сами по себе монстры (гигантские пауки, скорпионы).\r\n\r\nОсобенность:* Практически всегда наносит урон по времени (Damage over Time - DoT). Может накладывать ослабляющие эффекты (снижение характеристик, замедление регенерации).\"", "Poison", "Ядовитый" },
                    { 10, "\"Проявления: Разрушительный рёв, ударная звуковая волна, резонанс, разрыв барабанных перепонок, крик банши, звуковые пушки.\r\n\r\nУязвимы: Стеклянные/хрустальные существа, конструкции с хрупкими элементами, существа с тонким слухом (летучие мыши, эльфы). Механизмы (может нарушить работу).\r\n\r\nУстойчивы/Иммунны: Глухие существа, каменные/земляные големы (плохо проводят звук), существа из вакуума/космоса, призраки (иногда).\r\n\r\nОсобенность: Часто игнорирует физическую броню (звук проходит сквозь пластины) и магическую защиту. Имеет высокий шанс наложить дебаффы: оглушение, дезориентация, снижение точности, развеивание иллюзий/невидимости. Может активировать хрупкие механизмы или разрушать звуковые барьеры. В тихих локациях (подземелья) может привлекать других врагов.\"", "Sonic", "Звуковой" },
                    { 11, "\"Проявления: Божественный гнев, очищающий свет, заклинания жрецов и паладинов, освящённое оружие.\r\n\r\nУязвимы: Нежить (зомби, скелеты, призраки, вампиры), демоны/исчадия, существа Тьмы и Хаоса.\r\n\r\nУстойчивы/Иммунны: Ангелы, святые существа, некоторые нейтральные духи природы.\r\n\r\nОсобенность: Часто прерывает концентрацию магов, изгоняет/отпугивает нежить, наносит увеличенный/критический урон к уязвимым типам. Может накладывать ослепление.\"", "Light", "Световой" },
                    { 12, "\"Проявления: Энергия смерти, вампиризм (кража HP), проклятия, теневая магия.\r\n\r\nУязвимы: Живые существа (подрывает жизненную силу), святые существа (иногда).\r\n\r\nУстойчивы/Иммунны: Нежить (часто её лечит или не вредит), демоны Тени, существа Смерти.\r\n\r\nОсобенность: Может накладывать эффекты снижения максимального запаса здоровья, запрета на лечение (анти-хил), страха или ослабления. Противоположен Священному.\"", "Necrotic", "Некротический" },
                    { 13, "\"Проявления: Взрыв разума, телепатическое копье, ментальные тиски, навязывание невыносимых мыслей, иллюзии, причиняющие реальную боль, атаки по сновидениям.\r\n\r\nУязвимы: Все разумные существа (люди, эльфы, орки, драконы). Особенно те, кто обладает высоким интеллектом, но слабой силой воли.\r\n\r\nУстойчивы/Иммунны: Конструкты, големы, неразумная нежить (зомби, скелеты), животные/звери с примитивным интеллектом, берсеркеры/дикари (их разум хаотичен и защищен яростью), роботы, некоторые демоны пустоты.\r\n\r\nОсобенность: Почти всегда игнорирует физическую броню и магическую защиту (зависит от сопротивления ментальным эффектам/силе воли). Реже наносит чистый урон, чаще накладывает мощнейшие контроль-эффекты: страх, очарование, сон, паралич, безумие, взятие под контроль. Может снижать характеристики (Интеллект, Мудрость, Харизму). Не оставляет физических ран.\"", "Telepathic", "Телепатический" },
                    { 14, "\"Проявления: Плевки кислотой, едкие пары, растворяющие жидкости.\r\n\r\nУязвимы: Существа с хитиновым/металлическим панцирем, механизмы, бронированные цели.\r\n\r\nУстойчивы/Иммунны: Слизни, кислотные элементали, аморфные существа.\r\n\r\nОсобенность: Часто снижает защиту (броню) цели на время или игнорирует её часть. Может наносить урон предметам/снаряжению.\"", "Acid", "Кислотный" },
                    { 15, "\"Проявления: Нестабильная энергия хаоса, смесь стихий, дикая магия.\r\n\r\nУязвимы: Существа Закона и Порядка (дэвы, механикусы).\r\n\r\nУстойчивы/Иммунны: Демоны Бездны, хаотические элементали.\r\n\r\nОсобенность: Часто игнорирует фиксированный процент всех сопротивлений (например, 50% урона проходит всегда) или имеет шанс наложить случайный негативный эффект. Непредсказуем.\"", "Chaos", "Хаотический" },
                    { 16, "\"Проявления: Божественная кара, нарушение законов реальности, урон из самого \"\"кода\"\" вселенной, абсолютное расщепление материи, хирургически точное уничтожение без эффектов.\r\n\r\nУязвимы: ВСЕ. По определению.\r\n\r\nУстойчивы/Иммунны: НИКТО. По определению. (Хотя могут быть исключения в виде уникальных божественных артефактов или \"сюжетная неуязвимость\").\r\n\r\nОсобенность: Игнорирует ВСЕ виды защиты, сопротивления, иммунитеты, поглощения и снижения урона. Наносит ровно то количество урона, которое указано. Это последний аргумент в игровом балансе. Крайне редок, обычно доступен через:\r\n\r\nУльтимативные способности с долгим откатом.\r\n\r\nЛегендарные/божественные артефакты.\r\n\r\nСпособности, срабатывающие при выполнении сложных условий (например, при HP противника ниже 5%).\r\n\r\nКак механика для определенных типов врагов (например, урон от падения в бездну или \"\"неизбежная\"\" атака босса).\"", "Pure", "Чистый" }
                });

            migrationBuilder.InsertData(
                schema: "_heroes",
                table: "Heroes",
                columns: new[] { "id", "damage", "health", "is_unique", "name", "rarity" },
                values: new object[,]
                {
                    { 1, "4d21_44", "16d24_200", false, "Warrior", 1 },
                    { 2, "5d21_55", "10d28_145", false, "Huntress", 1 },
                    { 3, "3d25_39", "11d39_220", false, "Hammerman", 1 },
                    { 4, "4d23_48", "15d21_165", false, "Rogue", 1 },
                    { 5, "4d21_44", "16d58_472", false, "Battle orc", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "CreatureTypes_name_idx",
                schema: "__lists",
                table: "CreatureTypes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "DamageTypes_name_idx",
                schema: "__lists",
                table: "DamageTypes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Heroes_name_idx",
                schema: "_heroes",
                table: "Heroes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Weapons_name_idx",
                schema: "_equipment",
                table: "Weapons",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Weapons_weapon_type_id_idx",
                schema: "_equipment",
                table: "Weapons",
                column: "weapon_type_id");

            migrationBuilder.CreateIndex(
                name: "WeaponTypes_name_idx",
                schema: "__lists",
                table: "WeaponTypes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "X_Hero_CreatureType_CreatureTypeId_idx",
                schema: "x_Cross",
                table: "X_Hero_CreatureType",
                column: "CreatureTypeId");

            migrationBuilder.CreateIndex(
                name: "X_Hero_CreatureType_HeroId_idx",
                schema: "x_Cross",
                table: "X_Hero_CreatureType",
                column: "HeroId");

            migrationBuilder.CreateIndex(
                name: "X_WeaponType_DamageType_DamageTypeId_idx",
                schema: "x_Cross",
                table: "X_WeaponType_DamageType",
                column: "DamageTypeId");

            migrationBuilder.CreateIndex(
                name: "X_WeaponType_DamageType_WeaponTypeId_idx",
                schema: "x_Cross",
                table: "X_WeaponType_DamageType",
                column: "WeaponTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weapons",
                schema: "_equipment");

            migrationBuilder.DropTable(
                name: "X_Hero_CreatureType",
                schema: "x_Cross");

            migrationBuilder.DropTable(
                name: "X_WeaponType_DamageType",
                schema: "x_Cross");

            migrationBuilder.DropTable(
                name: "CreatureTypes",
                schema: "__lists");

            migrationBuilder.DropTable(
                name: "Heroes",
                schema: "_heroes");

            migrationBuilder.DropTable(
                name: "DamageTypes",
                schema: "__lists");

            migrationBuilder.DropTable(
                name: "WeaponTypes",
                schema: "__lists");
        }
    }
}
