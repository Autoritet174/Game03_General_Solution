INSERT INTO game_data.creature_types (id, name) VALUES (1, 'Humanoid') ON CONFLICT DO NOTHING;
INSERT INTO game_data.creature_types (id, name) VALUES (2, 'Orc') ON CONFLICT DO NOTHING;
INSERT INTO game_data.creature_types (id, name) VALUES (3, 'Goblin') ON CONFLICT DO NOTHING;
INSERT INTO game_data.creature_types (id, name) VALUES (4, 'Zombie') ON CONFLICT DO NOTHING;
INSERT INTO game_data.creature_types (id, name) VALUES (5, 'Vampire') ON CONFLICT DO NOTHING;
INSERT INTO game_data.creature_types (id, name) VALUES (6, 'Skeleton') ON CONFLICT DO NOTHING;
INSERT INTO game_data.creature_types (id, name) VALUES (7, 'Bear') ON CONFLICT DO NOTHING;
INSERT INTO game_data.creature_types (id, name) VALUES (8, 'Wolf') ON CONFLICT DO NOTHING;




INSERT INTO game_data.damage_types (id, name, name_ru, dev_hint_ru) VALUES (1, 'Piercing', 'Колющий', 'Копьё Пика Рапира Шпага Трезубец Лук Арбалет Дротики Меч(колющий)') ON CONFLICT DO NOTHING;
INSERT INTO game_data.damage_types (id, name, name_ru, dev_hint_ru) VALUES (2, 'Blunt', 'Дробящий', 'Булава Палица Боевой молот Моргенштерн ') ON CONFLICT DO NOTHING;
INSERT INTO game_data.damage_types (id, name, name_ru, dev_hint_ru) VALUES (3, 'Cutting', 'Режущий', 'Сабля Ятаган Катана Скимитар Боевой веер Коса Бумеранг Чакрам Меч(режущий)') ON CONFLICT DO NOTHING;
INSERT INTO game_data.damage_types (id, name, name_ru, dev_hint_ru) VALUES (4, 'Chopping', 'Рубящий', 'Топор Секира Бердыш Алебарда') ON CONFLICT DO NOTHING;
INSERT INTO game_data.damage_types (id, name, name_ru, dev_hint_ru) VALUES (5, 'Force', 'Силовой', '"Проявления: Волшебные стрелы (Magic Missile), телекинетические удары, невидимые клинки силы.

Уязвимы: Призраки, астральные существа (иногда).

Устойчивы/Иммунны: Практически нет, это магическая энергия.

Особенность: Редко имеет сопротивление. Наносит чистый, не-элементальный магический урон. Часто не может быть отражен или заблокирован обычным щитом, только магическими средствами. Может отбрасывать цели."') ON CONFLICT DO NOTHING;
INSERT INTO game_data.damage_types (id, name, name_ru, dev_hint_ru) VALUES (6, 'Fire', 'Огненный', '"Проявления: Огненные шары, пламя дракона, горение.

Уязвимы: Духи льда, растения, нежить (часто), насекомые, ледяные элементали.

Устойчивы/Иммунны: Демоны, огненные элементали, лавовые големы, красные драконы.

Особенность: Часто накладывает эффект горения (DoT), может разрушать ледяные преграды, поджигать местность. Противоположен Ледяному.

"') ON CONFLICT DO NOTHING;
INSERT INTO game_data.damage_types (id, name, name_ru, dev_hint_ru) VALUES (7, 'Frost', 'Ледяной', '"Проявления: Ледяные стрелы, стужа, обморожение, ледяная тюрьма.

Уязвимы: Огненные существа, драконы, рептилии, лавовые големы.

Устойчивы/Иммунны: Ледяные элементали, белые драконы, арктические существа.

Особенность: Часто накладывает эффект замедления или оковы (обездвиживания). Может создавать ледяной наст на воде. Противоположен Огненному."') ON CONFLICT DO NOTHING;
INSERT INTO game_data.damage_types (id, name, name_ru, dev_hint_ru) VALUES (8, 'Electric', 'Электрический', '"Проявления: Молнии, электрические разряды, шок, громовые волны.

Уязвимы: Механизмы (роботы, големы), существа в металлической броне, водные/слизневые существа (проводят ток).

Устойчивы/Иммунны: Воздушные элементали, существа из изоляционных материалов (камень, дерево — не всегда).

Особенность: Высокий шанс наложить эффект стана (паралич, оглушение). Урон может цепляться на несколько ближайших целей. Часто игнорирует часть брони."') ON CONFLICT DO NOTHING;
INSERT INTO game_data.damage_types (id, name, name_ru, dev_hint_ru) VALUES (9, 'Poison', 'Ядовитый', '"Проявления: Ядовитые облака, укусы, отравленные клинки, токсичные выбросы.

Уязвимы: Органические, живые существа (люди, звери, растения).

Устойчивы/Иммунны: Нежить, конструкты, каменные/земляные элементали, ядовитые сами по себе монстры (гигантские пауки, скорпионы).

Особенность:* Практически всегда наносит урон по времени (Damage over Time - DoT). Может накладывать ослабляющие эффекты (снижение характеристик, замедление регенерации)."') ON CONFLICT DO NOTHING;
INSERT INTO game_data.damage_types (id, name, name_ru, dev_hint_ru) VALUES (10, 'Sonic', 'Звуковой', '"Проявления: Разрушительный рёв, ударная звуковая волна, резонанс, разрыв барабанных перепонок, крик банши, звуковые пушки.

Уязвимы: Стеклянные/хрустальные существа, конструкции с хрупкими элементами, существа с тонким слухом (летучие мыши, эльфы). Механизмы (может нарушить работу).

Устойчивы/Иммунны: Глухие существа, каменные/земляные големы (плохо проводят звук), существа из вакуума/космоса, призраки (иногда).

Особенность: Часто игнорирует физическую броню (звук проходит сквозь пластины) и магическую защиту. Имеет высокий шанс наложить дебаффы: оглушение, дезориентация, снижение точности, развеивание иллюзий/невидимости. Может активировать хрупкие механизмы или разрушать звуковые барьеры. В тихих локациях (подземелья) может привлекать других врагов."') ON CONFLICT DO NOTHING;
INSERT INTO game_data.damage_types (id, name, name_ru, dev_hint_ru) VALUES (11, 'Light', 'Световой', '"Проявления: Божественный гнев, очищающий свет, заклинания жрецов и паладинов, освящённое оружие.

Уязвимы: Нежить (зомби, скелеты, призраки, вампиры), демоны/исчадия, существа Тьмы и Хаоса.

Устойчивы/Иммунны: Ангелы, святые существа, некоторые нейтральные духи природы.

Особенность: Часто прерывает концентрацию магов, изгоняет/отпугивает нежить, наносит увеличенный/критический урон к уязвимым типам. Может накладывать ослепление."') ON CONFLICT DO NOTHING;
INSERT INTO game_data.damage_types (id, name, name_ru, dev_hint_ru) VALUES (12, 'Necrotic', 'Некротический', '"Проявления: Энергия смерти, вампиризм (кража HP), проклятия, теневая магия.

Уязвимы: Живые существа (подрывает жизненную силу), святые существа (иногда).

Устойчивы/Иммунны: Нежить (часто её лечит или не вредит), демоны Тени, существа Смерти.

Особенность: Может накладывать эффекты снижения максимального запаса здоровья, запрета на лечение (анти-хил), страха или ослабления. Противоположен Священному."') ON CONFLICT DO NOTHING;
INSERT INTO game_data.damage_types (id, name, name_ru, dev_hint_ru) VALUES (13, 'Telepathic', 'Телепатический', '"Проявления: Взрыв разума, телепатическое копье, ментальные тиски, навязывание невыносимых мыслей, иллюзии, причиняющие реальную боль, атаки по сновидениям.

Уязвимы: Все разумные существа (люди, эльфы, орки, драконы). Особенно те, кто обладает высоким интеллектом, но слабой силой воли.

Устойчивы/Иммунны: Конструкты, големы, неразумная нежить (зомби, скелеты), животные/звери с примитивным интеллектом, берсеркеры/дикари (их разум хаотичен и защищен яростью), роботы, некоторые демоны пустоты.

Особенность: Почти всегда игнорирует физическую броню и магическую защиту (зависит от сопротивления ментальным эффектам/силе воли). Реже наносит чистый урон, чаще накладывает мощнейшие контроль-эффекты: страх, очарование, сон, паралич, безумие, взятие под контроль. Может снижать характеристики (Интеллект, Мудрость, Харизму). Не оставляет физических ран."') ON CONFLICT DO NOTHING;
INSERT INTO game_data.damage_types (id, name, name_ru, dev_hint_ru) VALUES (14, 'Acid', 'Кислотный', '"Проявления: Плевки кислотой, едкие пары, растворяющие жидкости.

Уязвимы: Существа с хитиновым/металлическим панцирем, механизмы, бронированные цели.

Устойчивы/Иммунны: Слизни, кислотные элементали, аморфные существа.

Особенность: Часто снижает защиту (броню) цели на время или игнорирует её часть. Может наносить урон предметам/снаряжению."') ON CONFLICT DO NOTHING;
INSERT INTO game_data.damage_types (id, name, name_ru, dev_hint_ru) VALUES (15, 'Chaos', 'Хаотический', '"Проявления: Нестабильная энергия хаоса, смесь стихий, дикая магия.

Уязвимы: Существа Закона и Порядка (дэвы, механикусы).

Устойчивы/Иммунны: Демоны Бездны, хаотические элементали.

Особенность: Часто игнорирует фиксированный процент всех сопротивлений (например, 50% урона проходит всегда) или имеет шанс наложить случайный негативный эффект. Непредсказуем."') ON CONFLICT DO NOTHING;
INSERT INTO game_data.damage_types (id, name, name_ru, dev_hint_ru) VALUES (16, 'Pure', 'Чистый', '"Проявления: Божественная кара, нарушение законов реальности, урон из самого ""кода"" вселенной, абсолютное расщепление материи, хирургически точное уничтожение без эффектов.

Уязвимы: ВСЕ. По определению.

Устойчивы/Иммунны: НИКТО. По определению. (Хотя могут быть исключения в виде уникальных божественных артефактов или "сюжетная неуязвимость").

Особенность: Игнорирует ВСЕ виды защиты, сопротивления, иммунитеты, поглощения и снижения урона. Наносит ровно то количество урона, которое указано. Это последний аргумент в игровом балансе. Крайне редок, обычно доступен через:

Ультимативные способности с долгим откатом.

Легендарные/божественные артефакты.

Способности, срабатывающие при выполнении сложных условий (например, при HP противника ниже 5%).

Как механика для определенных типов врагов (например, урон от падения в бездну или ""неизбежная"" атака босса)."') ON CONFLICT DO NOTHING;







INSERT INTO game_data.slot_types (id, name, name_ru) VALUES (1, 'Head', 'Голова') ON CONFLICT DO NOTHING;
INSERT INTO game_data.slot_types (id, name, name_ru) VALUES (2, 'Shoulders', 'Наплечники') ON CONFLICT DO NOTHING;
INSERT INTO game_data.slot_types (id, name, name_ru) VALUES (3, 'Chest', 'Нагрудник') ON CONFLICT DO NOTHING;
INSERT INTO game_data.slot_types (id, name, name_ru) VALUES (4, 'Hands', 'Руки') ON CONFLICT DO NOTHING;
INSERT INTO game_data.slot_types (id, name, name_ru) VALUES (5, 'Legs', 'Поножи') ON CONFLICT DO NOTHING;
INSERT INTO game_data.slot_types (id, name, name_ru) VALUES (6, 'Feet', 'Ступни') ON CONFLICT DO NOTHING;
INSERT INTO game_data.slot_types (id, name, name_ru) VALUES (7, 'Waist', 'Пояс') ON CONFLICT DO NOTHING;
INSERT INTO game_data.slot_types (id, name, name_ru) VALUES (8, 'Wrist', 'Запястья') ON CONFLICT DO NOTHING;
INSERT INTO game_data.slot_types (id, name, name_ru) VALUES (9, 'Back', 'Спина') ON CONFLICT DO NOTHING;
INSERT INTO game_data.slot_types (id, name, name_ru) VALUES (10, 'Bracelet', 'Браслет') ON CONFLICT DO NOTHING;
INSERT INTO game_data.slot_types (id, name, name_ru) VALUES (11, 'HandLeftRight', 'Рука Левая Правая') ON CONFLICT DO NOTHING;
INSERT INTO game_data.slot_types (id, name, name_ru) VALUES (12, 'Ring', 'Кольцо') ON CONFLICT DO NOTHING;
INSERT INTO game_data.slot_types (id, name, name_ru) VALUES (13, 'Earring', 'Серьги') ON CONFLICT DO NOTHING;
INSERT INTO game_data.slot_types (id, name, name_ru) VALUES (14, 'Trinket', 'Аксессуар') ON CONFLICT DO NOTHING;





INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (9, 'War Fan', 'Боевой веер', 800, 11, false, true, '5d3_10', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (10, 'Scimitar', 'Скимитар', 1200, 11, false, true, '6d4_15', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (11, 'Katana', 'Катана', 1100, 11, false, true, '7d3_14', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (14, 'Morning Star', 'Моргенштерн', 5000, 11, false, true, '1d123_62', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (1, 'Sword', 'Меч', 1600, 11, false, true, '8d4_20', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (18, 'Bow', 'Лук', 960, 11, false, true, '4d2_6', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (21, 'Pike', 'Пика', 2800, 11, false, true, '24d2_36', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (20, 'Rapier', 'Рапира', 850, 11, false, true, '8d2_12', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (19, 'Trident', 'Трезубец', 3400, 11, false, true, '28d2_42', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (17, 'Crossbow', 'Арбалет', 3200, 11, false, true, '14d2_21', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (7, 'Shuriken', 'Сюрикен', 180, 11, false, true, '6d2_9', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (22, 'Spear', 'Копьё', 2200, 11, false, true, '20d2_30', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (24, 'Dagger', 'Кинжал', 400, 11, false, true, '4d2_6', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (12, 'Yataghan', 'Ятаган', 1000, 11, false, true, '6d3_12', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (13, 'Sabre', 'Сабля', 1100, 11, false, true, '7d3_14', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (15, 'Warhammer', 'Боевой молот', 6200, 11, false, true, '1d153_77', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (16, 'Mace', 'Булава', 3000, 11, false, true, '1d73_37', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (3, 'Halberd', 'Алебарда', 4400, 11, false, true, '4d27_56', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (5, 'Poleaxe', 'Секира', 4000, 11, false, true, '4d24_50', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (23, 'Broadaxe', 'Широкий топор', 5200, 11, false, true, '4d32_66', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (2, 'Axe', 'Топор', 2800, 11, false, true, '4d17_36', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (4, 'Berdysh', 'Бердыш', 3800, 11, false, true, '4d23_48', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (6, 'Chakram', 'Чакрам', 350, 11, false, true, '5d3_10', 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.equipment_types (id, name, name_ru, mass_physical, slot_type_id, can_craft_jewelcrafting, can_craft_smithing, attack, spend_action_points) VALUES (8, 'Scythe', 'Коса', 3200, 11, false, true, '5d3_10', 0) ON CONFLICT DO NOTHING;







INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (1, 'Iron', 'Железо') ON CONFLICT DO NOTHING;
INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (2, 'Steel', 'Сталь') ON CONFLICT DO NOTHING;
INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (3, 'Penetron', 'Пенетрон') ON CONFLICT DO NOTHING;
INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (4, 'Brutalite', 'Бруталит') ON CONFLICT DO NOTHING;
INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (5, 'Ostenium', 'Остениум') ON CONFLICT DO NOTHING;
INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (6, 'Divisorium', 'Дивизориум') ON CONFLICT DO NOTHING;
INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (7, 'Ebonite', 'Эбонит') ON CONFLICT DO NOTHING;
INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (8, 'Volantir', 'Волантир') ON CONFLICT DO NOTHING;
INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (9, 'Pyronite', 'Пиронит') ON CONFLICT DO NOTHING;
INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (10, 'Cryonite', 'Крионит') ON CONFLICT DO NOTHING;
INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (12, 'Vermium', 'Вермиум') ON CONFLICT DO NOTHING;
INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (13, 'Vibranium', 'Вибраниум') ON CONFLICT DO NOTHING;
INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (14, 'Solarium', 'Солариум') ON CONFLICT DO NOTHING;
INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (15, 'Mortium', 'Мортиум') ON CONFLICT DO NOTHING;
INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (16, 'Somnir', 'Сомнир') ON CONFLICT DO NOTHING;
INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (17, 'Acidium', 'Ацидиум') ON CONFLICT DO NOTHING;
INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (18, 'Discord', 'Дискорд') ON CONFLICT DO NOTHING;
INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (19, 'Nullite', 'Нуллит') ON CONFLICT DO NOTHING;
INSERT INTO game_data.smithing_materials (id, name, name_ru) VALUES (11, 'Fulgurite', 'Фульгурит') ON CONFLICT DO NOTHING;







INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (1, 2, 1, 10) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (2, 2, 2, 10) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (3, 2, 3, 10) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (4, 2, 4, 10) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (5, 3, 1, 50) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (6, 4, 2, 50) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (7, 5, 3, 50) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (8, 6, 4, 50) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (9, 7, 1, 40) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (10, 7, 2, 40) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (11, 7, 3, 40) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (12, 7, 4, 40) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (13, 8, 5, 50) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (14, 9, 6, 50) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (15, 10, 7, 50) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (16, 11, 8, 50) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (17, 12, 9, 50) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (18, 13, 10, 50) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (19, 14, 11, 50) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (20, 15, 12, 50) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (21, 16, 13, 50) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (22, 17, 14, 50) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (23, 18, 15, 500) ON CONFLICT DO NOTHING;
INSERT INTO game_data.material_damage_percents (id, smithing_materials_id, damage_type_id, percent) VALUES (24, 19, 16, 50) ON CONFLICT DO NOTHING;





INSERT INTO game_data.base_heroes (id, name, rarity, is_unique, health, damage) VALUES (1, 'Warrior', 1, false, '16d24_200', '4d21_44') ON CONFLICT DO NOTHING;
INSERT INTO game_data.base_heroes (id, name, rarity, is_unique, health, damage) VALUES (2, 'Huntress', 1, false, '10d28_145', '5d21_55') ON CONFLICT DO NOTHING;
INSERT INTO game_data.base_heroes (id, name, rarity, is_unique, health, damage) VALUES (3, 'Hammerman', 1, false, '11d39_220', '3d25_39') ON CONFLICT DO NOTHING;
INSERT INTO game_data.base_heroes (id, name, rarity, is_unique, health, damage) VALUES (4, 'Rogue', 1, false, '15d21_165', '4d23_48') ON CONFLICT DO NOTHING;
INSERT INTO game_data.base_heroes (id, name, rarity, is_unique, health, damage) VALUES (5, 'Battle orc', 1, false, '16d58_472', '4d21_44') ON CONFLICT DO NOTHING;





INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (14, 1, 15) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (14, 2, 85) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (19, 1, 73) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (19, 2, 9) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (2, 2, 10) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (2, 3, 5) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (2, 4, 85) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (16, 1, 16) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (19, 3, 18) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (21, 1, 100) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (16, 2, 75) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (16, 4, 9) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (15, 2, 100) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (7, 1, 55) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (7, 3, 45) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (6, 1, 11) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (6, 3, 89) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (4, 1, 12) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (22, 1, 80) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (22, 2, 5) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (22, 3, 15) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (23, 4, 100) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (4, 3, 2) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (18, 1, 100) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (17, 1, 100) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (24, 1, 70) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (24, 2, 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (24, 3, 30) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (4, 4, 86) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (8, 1, 35) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (8, 3, 65) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (1, 1, 27) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (24, 4, 0) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (1, 3, 56) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (1, 4, 17) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (13, 1, 13) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (13, 3, 65) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (13, 4, 22) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (3, 1, 37) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (3, 2, 7) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (3, 4, 56) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (5, 2, 8) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (5, 3, 17) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (5, 4, 75) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (10, 1, 6) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (10, 3, 87) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (10, 4, 7) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (11, 1, 13) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (11, 3, 80) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (11, 4, 7) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (12, 1, 19) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (12, 3, 77) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (12, 4, 4) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (20, 1, 84) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (20, 3, 14) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (20, 4, 2) ON CONFLICT DO NOTHING;
INSERT INTO game_data.x_equipment_type_damage_type (equipment_type_id, damage_type_id, damage_coef) VALUES (9, 3, 100) ON CONFLICT DO NOTHING;







SELECT pg_catalog.setval('game_data.creature_types_id_seq', 9, false);
SELECT pg_catalog.setval('game_data.damage_types_id_seq', 17, false);
SELECT pg_catalog.setval('game_data.slot_types_id_seq', 14, true);
SELECT pg_catalog.setval('game_data.smithing_materials_id_seq', 19, true);
SELECT pg_catalog.setval('game_data.equipment_types_id_seq', 24, true);
SELECT pg_catalog.setval('game_data.base_heroes_id_seq', 6, false);
SELECT pg_catalog.setval('game_data.material_damage_percents_id_seq', 25, false);