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
    public async Task LoadServerDataAsync(DbContextGame db, CancellationToken cancellationToken)
    {
        TableUserBanReasons = await db.UserBanReasons.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);

        await LoadTableUserSessionInactivationReasonsAsync(db, cancellationToken).ConfigureAwait(false);


        TableBaseEquipment = await db.BaseEquipments.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
        List<DtoBaseEquipment> baseEquipments = [.. TableBaseEquipment.Select(static h => new DtoBaseEquipment(
            h.Id, h.Name, h.Rarity, h.IsUnique, h.EquipmentTypeId, h.Health, h.Damage)
            )];

        TableBaseHeroes = await db.BaseHeroes.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
        List<DtoBaseHero> baseHeroes = [.. TableBaseHeroes.Select(static h => new DtoBaseHero(
            h.Id, h.Name, h.Rarity, h.IsUnique, h.MainStat, h.Health, h.Damage)
            )];

        TableCreatureTypes = await db.CreatureTypes.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
        List<DtoCreatureType> creatureTypes = [..TableCreatureTypes.Select(static h => new DtoCreatureType(
            h.Id, h.Name)
            )];

        TableDamageTypes = await db.DamageTypes.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
        List<DtoDamageType> damageTypes = [..TableDamageTypes.Select(static h => new DtoDamageType(
            h.Id, h.Name)
            )];

        TableEquipmentTypes = await db.EquipmentTypes.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
        List<DtoEquipmentType> equipmentType = [..TableEquipmentTypes.Select(static h => new DtoEquipmentType(
            h.Id, h.Name, h.MassPhysical, h.MassMagical, h.SlotTypeId, h.CanCraftSmithing, h.CanCraftJewelcrafting, h.SpendActionPoints, h.BlockOtherHand)
           )];

        TableMaterialDamagePercents = await db.MaterialDamagePercents.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
        List<DtoMaterialDamagePercent> materialDamagePercents = [..TableMaterialDamagePercents.Select(static h => new DtoMaterialDamagePercent(
            h.Id, h.SmithingMaterialsId, h.DamageTypeId, h.Percent)
            )];

        TableSlotTypes = await db.SlotTypes.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
        List<DtoSlotType> slotTypes = [..TableSlotTypes.Select(static h => new DtoSlotType(
            h.Id, h.Name, h.HaveAltSlot)
           )];

        TableSlots = await db.Slots.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
        List<DtoSlot> slots = [..TableSlots.Select(static h => new DtoSlot(
            h.Id, h.Name, h.SlotTypeId)
           )];

        TableSmithingMaterials = await db.SmithingMaterials.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
        List<DtoSmithingMaterial> smithingMaterials = [..TableSmithingMaterials.Select(static h => new DtoSmithingMaterial(
            h.Id, h.Name)
           )];

        TableX_EquipmentTypes_DamageTypes = await db.x_EquipmentTypes_DamageTypes.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
        List<DtoXEquipmentTypeDamageType> xEquipmentTypesDamageTypes = [..TableX_EquipmentTypes_DamageTypes.Select(static h => new DtoXEquipmentTypeDamageType(
            h.EquipmentTypeId, h.DamageTypeId, h.DamageCoef)
           )];

        TableX_Heroes_CreatureTypes = await db.x_Heroes_CreatureTypes.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
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

        foreach (BaseEquipment i in TableBaseEquipment)
        {
            i.EquipmentType = TableEquipmentTypes.First(a => a.Id == i.EquipmentTypeId);
            i.SmithingMaterial = TableSmithingMaterials.First(a => a.Id == i.SmithingMaterialId);
        }
        foreach (BaseHero i in TableBaseHeroes)
        {

        }
        foreach (CreatureType i in TableCreatureTypes)
        {

        }
        foreach (DamageType i in TableDamageTypes)
        {

        }
        foreach (EquipmentType i in TableEquipmentTypes)
        {
            i.SlotType = TableSlotTypes.First(a => a.Id == i.SlotTypeId);
        }
        foreach (MaterialDamagePercent i in TableMaterialDamagePercents)
        {
            i.SmithingMaterials = TableSmithingMaterials.First(a => a.Id == i.SmithingMaterialsId);
            i.DamageType = TableDamageTypes.First(a => a.Id == i.DamageTypeId);
        }
        foreach (SlotType i in TableSlotTypes)
        {

        }
        foreach (Slot i in TableSlots)
        {
            i.SlotType = TableSlotTypes.First(a => a.Id == i.SlotTypeId);
        }
        foreach (SmithingMaterial i in TableSmithingMaterials)
        {

        }
        foreach (X_EquipmentType_DamageType i in TableX_EquipmentTypes_DamageTypes)
        {
            i.EquipmentType = TableEquipmentTypes.First(a => a.Id == i.EquipmentTypeId);
            i.DamageType = TableDamageTypes.First(a => a.Id == i.DamageTypeId);
        }
        foreach (X_Hero_CreatureType i in TableX_Heroes_CreatureTypes)
        {
            i.BaseHero = TableBaseHeroes.First(a => a.Id == i.BaseHeroId);
            i.CreatureType = TableCreatureTypes.First(a => a.Id == i.CreatureTypeId);
        }


    }

    private async Task LoadTableUserSessionInactivationReasonsAsync(DbContextGame db, CancellationToken cancellationToken)
    {
        bool reload = false;

        async Task LoadAsync()
        {
            TableUserSessionInactivationReasons = await db.UserSessionInactivationReasons.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        await LoadAsync().ConfigureAwait(false);
        foreach (InactivationReason item in Enum.GetValues<InactivationReason>())
        {
            if (!TableUserSessionInactivationReasons.Any(a => a.Code == item))
            {
                var entity = new UserSessionInactivationReason
                {
                    Name = item.ToString(),
                    Code = item
                };
                _ = await db.UserSessionInactivationReasons.AddAsync(entity, cancellationToken).ConfigureAwait(false);
                reload = true;
            }
        }

        if (reload)
        {
            _ = await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await LoadAsync().ConfigureAwait(false);
        }
    }

    public int GetInactivationReasonIdByCode(InactivationReason code)
    {
        return TableUserSessionInactivationReasons.First(a => a.Code == code).Id;
    }
}
