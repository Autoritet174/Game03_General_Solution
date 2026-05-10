using General;
using General.DTO.Entities;
using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.Collection;

public class CollectionProvider
{

    private static readonly List<string> listGroupNameHero = [];
    private static readonly List<string> listGroupNameEquipment = [];
    private static DtoContainerCollection collection = null!;
    private static readonly Logger<CollectionProvider> logger = new();

    public static async Task<bool> LoadAllCollectionFromServerAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            logger.LogError("LoadAllCollectionFromServerAsync cancelled");
            return false;
        }

        // Получить коллекцию героев игрока
        string? response = await HttpRequester.GetResponseAsync(General.Url.Collection.ALL, null, cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(response))
        {
            logger.LogError("response is null or empty");
            return false;
        }
        DtoContainerCollection? c = JSON.Deserialize<DtoContainerCollection>(response);

        if (c == null)
        {
            logger.LogError("c is null");
            return false;
        }

        IEnumerable<BaseEquipment> baseEquipments = GameData.Container.BaseEquipments;
        foreach (Equipment i in c.CollectionEquipments)
        {
            i.BaseEquipment = baseEquipments.FirstOrDefault(a => a.Id == i.BaseEquipmentId);
        }

        IEnumerable<BaseHero> baseHeroes = GameData.Container.BaseHeroes;
        foreach (Hero i in c.CollectionHeroes)
        {
            i.BaseHero = baseHeroes.FirstOrDefault(a => a.Id == i.BaseHeroId);
        }

        collection = c;

        RefreshListGroupNameHero();
        RefreshListGroupNameEquipment();

        // Сортировка героев по редкости, уровню и имени
        c.CollectionHeroes.Sort((a, b) =>
        {
            // Сначала по Rarity (убывание)
            int result = b.BaseHero!.Rarity.CompareTo(a.BaseHero!.Rarity);
            if (result != 0) return result;

            // Затем по Level (возрастание)
            result = a.Level.CompareTo(b.Level);
            if (result != 0) return result;

            // Затем по Name (возрастание)
            if (a.BaseHero == null || b.BaseHero == null)
            {
                logger.LogError("BaseHero is null");
                throw new Exception();
            }

            return string.Compare(a.BaseHero.Name, b.BaseHero.Name, StringComparison.Ordinal);
        });

        return true;
    }

    public static void RefreshListGroupNameHero()
    {
        List<string> list = listGroupNameHero;
        list.Clear();
        list.Add(string.Empty);
        foreach (Hero i in collection.CollectionHeroes)
        {
            string group_name = i.GroupName ?? string.Empty;
            if (!list.Contains(group_name))
            {
                list.Add(group_name);
            }
        }
    }

    public static void RefreshListGroupNameEquipment()
    {
        List<string> list = listGroupNameEquipment;
        list.Clear();
        list.Add(string.Empty);
        foreach (Equipment i in collection.CollectionEquipments)
        {
            string group_name = i.GroupName ?? string.Empty;
            if (!list.Contains(group_name))
            {
                list.Add(group_name);
            }
        }
    }

    public static IEnumerable<Hero> GetCollectionHeroesFromCache() => collection.CollectionHeroes;
    public static IEnumerable<Equipment> GetCollectionEquipmentsFromCache() => collection.CollectionEquipments;

    public static int GetCountHeroes() => collection.CollectionHeroes.Count();
    public static int GetCountEquipments() => collection.CollectionEquipments.Count();

    public const int PAGE_SIZE = 100;

    /// <summary> Получить коллекцию героев сгруппированную по именам групп. </summary>
    public static IEnumerable<GroupCollectionElement> GetCollectionHeroesGroupedByGroupNames(int page)
    {
        List<GroupCollectionElement> result = [];
        IEnumerable<Hero> c = collection.CollectionHeroes;
        if (page > 0)
        {
            c = [.. c.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE)];
        }

        foreach (string groupName in listGroupNameHero)
        {
            IEnumerable<Hero> heroes = groupName == string.Empty ? c.Where(a => a.GroupName is null or "") : c.Where(a => a.GroupName == groupName);
            List<CollectionElement> collectionElements = [];
            foreach (Hero hero in heroes)
            {
                if (hero.BaseHero == null)
                {
                    logger.LogError("hero.DtoBaseHero is null");
                    throw new Exception();
                }
                collectionElements.Add(new CollectionElement(hero.Id, hero.BaseHeroId, hero.BaseHero.Rarity, hero.BaseHero.Name, hero.BaseHero.IsUnique, TypeCollectionElement.Hero));
            }

            GroupCollectionElement groupCollectionElement = new(groupName, collectionElements);
            result.Add(groupCollectionElement);
            if (groupName == string.Empty)
            {
                groupCollectionElement.Priority = -1;
            }
        }
        return result.OrderByDescending(a => a.Priority);
    }

    /// <summary> Получить коллекцию экипировки сгруппированную по именам групп. </summary>
    public static IEnumerable<GroupCollectionElement> GetCollectionEquipmentesGroupByGroups(int page)
    {
        List<GroupCollectionElement> result = [];
        collection.CollectionEquipments.Sort(DtoEquipmentComparer);
        IEnumerable<Equipment> c = collection.CollectionEquipments;
        if (page > 0)
        {
            c = [.. c.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE)];
        }
        foreach (string groupName in listGroupNameEquipment)
        {
            IEnumerable<Equipment> equipments = groupName == string.Empty ? c.Where(a => a.GroupName is null or "") : c.Where(a => a.GroupName == groupName);
            List<CollectionElement> collectionElements = [];
            foreach (Equipment equipment in equipments)
            {
                if (equipment.BaseEquipment == null)
                {
                    logger.LogError("Equipment.DtoBaseEquipment is null");
                    throw new Exception();
                }
                collectionElements.Add(new CollectionElement(equipment.Id, equipment.BaseEquipmentId, equipment.BaseEquipment.Rarity, equipment.BaseEquipment.Name, equipment.BaseEquipment.IsUnique, TypeCollectionElement.Equipment));
            }

            GroupCollectionElement groupCollectionElement = new(groupName, collectionElements);
            result.Add(groupCollectionElement);
            if (groupName == string.Empty)
            {
                groupCollectionElement.Priority = -1;
            }
        }
        return result.OrderByDescending(a => a.Priority);
    }

    public static bool EquipmentIsEquipped(Guid equipmentId)
    {
        Equipment? equipment = collection.CollectionEquipments.FirstOrDefault(a => a.Id == equipmentId);
        if (equipment == null)
        {
            logger.LogError("Equipment not found in collection. Id: {EquipmentId}", equipmentId.ToString());
            return false;
        }
        return equipment.HeroId != null;
    }

    public static async Task<bool> EquipmentTakeOnAsync(Guid equipmentId, Guid heroId, bool? inAltSlot, CancellationToken cancellationToken)
    {
        Equipment? equipment = collection.CollectionEquipments.FirstOrDefault(a => a.Id == equipmentId);
        if (equipment == null)
        {
            logger.LogError("Equipment not found in collection. Id: {EquipmentId}", equipmentId.ToString());
            return false;
        }
        Hero? hero = collection.CollectionHeroes.FirstOrDefault(a => a.Id == heroId);
        if (hero == null)
        {
            logger.LogError("Hero not found in collection. Id: {heroId}", heroId.ToString());
            return false;
        }

        if (equipment.HeroId != null)
        {
            logger.LogError("Equipment is not equipped. Id: {EquipmentId}", equipmentId.ToString());
            return false;
        }

        try
        {
            bool success = await WebSocketProvider.InvokeAsync<bool>(
                General.HubMethodNames.EMethod.EQUIPMENT_TAKE_ON,
                cancellationToken,
                heroId,
                equipmentId,
                inAltSlot
            ).ConfigureAwait(false);

            if (success)
            {
                equipment.HeroId = heroId;
                equipment.SlotId = GetSlotId(equipment, inAltSlot);
                return true;
            }
        }
        catch (OperationCanceledException)
        {
            // отмена операции
        }
        catch (Exception)
        {
            // Обработка ошибок (таймаут, разрыв соединения, ошибка хаба)
            // Логирование и проброс дальше или возврат false
        }

        return false;
    }

    public static General.ESlot GetSlotId(Equipment equipment, bool? inAltSlot = null)
    {
        General.ESlotType slotTypeId = equipment.BaseEquipment?.EquipmentType?.SlotType?.Id ?? 0;
        return slotTypeId switch
        {
            General.ESlotType.Weapon => inAltSlot == true ? General.ESlot.LeftHand : General.ESlot.RightHand,     // Оружие
            General.ESlotType.Ring => inAltSlot == true ? General.ESlot.Ring2 : General.ESlot.Ring1,    // Кольцо
            General.ESlotType.Trinket => inAltSlot == true ? General.ESlot.Trinket2 : General.ESlot.Trinket1,  // Аксессуар
            _ => GameData.Container.Slots.First(a => a.SlotTypeId == slotTypeId).Id
        };
        /*
        return slotTypeId switch
        {
            General.SlotType.Weapon => inAltSlot ? General.Slot.LeftHand : General.Slot.RightHand,     // Оружие
            General.SlotType.Ring => inAltSlot ? General.Slot.Ring2 : General.Slot.Ring1,    // Кольцо
            General.SlotType.Trinket => inAltSlot ? General.Slot.Trinket2 : General.Slot.Trinket1,  // Аксессуар
            _ => cacheService.TableSlots.First(a => a.SlotTypeId == slotTypeId).Id
        };
        */
    }

    public static async Task<bool> EquipmentTakeOffAsync(Guid equipmentId, CancellationToken cancellationToken)
    {
        Equipment? equipment = collection.CollectionEquipments.FirstOrDefault(a => a.Id == equipmentId);
        if (equipment == null)
        {
            logger.LogError("Equipment not found in collection. Id: {EquipmentId}", equipmentId.ToString());
            return false;
        }

        // Сразу возвращаем успех если предмет и так не одет
        if (equipment.HeroId == null)
        {
            return true;
        }

        try
        {
            bool success = await WebSocketProvider.InvokeAsync<bool>(
                General.HubMethodNames.EMethod.EQUIPMENT_TAKE_OFF,
                cancellationToken,
                equipmentId
                ).ConfigureAwait(false);
            if (success)
            {
                equipment.HeroId = null;
                equipment.SlotId = null;
                return true;
            }
        }
        catch (OperationCanceledException)
        {
            // отмена операции
        }
        catch (Exception)
        {
            // Обработка ошибок (таймаут, разрыв соединения, ошибка хаба)
            // Логирование и проброс дальше или возврат false
        }
        return false;
    }


    private static readonly Comparer<Equipment> DtoEquipmentComparer = Comparer<Equipment>.Create(static (a, b) =>
    {
        BaseEquipment aBE = a.BaseEquipment ?? throw new Exception("a.BaseEquipment is null");
        BaseEquipment bBE = b.BaseEquipment ?? throw new Exception("b.BaseEquipment is null");
        EquipmentType aET = aBE.EquipmentType ?? throw new Exception("a.EquipmentType is null");
        EquipmentType bET = bBE.EquipmentType ?? throw new Exception("b.EquipmentType is null");
        SlotType aST = aET.SlotType ?? throw new Exception("a.SlotType is null");
        SlotType bST = bET.SlotType ?? throw new Exception("b.SlotType is null");

        // Сортировка по SlotType.Sorting
        int slotCompare = aST.Sorting.CompareTo(bST.Sorting);
        if (slotCompare != 0)
        {
            return slotCompare;
        }

        // Сортировка по Rarity (по убыванию)
        int rarityCompare = bBE.Rarity.CompareTo(aBE.Rarity);
        if (rarityCompare != 0)
        {
            return rarityCompare;
        }

        // Сортировка по IsUnique. Сначала true, потом false
        int uniqueCompare = bBE.IsUnique.CompareTo(aBE.IsUnique);
        if (uniqueCompare != 0)
        {
            return uniqueCompare;
        }

        // Сортировка по уровню (от большего к меньшему)
        int levelCompare = b.Level.CompareTo(a.Level);
        if (levelCompare != 0)
        {
            return levelCompare;
        }

        // Сортировка по Name
        return string.Compare(aBE.Name, bBE.Name, StringComparison.Ordinal);
    });
}
