using General.DTO.Entities;
using General.DTO.Entities.GameData;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server_DB_Postgres;

namespace Server.GameDataCache;

public interface IGameDataCacheService
{
    string GameDataJson { get; }
    Task RefreshGameDataJsonAsync(DbContext_Game db, CancellationToken cancellationToken = default);
}
public class GameDataCacheService() : IGameDataCacheService
{
    //private readonly ILogger<GameDataCacheService> _logger = logger;

    /// <summary> Возвращает JSON-строку с данными. </summary>
    /// <returns> JSON-строка со всеми константными игровыми данными необходимыми на стороне клиента. </returns>
    /// <exception cref="InvalidOperationException">
    /// Возникает, если кэш не был инициализирован.
    /// </exception>
    public string GameDataJson { get => field ?? throw new InvalidOperationException("Кэш не инициализирован. Вызовите InitializeAsync перед использованием."); private set; }

    public async Task RefreshGameDataJsonAsync(DbContext_Game db, CancellationToken cancellationToken = default)
    {
        List<DtoBaseEquipment> baseEquipments = await db.BaseEquipments.AsNoTracking().Select(static h => new DtoBaseEquipment(
            h.Id, h.Name, h.Rarity, h.IsUnique, h.EquipmentTypeId, h.Stats)
            ).ToListAsync(cancellationToken);

        List<DtoBaseHero> baseHeroes = await db.BaseHeroes.AsNoTracking().Select(static h => new DtoBaseHero(
            h.Id, h.Name, h.Rarity, h.IsUnique, h.MainStat, h.Stats)
            ).ToListAsync(cancellationToken);

        List<DtoCreatureType> creatureTypes = await db.CreatureTypes.AsNoTracking().Select(static h => new DtoCreatureType(
            h.Id, h.Name)
            ).ToListAsync(cancellationToken);

        List<DtoDamageType> damageTypes = await db.DamageTypes.AsNoTracking().Select(static h => new DtoDamageType(
            h.Id, h.Name)
            ).ToListAsync(cancellationToken);

        List<DtoEquipmentType> equipmentType = await db.EquipmentTypes.AsNoTracking().Select(static h => new DtoEquipmentType(
            h.Id, h.Name, h.MassPhysical, h.MassMagical, h.SlotTypeId, h.CanCraftSmithing, h.CanCraftJewelcrafting, h.SpendActionPoints, h.BlockOtherHand)
           ).ToListAsync(cancellationToken);

        List<DtoMaterialDamagePercent> materialDamagePercents = await db.MaterialDamagePercents.AsNoTracking().Select(static h => new DtoMaterialDamagePercent(
            h.Id, h.SmithingMaterialsId, h.DamageTypeId, h.Percent)
            ).ToListAsync(cancellationToken);

        List<DtoSlotType> slotTypes = await db.SlotTypes.AsNoTracking().Select(static h => new DtoSlotType(
            h.Id, h.Name)
           ).ToListAsync(cancellationToken);

        List<DtoSmithingMaterial> smithingMaterials = await db.SmithingMaterials.AsNoTracking().Select(static h => new DtoSmithingMaterial(
            h.Id, h.Name)
           ).ToListAsync(cancellationToken);

        List<DtoXEquipmentTypeDamageType> xEquipmentTypesDamageTypes = await db.x_EquipmentTypes_DamageTypes.AsNoTracking().Select(static h => new DtoXEquipmentTypeDamageType(
            h.EquipmentTypeId, h.DamageTypeId, h.DamageCoef)
           ).ToListAsync(cancellationToken);

        List<DtoXHeroCreatureType> xHeroesCreatureTypes = await db.x_Heroes_CreatureTypes.AsNoTracking().Select(static h => new DtoXHeroCreatureType(
            h.BaseHeroId, h.CreatureTypeId)
           ).ToListAsync(cancellationToken);


        DtoContainerGameData container = new()
        {
            DtoBaseEquipments = baseEquipments,
            DtoBaseHeroes = baseHeroes,
            DtoCreatureTypes = creatureTypes,
            DtoDamageTypes = damageTypes,
            DtoEquipmentTypes = equipmentType,
            DtoMaterialDamagePercents = materialDamagePercents,
            DtoSlotTypes = slotTypes,
            DtoSmithingMaterials = smithingMaterials,
            DtoXEquipmentTypesDamageTypes = xEquipmentTypesDamageTypes,
            DtoXHeroesCreatureTypes = xHeroesCreatureTypes
        };

        GameDataJson = JsonConvert.SerializeObject(container, General.G.JsonSerializerSettings);
    }

}
