using General;
using General.DTO.Entities;
using General.DTO.Entities.GameData;
using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres;
using Server_DB_Postgres.Entities.GameData;
using Server_DB_Postgres.Entities.Server;
using System.Text.Json;

namespace Server.Cache;

public class CacheService()
{
    /// <summary> Возвращает JSON-строку с данными. </summary>
    /// <returns> JSON-строка со всеми константными игровыми данными необходимыми на стороне клиента. </returns>
    /// <exception cref="InvalidOperationException">
    /// Возникает, если кэш не был инициализирован.
    /// </exception>
    public string GameDataJson { get => field!; private set; }

    public Dictionary<int, UserBanReason> TableUserBanReasons { get; private set; } = null!;
    public Dictionary<int, UserSessionInactivationReason> TableUserSessionInactivationReasons { get; private set; } = null!;
    public Dictionary<int, BaseEquipment> TableBaseEquipments { get; private set; } = null!;
    public Dictionary<string, BaseEquipment> TableBaseEquipmentsByName { get; private set; } = null!;
    public Dictionary<int, BaseHero> TableBaseHeroes { get; private set; } = null!;
    public Dictionary<int, CreatureType> TableCreatureTypes { get; private set; } = null!;
    public Dictionary<int, DamageType> TableDamageTypes { get; private set; } = null!;
    public Dictionary<int, EquipmentType> TableEquipmentTypes { get; private set; } = null!;
    public Dictionary<int, MaterialDamagePercent> TableMaterialDamagePercents { get; private set; } = null!;
    public Dictionary<ESlotType, SlotType> TableSlotTypes { get; private set; } = null!;
    public Dictionary<ESlot, Slot> TableSlots { get; private set; } = null!;
    public Dictionary<int, SmithingMaterial> TableSmithingMaterials { get; private set; } = null!;
    public Dictionary<int, X_EquipmentType_DamageType> TableX_EquipmentTypes_DamageTypes { get; private set; } = null!;
    public Dictionary<int, X_Hero_CreatureType> TableX_Heroes_CreatureTypes { get; private set; } = null!;
    public Dictionary<int, X_Battlefield_BaseNpc> TableX_Battlefields_Npcs { get; private set; } = null!;
    public Dictionary<int, BaseNpc> TableBaseNpcs { get; private set; } = null!;
    public Dictionary<int, Battlefield> TableBattlefields { get; private set; } = null!;

    /// <summary>
    /// Загрузка константных данных в оперативную память. Эти данные меняются только при не работающем сервере.
    /// </summary>
    /// <param name="db"></param>
    /// <returns></returns>
    public void LoadServerData(DbContextGame db)
    {
        TableUserBanReasons = db.UserBanReasons.AsNoTracking().ToDictionary(a => a.Id);

        LoadTableUserSessionInactivationReasons(db);

        TableBaseEquipments = db.BaseEquipments.AsNoTracking().ToDictionary(a => a.Id);
        TableBaseEquipmentsByName = [];
        foreach (KeyValuePair<int, BaseEquipment> kv in TableBaseEquipments)
        {
            BaseEquipment v = kv.Value;
            TableBaseEquipmentsByName.Add(v.Name, v);
        }

        TableBaseHeroes = db.BaseHeroes.AsNoTracking().ToDictionary(a => a.Id);
        TableCreatureTypes = db.CreatureTypes.AsNoTracking().ToDictionary(a => a.Id);
        TableDamageTypes = db.DamageTypes.AsNoTracking().ToDictionary(a => a.Id);
        TableEquipmentTypes = db.EquipmentTypes.AsNoTracking().ToDictionary(a => a.Id);
        TableMaterialDamagePercents = db.MaterialDamagePercents.AsNoTracking().ToDictionary(a => a.Id);
        TableSlotTypes = db.SlotTypes.AsNoTracking().ToDictionary(a => a.Id);
        TableSlots = db.Slots.AsNoTracking().ToDictionary(a => a.Id);
        TableSmithingMaterials = db.SmithingMaterials.AsNoTracking().ToDictionary(a => a.Id);
        TableX_EquipmentTypes_DamageTypes = db.x_EquipmentTypes_DamageTypes.AsNoTracking().ToDictionary(a => a.Id);
        TableX_Heroes_CreatureTypes = db.x_Heroes_CreatureTypes.AsNoTracking().ToDictionary(a => a.Id);
        TableBaseNpcs = db.BaseNpcs.AsNoTracking().ToDictionary(a => a.Id);
        TableBattlefields = db.Battlefields.AsNoTracking().ToDictionary(a => a.Id);
        TableX_Battlefields_Npcs = db.x_Battlefields_BaseNpcs.AsNoTracking().ToDictionary(a => a.Id);


        DtoContainerGameData container = new(
            baseEquipments: TableBaseEquipments.Values.Select(static h => new DtoBaseEquipment(h.Id, h.Name, h.Rarity, h.IsUnique, h.EquipmentTypeId, null, h.PossibleStats)),

            baseHeroes: TableBaseHeroes.Values.Select(static h => new DtoBaseHero(h.Id, h.Name, h.Rarity, h.IsUnique, h.MainStat, h.Health, h.Damage, h.Strength, h.Agility, h.Intelligence, h.CritChance, h.CritMultiplier, h.Haste, h.Versality, h.EndurancePhysical, h.EnduranceMagical, h.Initiative)),

            creatureTypes: TableCreatureTypes.Values.Select(static h => new DtoCreatureType(h.Id, h.Name)),

            damageTypes: TableDamageTypes.Values.Select(static h => new DtoDamageType(h.Id, h.Name)),

            equipmentTypes: TableEquipmentTypes.Values.Select(static h => new DtoEquipmentType(h.Id, h.Name, h.MassPhysical, h.MassMagical, h.SlotTypeId, h.CanCraftSmithing, h.CanCraftJewelcrafting, h.SpendActionPoints, h.BlockOtherHand, null, h.PossibleStats)),

            materialDamagePercents: TableMaterialDamagePercents.Values.Select(static h => new DtoMaterialDamagePercent(h.Id, h.SmithingMaterialsId, h.DamageTypeId, h.Percent)),

            slotTypes: TableSlotTypes.Values.Select(static h => new DtoSlotType(h.Id, h.Name, h.HaveAltSlot, h.Sorting)),

            smithingMaterials: TableSmithingMaterials.Values.Select(static h => new DtoSmithingMaterial(h.Id, h.Name)),

            xEquipmentTypesDamageTypes: TableX_EquipmentTypes_DamageTypes.Values.Select(static h => new DtoXEquipmentTypeDamageType(h.EquipmentTypeId, h.DamageTypeId, h.DamageCoef)),

            xHeroesCreatureTypes: TableX_Heroes_CreatureTypes.Values.Select(static h => new DtoXHeroCreatureType(h.BaseHeroId, h.CreatureTypeId, null, null)),

            slots: TableSlots.Values.Select(static h => new DtoSlot(h.Id, h.Name, h.SlotTypeId)),

            xBattlefieldNpc: TableX_Battlefields_Npcs.Values.Select(static h => new DtoXBattlefieldNpc(h.Id, h.BattlefieldId, h.BaseNpcId, h.GuarantSpawn, h.ProbabilitySpawn, h.PossibleRank))
            );

        GameDataJson = JsonSerializer.Serialize(container, GlobalJsonContext.Default.DtoContainerGameData);

        if (string.IsNullOrWhiteSpace(GameDataJson))
        {
            throw new InvalidOperationException("Кэш не инициализирован.");
        }

        foreach (KeyValuePair<int, BaseEquipment> kv in TableBaseEquipments)
        {
            BaseEquipment i = kv.Value;
            i.EquipmentType = TableEquipmentTypes[i.EquipmentTypeId];
            i.SmithingMaterial = i.SmithingMaterialId != null ? TableSmithingMaterials[i.SmithingMaterialId.Value] : null;
        }

        foreach (KeyValuePair<int, EquipmentType> kv in TableEquipmentTypes)
        {
            EquipmentType i = kv.Value;
            i.SlotType = TableSlotTypes[i.SlotTypeId];
        }

        foreach (KeyValuePair<int, MaterialDamagePercent> kv in TableMaterialDamagePercents)
        {
            MaterialDamagePercent i = kv.Value;
            i.SmithingMaterials = TableSmithingMaterials[i.SmithingMaterialsId];
            i.DamageType = TableDamageTypes[i.DamageTypeId];
        }

        foreach (KeyValuePair<ESlot, Slot> kv in TableSlots)
        {
            Slot i = kv.Value;
            i.SlotType = TableSlotTypes[i.SlotTypeId];
        }

        foreach (KeyValuePair<int, X_EquipmentType_DamageType> kv in TableX_EquipmentTypes_DamageTypes)
        {
            X_EquipmentType_DamageType i = kv.Value;
            i.EquipmentType = TableEquipmentTypes[i.EquipmentTypeId];
            i.DamageType = TableDamageTypes[i.DamageTypeId];
        }

        foreach (KeyValuePair<int, X_Hero_CreatureType> kv in TableX_Heroes_CreatureTypes)
        {
            X_Hero_CreatureType i = kv.Value;
            i.BaseHero = TableBaseHeroes[i.BaseHeroId];
            i.CreatureType = TableCreatureTypes[i.CreatureTypeId];
        }

        foreach (KeyValuePair<int, X_Battlefield_BaseNpc> kv in TableX_Battlefields_Npcs)
        {
            X_Battlefield_BaseNpc i = kv.Value;
            i.BaseNpc = TableBaseNpcs[i.BaseNpcId];
            i.Battlefield = TableBattlefields[i.BattlefieldId];
        }


    }

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
}
