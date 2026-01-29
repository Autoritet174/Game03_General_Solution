using General;
using General.DTO.Entities;
using General.DTO.Entities.GameData;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    public IEnumerable<UserBanReason> TableUserBanReasons { get; private set; } = null!;
    public IEnumerable<UserSessionInactivationReason> TableUserSessionInactivationReasons { get; private set; } = null!;
    public IEnumerable<BaseEquipment> TableBaseEquipment { get; private set; } = null!;
    public IEnumerable<BaseHero> TableBaseHeroes { get; private set; } = null!;
    public IEnumerable<CreatureType> TableCreatureTypes { get; private set; } = null!;
    public IEnumerable<DamageType> TableDamageTypes { get; private set; } = null!;
    public IEnumerable<EquipmentType> TableEquipmentTypes { get; private set; } = null!;
    public IEnumerable<MaterialDamagePercent> TableMaterialDamagePercents { get; private set; } = null!;
    public IEnumerable<SlotType> TableSlotTypes { get; private set; } = null!;
    public IEnumerable<Slot> TableSlots { get; private set; } = null!;
    public IEnumerable<SmithingMaterial> TableSmithingMaterials { get; private set; } = null!;
    public IEnumerable<X_EquipmentType_DamageType> TableX_EquipmentTypes_DamageTypes { get; private set; } = null!;
    public IEnumerable<X_Hero_CreatureType> TableX_Heroes_CreatureTypes { get; private set; } = null!;

    /// <summary>
    /// Загрузка константных данных в оперативную память. Эти данные меняются только при не работающем сервере.
    /// </summary>
    /// <param name="db"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task LoadServerDataAsync(DbContextGame db, CancellationToken cancellationToken = default)
    {
        TableUserBanReasons = await db.UserBanReasons.AsNoTracking().ToListAsync(cancellationToken);

        await LoadTableUserSessionInactivationReasons(db, cancellationToken);


         TableBaseEquipment = await db.BaseEquipments.AsNoTracking().ToListAsync(cancellationToken);
        List<DtoBaseEquipment> baseEquipments = [.. TableBaseEquipment.Select(static h => new DtoBaseEquipment(
            h.Id, h.Name, h.Rarity, h.IsUnique, h.EquipmentTypeId, h.Health, h.Damage)
            )];

        TableBaseHeroes = await db.BaseHeroes.AsNoTracking().ToListAsync(cancellationToken);
        List<DtoBaseHero> baseHeroes = [.. TableBaseHeroes.Select(static h => new DtoBaseHero(
            h.Id, h.Name, h.Rarity, h.IsUnique, h.MainStat, h.Health, h.Damage)
            )];

        TableCreatureTypes = await db.CreatureTypes.AsNoTracking().ToListAsync(cancellationToken);
        List<DtoCreatureType> creatureTypes = [..TableCreatureTypes.Select(static h => new DtoCreatureType(
            h.Id, h.Name)
            )];

        TableDamageTypes = await db.DamageTypes.AsNoTracking().ToListAsync(cancellationToken);
        List<DtoDamageType> damageTypes = [..TableDamageTypes.Select(static h => new DtoDamageType(
            h.Id, h.Name)
            )];

        TableEquipmentTypes = await db.EquipmentTypes.AsNoTracking().ToListAsync(cancellationToken);
        List<DtoEquipmentType> equipmentType = [..TableEquipmentTypes.Select(static h => new DtoEquipmentType(
            h.Id, h.Name, h.MassPhysical, h.MassMagical, h.SlotTypeId, h.CanCraftSmithing, h.CanCraftJewelcrafting, h.SpendActionPoints, h.BlockOtherHand)
           )];

        TableMaterialDamagePercents = await db.MaterialDamagePercents.AsNoTracking().ToListAsync(cancellationToken);
        List<DtoMaterialDamagePercent> materialDamagePercents = [..TableMaterialDamagePercents.Select(static h => new DtoMaterialDamagePercent(
            h.Id, h.SmithingMaterialsId, h.DamageTypeId, h.Percent)
            )];

        TableSlotTypes = await db.SlotTypes.AsNoTracking().ToListAsync(cancellationToken);
        List<DtoSlotType> slotTypes = [..TableSlotTypes.Select(static h => new DtoSlotType(
            h.Id, h.Name)
           )];

        TableSlots = await db.Slots.AsNoTracking().ToListAsync(cancellationToken);
        List<DtoSlot> slots = [..TableSlots.Select(static h => new DtoSlot(
            h.Id, h.Name, h.SlotTypeId)
           )];

        TableSmithingMaterials = await db.SmithingMaterials.AsNoTracking().ToListAsync(cancellationToken);
        List<DtoSmithingMaterial> smithingMaterials = [..TableSmithingMaterials.Select(static h => new DtoSmithingMaterial(
            h.Id, h.Name)
           )];

        TableX_EquipmentTypes_DamageTypes = await db.x_EquipmentTypes_DamageTypes.AsNoTracking().ToListAsync(cancellationToken);
        List<DtoXEquipmentTypeDamageType> xEquipmentTypesDamageTypes = [..TableX_EquipmentTypes_DamageTypes.Select(static h => new DtoXEquipmentTypeDamageType(
            h.EquipmentTypeId, h.DamageTypeId, h.DamageCoef)
           )];

        TableX_Heroes_CreatureTypes = await db.x_Heroes_CreatureTypes.AsNoTracking().ToListAsync(cancellationToken);
        List<DtoXHeroCreatureType> xHeroesCreatureTypes = [..TableX_Heroes_CreatureTypes.Select(static h => new DtoXHeroCreatureType(
            h.BaseHeroId, h.CreatureTypeId)
           )];

        DtoContainerGameData container = new(baseEquipments, baseHeroes, creatureTypes, damageTypes, equipmentType, materialDamagePercents, slotTypes, smithingMaterials, xEquipmentTypesDamageTypes, xHeroesCreatureTypes, slots);

        //GameDataJson = Newtonsoft.Json.JsonConvert.SerializeObject(container, General.GlobalHelper.JsonSerializerSettings);
        //GameDataJson = JsonSerializer.Serialize(container, GlobalJsonOptions.jsonOptions);
        GameDataJson = JsonSerializer.Serialize(container, GlobalJsonContext.Default.DtoContainerGameData);
        if (string.IsNullOrWhiteSpace(GameDataJson))
        {
            throw new InvalidOperationException("Кэш не инициализирован.");
        }
    }

    private async Task LoadTableUserSessionInactivationReasons(DbContextGame db, CancellationToken cancellationToken = default) {
        bool reload = false;

        async Task Load() {
            TableUserSessionInactivationReasons = await db.UserSessionInactivationReasons.AsNoTracking().ToListAsync(cancellationToken);
        }

        await Load();
        foreach (InactivationReason item in Enum.GetValues<InactivationReason>())
        {
            if (!TableUserSessionInactivationReasons.Any(a => a.Code == item))
            {
                var entity = new UserSessionInactivationReason
                {
                    Name = item.ToString(),
                    Code = item
                };
                await db.UserSessionInactivationReasons.AddAsync(entity, cancellationToken);
                reload = true;
            }
        }

        if (reload)
        {
            await db.SaveChangesAsync(cancellationToken);
            await Load();
        }
    }

    public int GetInactivationReasonIdByCode(InactivationReason code)
    {
        return TableUserSessionInactivationReasons.First(a => a.Code == code).Id;
    }
}
