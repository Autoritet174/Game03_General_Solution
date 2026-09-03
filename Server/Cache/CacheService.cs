
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
    public string GameDataJson { get => field!; private set; }

    public Dictionary<int, UserBanReason> TableUserBanReasons { get; private set; } = null!;
    public Dictionary<int, UserSessionInactivationReason> TableUserSessionInactivationReasons { get; private set; } = null!;
    public Dictionary<int, BaseEquipment> TableBaseEquipments { get; private set; } = null!;
    public Dictionary<string, BaseEquipment> TableBaseEquipmentsByName { get; private set; } = null!;

    public Dictionary<int, BaseHero> TableBaseHeroesOnlyPlayable { get; private set; } = null!;
    public Dictionary<int, BaseHero> TableBaseHeroesWithNotPlayable { get; private set; } = null!;

    public Dictionary<int, CreatureType> TableCreatureTypes { get; private set; } = null!;
    public Dictionary<int, DamageType> TableDamageTypes { get; private set; } = null!;
    public Dictionary<int, EquipmentType> TableEquipmentTypes { get; private set; } = null!;
    public Dictionary<int, MaterialDamagePercent> TableMaterialDamagePercents { get; private set; } = null!;
    public Dictionary<ESlotType, SlotType> TableSlotTypes { get; private set; } = null!;
    public Dictionary<ESlot, Slot> TableSlots { get; private set; } = null!;
    public Dictionary<int, SmithingMaterial> TableSmithingMaterials { get; private set; } = null!;
    public Dictionary<int, X_EquipmentType_DamageType> TableX_EquipmentTypes_DamageTypes { get; private set; } = null!;
    public Dictionary<int, X_Hero_CreatureType> TableX_Heroes_CreatureTypes { get; private set; } = null!;
    public Dictionary<int, X_Battlefield_BaseHero> TableX_Battlefields_BaseHeroes { get; private set; } = null!;
    public Dictionary<EBattleFiled, General.DTO.Entities.GameData.Battlefield> TableBattlefields { get; private set; } = null!;

    /// <summary>
    /// Загрузка константных данных в оперативную память. Эти данные меняются только при не работающем сервере.
    /// </summary>
    /// <param name="db"></param>
    /// <returns></returns>
    public void LoadServerData(DbContextGame db)
    {
        TableUserBanReasons = db.UserBanReasons.AsNoTracking().ToDictionary(a => a.Id);

        //LoadTableUserSessionInactivationReasons(db);
        TableUserSessionInactivationReasons = db.UserSessionInactivationReasons.AsNoTracking().ToDictionary(a => a.Id);
        TableBaseEquipments = db.BaseEquipments.AsNoTracking().ToDictionary(a => a.id);
        TableBaseEquipmentsByName = [];
        foreach (KeyValuePair<int, BaseEquipment> kv in TableBaseEquipments)
        {
            BaseEquipment v = kv.Value;
            TableBaseEquipmentsByName.Add(v.name, v);
        }

        TableBaseHeroesOnlyPlayable = db.BaseHeroes.AsNoTracking().Where(a => a.isPlayable).ToDictionary(a => a.id);


        TableBaseHeroesWithNotPlayable = [];
        foreach (KeyValuePair<int, BaseHero> i in TableBaseHeroesOnlyPlayable)
        {
            TableBaseHeroesWithNotPlayable.Add(i.Key, i.Value);
        }
        foreach (BaseHero? i in db.BaseHeroes.AsNoTracking().Where(a => !a.isPlayable))
        {
            TableBaseHeroesWithNotPlayable.Add(i.id, i);
        }


        TableCreatureTypes = db.CreatureTypes.AsNoTracking().ToDictionary(a => a.id);
        TableDamageTypes = db.DamageTypes.AsNoTracking().ToDictionary(a => a.id);
        TableEquipmentTypes = db.EquipmentTypes.AsNoTracking().ToDictionary(a => a.id);
        TableMaterialDamagePercents = db.MaterialDamagePercents.AsNoTracking().ToDictionary(a => a.id);
        TableSlotTypes = db.SlotTypes.AsNoTracking().ToDictionary(a => a.id);
        TableSlots = db.Slots.AsNoTracking().ToDictionary(a => a.id);
        TableSmithingMaterials = db.SmithingMaterials.AsNoTracking().ToDictionary(a => a.id);
        TableX_EquipmentTypes_DamageTypes = db.x_EquipmentTypes_DamageTypes.AsNoTracking().ToDictionary(a => a.id);
        TableX_Heroes_CreatureTypes = db.x_Heroes_CreatureTypes.AsNoTracking().ToDictionary(a => a.id);
        TableBattlefields = db.Battlefields.AsNoTracking().ToDictionary(a => a.id);
        TableX_Battlefields_BaseHeroes = db.x_Battlefields_BaseHeroes.AsNoTracking().ToDictionary(a => a.id);

        ThrowIfDataNotCorrect();

        DtoContainerGameData container = new()
        {
            baseEquipments = TableBaseEquipments.Values.AsEnumerable(),
            baseHeroes = TableBaseHeroesOnlyPlayable.Values.AsEnumerable(),

            creatureTypes = TableCreatureTypes.Values.AsEnumerable(),

            damageTypes = TableDamageTypes.Values.AsEnumerable(),

            equipmentTypes = TableEquipmentTypes.Values.AsEnumerable(),

            materialDamagePercents = TableMaterialDamagePercents.Values.AsEnumerable(),

            slotTypes = TableSlotTypes.Values.AsEnumerable(),

            smithingMaterials = TableSmithingMaterials.Values.AsEnumerable(),

            xEquipmentTypesDamageTypes = TableX_EquipmentTypes_DamageTypes.Values.AsEnumerable(),

            xHeroesCreatureTypes = TableX_Heroes_CreatureTypes.Values.AsEnumerable(),

            Slots = TableSlots.Values.AsEnumerable(),

            xBattlefieldNpc = TableX_Battlefields_BaseHeroes.Values.AsEnumerable(),

            battlefields = TableBattlefields.Values.AsEnumerable()
        };


        GameDataJson = JSON.Serialize(container);

        if (string.IsNullOrWhiteSpace(GameDataJson))
        {
            throw new InvalidOperationException("Кэш не инициализирован.");
        }

        foreach (KeyValuePair<int, BaseEquipment> kv in TableBaseEquipments)
        {
            BaseEquipment i = kv.Value;
            i.equipmentType = TableEquipmentTypes[i.equipmentTypeId];
            i.smithingMaterial = i.smithingMaterialId != null ? TableSmithingMaterials[i.smithingMaterialId.Value] : null;
        }

        foreach (KeyValuePair<int, EquipmentType> kv in TableEquipmentTypes)
        {
            EquipmentType i = kv.Value;
            i.slotType = TableSlotTypes[i.slotTypeId];
        }

        foreach (KeyValuePair<int, MaterialDamagePercent> kv in TableMaterialDamagePercents)
        {
            MaterialDamagePercent i = kv.Value;
            i.smithingMaterials = TableSmithingMaterials[i.smithingMaterialsId];
            i.damageType = TableDamageTypes[i.damageTypeId];
        }

        foreach (KeyValuePair<ESlot, Slot> kv in TableSlots)
        {
            Slot i = kv.Value;
            i.slotType = TableSlotTypes[i.slotTypeId];
        }

        foreach (KeyValuePair<int, X_EquipmentType_DamageType> kv in TableX_EquipmentTypes_DamageTypes)
        {
            X_EquipmentType_DamageType i = kv.Value;
            i.equipmentType = TableEquipmentTypes[i.equipmentTypeId];
            i.damageType = TableDamageTypes[i.damageTypeId];
        }

        foreach (KeyValuePair<int, X_Hero_CreatureType> kv in TableX_Heroes_CreatureTypes)
        {
            X_Hero_CreatureType i = kv.Value;
            i.baseHero = TableBaseHeroesOnlyPlayable[i.baseHeroId];
            i.creatureType = TableCreatureTypes[i.creatureTypeId];
        }

        foreach (KeyValuePair<int, X_Battlefield_BaseHero> kv in TableX_Battlefields_BaseHeroes)
        {
            X_Battlefield_BaseHero i = kv.Value;
            i.baseHero = TableBaseHeroesOnlyPlayable[i.baseHeroId];
            i.battlefield = TableBattlefields[i.battlefieldId];
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
        foreach (EBattleFiled i in Enum.GetValues<EBattleFiled>())
        {
            if (!TableBattlefields.Values.Any(a => a.enumName == i.ToString()))
            {
                throw new Exception("Not correct data in table Battlefields and enum EBattleFiled");
            }
        }
    }
}
