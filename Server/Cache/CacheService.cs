
using General.DTO.Entities;
using General.DTO.Entities.GameData;
using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.Server;

namespace Server.Cache;

public class CacheService()
{
    /// <summary> Возвращает JSON-строку с данными. </summary>
    /// <returns> JSON-строка со всеми константными игровыми данными необходимыми на стороне клиента. </returns>
    /// <exception cref="InvalidOperationException">
    /// Возникает, если кэш не был инициализирован.
    /// </exception>
    public string gameDataJson { get => field!; private set; }

    public Dictionary<int, UserBanReason> tableUserBanReasons { get; private set; } = null!;
    public Dictionary<int, UserSessionInactivationReason> tableUserSessionInactivationReasons { get; private set; } = null!;
    public Dictionary<int, BaseEquipment> tableBaseEquipments { get; private set; } = null!;
    public Dictionary<string, BaseEquipment> tableBaseEquipmentsByName { get; private set; } = null!;

    public Dictionary<int, BaseHero> tableBaseHeroesOnlyPlayable { get; private set; } = null!;
    public Dictionary<int, BaseHero> tableBaseHeroes { get; private set; } = null!;

    public Dictionary<int, Ability> tableAbilities { get; private set; } = null!;
    public Dictionary<int, CreatureType> tableCreatureTypes { get; private set; } = null!;
    public Dictionary<int, DamageType> tableDamageTypes { get; private set; } = null!;
    public Dictionary<int, EquipmentType> tableEquipmentTypes { get; private set; } = null!;
    public Dictionary<int, MaterialDamagePercent> tableMaterialDamagePercents { get; private set; } = null!;
    public Dictionary<ESlotType, SlotType> tableSlotTypes { get; private set; } = null!;
    public Dictionary<ESlot, Slot> tableSlots { get; private set; } = null!;
    public Dictionary<int, SmithingMaterial> tableSmithingMaterials { get; private set; } = null!;
    public Dictionary<int, X_EquipmentType_DamageType> tableX_EquipmentTypes_DamageTypes { get; private set; } = null!;
    public Dictionary<int, X_Hero_CreatureType> tableX_Heroes_CreatureTypes { get; private set; } = null!;
    public Dictionary<int, X_Battlefield_BaseHero> tableX_Battlefields_BaseHeroes { get; private set; } = null!;
    public Dictionary<int, X_BaseHero_Ability> tableX_BaseHeroes_Abilities { get; private set; } = null!;

    public Dictionary<EBattlefield, General.DTO.Entities.GameData.Battlefield> tableBattlefields { get; private set; } = null!;

    /// <summary>Загрузка константных данных в оперативную память. Эти данные меняются только при не работающем сервере.</summary>
    public void LoadServerData(DbContextGame db)
    {
        tableUserBanReasons = db.userBanReasons.AsNoTracking().ToDictionary(a => a.id);

        //LoadTableUserSessionInactivationReasons(db);
        tableUserSessionInactivationReasons = db.userSessionInactivationReasons.AsNoTracking().ToDictionary(a => a.id);
        tableBaseEquipments = db.baseEquipments.AsNoTracking().ToDictionary(a => a.id);
        tableBaseEquipmentsByName = [];
        foreach (KeyValuePair<int, BaseEquipment> kv in tableBaseEquipments)
        {
            BaseEquipment v = kv.Value;
            tableBaseEquipmentsByName.Add(v.name, v);
        }

        tableBaseHeroesOnlyPlayable = db.baseHeroes.AsNoTracking().Where(a => a.isPlayable).ToDictionary(a => a.id);


        tableBaseHeroes = [];
        foreach (KeyValuePair<int, BaseHero> i in tableBaseHeroesOnlyPlayable)
        {
            tableBaseHeroes.Add(i.Key, i.Value);
        }
        foreach (BaseHero? i in db.baseHeroes.AsNoTracking().Where(a => !a.isPlayable).ToList())
        {
            tableBaseHeroes.Add(i.id, i);
        }


        tableCreatureTypes = db.creatureTypes.AsNoTracking().ToDictionary(a => a.id);
        tableAbilities = db.abilities.AsNoTracking().ToDictionary(a => a.id);
        tableDamageTypes = db.damageTypes.AsNoTracking().ToDictionary(a => a.id);
        tableEquipmentTypes = db.equipmentTypes.AsNoTracking().ToDictionary(a => a.id);
        tableMaterialDamagePercents = db.materialDamagePercents.AsNoTracking().ToDictionary(a => a.id);
        tableSlotTypes = db.slotTypes.AsNoTracking().ToDictionary(a => a.id);
        tableSlots = db.slots.AsNoTracking().ToDictionary(a => a.id);
        tableSmithingMaterials = db.smithingMaterials.AsNoTracking().ToDictionary(a => a.id);
        tableX_EquipmentTypes_DamageTypes = db.x_EquipmentTypes_DamageTypes.AsNoTracking().ToDictionary(a => a.id);
        tableX_Heroes_CreatureTypes = db.x_Heroes_CreatureTypes.AsNoTracking().ToDictionary(a => a.id);
        tableBattlefields = db.battlefields.AsNoTracking().ToDictionary(a => a.id);
        tableX_Battlefields_BaseHeroes = db.x_Battlefields_BaseHeroes.AsNoTracking().ToDictionary(a => a.id);
        tableX_BaseHeroes_Abilities = db.x_BaseHeroes_Abilities.AsNoTracking().ToDictionary(a => a.id);


        ThrowIfDataNotCorrect();

        DtoContainerGameData container = new()
        {
            baseEquipments = tableBaseEquipments.Values.AsEnumerable(),
            baseHeroes = tableBaseHeroesOnlyPlayable.Values.AsEnumerable(),

            creatureTypes = tableCreatureTypes.Values.AsEnumerable(),

            damageTypes = tableDamageTypes.Values.AsEnumerable(),

            equipmentTypes = tableEquipmentTypes.Values.AsEnumerable(),

            materialDamagePercents = tableMaterialDamagePercents.Values.AsEnumerable(),

            slotTypes = tableSlotTypes.Values.AsEnumerable(),

            smithingMaterials = tableSmithingMaterials.Values.AsEnumerable(),

            xEquipmentTypesDamageTypes = tableX_EquipmentTypes_DamageTypes.Values.AsEnumerable(),

            xHeroesCreatureTypes = tableX_Heroes_CreatureTypes.Values.AsEnumerable(),

            slots = tableSlots.Values.AsEnumerable(),

            xBattlefieldNpc = tableX_Battlefields_BaseHeroes.Values.AsEnumerable(),

            battlefields = tableBattlefields.Values.AsEnumerable(),

            abilities = tableAbilities.Values.AsEnumerable(),

            xBaseHeroesAbilities = tableX_BaseHeroes_Abilities.Values.AsEnumerable()
        };


        gameDataJson = JSON.Serialize(container);

        if (string.IsNullOrWhiteSpace(gameDataJson))
        {
            throw new InvalidOperationException("Кэш не инициализирован.");
        }

        foreach (KeyValuePair<int, BaseHero> kv in tableBaseHeroes)
        {
            BaseHero i = kv.Value;
            i.abilities = [.. tableAbilities.Values.Where(a => tableX_BaseHeroes_Abilities.Values.Any(x => x.baseHeroId == i.id && x.abilityId == a.id))];
        }

        foreach (KeyValuePair<int, BaseEquipment> kv in tableBaseEquipments)
        {
            BaseEquipment i = kv.Value;
            i.equipmentType = tableEquipmentTypes[i.equipmentTypeId];
            i.smithingMaterial = i.smithingMaterialId != null ? tableSmithingMaterials[i.smithingMaterialId.Value] : null;
        }

        foreach (KeyValuePair<int, EquipmentType> kv in tableEquipmentTypes)
        {
            EquipmentType i = kv.Value;
            i.slotType = tableSlotTypes[i.slotTypeId];
        }

        foreach (KeyValuePair<int, MaterialDamagePercent> kv in tableMaterialDamagePercents)
        {
            MaterialDamagePercent i = kv.Value;
            i.smithingMaterials = tableSmithingMaterials[i.smithingMaterialsId];
            i.damageType = tableDamageTypes[i.damageTypeId];
        }

        foreach (KeyValuePair<ESlot, Slot> kv in tableSlots)
        {
            Slot i = kv.Value;
            i.slotType = tableSlotTypes[i.slotTypeId];
        }

        foreach (KeyValuePair<int, X_EquipmentType_DamageType> kv in tableX_EquipmentTypes_DamageTypes)
        {
            X_EquipmentType_DamageType i = kv.Value;
            i.equipmentType = tableEquipmentTypes[i.equipmentTypeId];
            i.damageType = tableDamageTypes[i.damageTypeId];
        }

        foreach (KeyValuePair<int, X_Hero_CreatureType> kv in tableX_Heroes_CreatureTypes)
        {
            X_Hero_CreatureType i = kv.Value;
            i.baseHero = tableBaseHeroesOnlyPlayable[i.baseHeroId];
            i.creatureType = tableCreatureTypes[i.creatureTypeId];
        }

        foreach (KeyValuePair<int, X_Battlefield_BaseHero> kv in tableX_Battlefields_BaseHeroes)
        {
            X_Battlefield_BaseHero i = kv.Value;
            i.baseHero = tableBaseHeroesOnlyPlayable[i.baseHeroId];
            i.battlefield = tableBattlefields[i.battlefieldId];
        }

        foreach (KeyValuePair<int, X_BaseHero_Ability> kv in tableX_BaseHeroes_Abilities)
        {
            X_BaseHero_Ability i = kv.Value;
            i.baseHero = tableBaseHeroesOnlyPlayable[i.baseHeroId];
            i.ability = tableAbilities[i.abilityId];
        }



    }

    /*
    /// <summary>
    /// Синхронизирует таблицу <see cref="UserSessionInactivationReason"/> с перечислением <see cref="InactivationReason"/>.
    /// Если в таблице отсутствуют записи для каких-либо значений enum — добавляет их,
    /// после чего повторно загружает кешированный список.
    /// </summary>
    /// <param name="db">Контекст базы данных игры.</param>
    private void LoadTableUserSessionInactivationReasons(DbContextGame db)
    {
        // Флаг, указывающий, были ли добавлены новые записи в БД.
        // Если true — после сохранения потребуется повторная загрузка кеша.
        bool reload = false;

        // Локальная функция: загружает все записи из таблицы UserSessionInactivationReasons
        // и сохраняет их в кеширующее свойство TableUserSessionInactivationReasons.
        // AsNoTracking используется, так как данные нужны только для чтения.
        void Load()
        {
            TableUserSessionInactivationReasons = db.UserSessionInactivationReasons.AsNoTracking().ToDictionary(a => a.Id);
        }

        // Первичная загрузка текущего состояния таблицы в кеш
        Load();

        // Перебираем все значения перечисления InactivationReason
        foreach (InactivationReason item in Enum.GetValues<InactivationReason>())
        {
            // Если в загруженных данных нет записи с текущим кодом enum —
            // значит, этого значения ещё нет в таблице
            if (!TableUserSessionInactivationReasons.Any(a => a.Value.Code == item))
            {
                // Создаём новую сущность: Name — строковое представление enum, Code — само значение enum
                var entity = new UserSessionInactivationReason
                {
                    Name = item.ToString(),
                    Code = item
                };

                // Добавляем сущность в контекст для последующей вставки в БД.
                _ = db.UserSessionInactivationReasons.Add(entity);

                // Поднимаем флаг, чтобы после цикла сохранить изменения и перезагрузить кеш
                reload = true;
            }
        }

        // Если были добавлены новые записи — сохраняем изменения в БД
        // и заново загружаем кеш (чтобы подтянуть сгенерированные базой ID и прочие поля)
        if (reload)
        {
            _ = db.SaveChanges();
            Load();
        }
    }
    
    public int GetInactivationReasonIdByCode(InactivationReason code)
    {
        return TableUserSessionInactivationReasons.First(a => a.Value.Code == code).Value.Id;
    }
    */

    public void ThrowIfDataNotCorrect()
    {
        foreach (EBattlefield i in Enum.GetValues<EBattlefield>())
        {
            if (!tableBattlefields.Values.Any(a => string.Equals(a.enumName.Replace("__", ""), i.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Not correct data in table Battlefields and enum EBattleFiled");
            }
        }
    }
}
