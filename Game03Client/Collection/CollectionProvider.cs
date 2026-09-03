
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

    private static readonly Dictionary<Guid, Hero> dictonaryHeroes = [];
    private static readonly Dictionary<Guid, Equipment> dictonaryEquipments = [];

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


        IEnumerable<BaseEquipment> baseEquipments = GameData.Container.baseEquipments;
        dictonaryEquipments.Clear();
        foreach (Equipment i in c.collectionEquipments)
        {
            i.baseEquipment = baseEquipments.FirstOrDefault(a => a.id == i.baseEquipmentId);
            dictonaryEquipments.Add(i.id, i);
        }


        IEnumerable<BaseHero> baseHeroes = GameData.Container.baseHeroes;
        dictonaryHeroes.Clear();
        foreach (Hero i in c.collectionHeroes)
        {
            i.baseHero = baseHeroes.FirstOrDefault(a => a.id == i.baseHeroId);
            dictonaryHeroes.Add(i.id, i);
        }


        collection = c;

        RefreshListGroupNameHero();
        RefreshListGroupNameEquipment();

        // Сортировка героев по редкости, уровню и имени
        c.collectionHeroes.Sort((a, b) =>
        {
            // Сначала по Rarity (убывание)
            int result = b.baseHero!.rarity.CompareTo(a.baseHero!.rarity);
            if (result != 0)
            {
                return result;
            }

            // Затем по Level (возрастание)
            result = a.level.CompareTo(b.level);
            if (result != 0)
            {
                return result;
            }

            // Затем по Name (возрастание)
            if (a.baseHero == null || b.baseHero == null)
            {
                logger.LogError("BaseHero is null");
                throw new Exception();
            }

            return string.Compare(a.baseHero.name, b.baseHero.name, StringComparison.Ordinal);
        });





        return true;
    }

    public static void RefreshListGroupNameHero()
    {
        List<string> list = listGroupNameHero;
        list.Clear();
        list.Add(string.Empty);
        foreach (Hero i in collection.collectionHeroes)
        {
            string group_name = i.groupName ?? string.Empty;
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
        foreach (Equipment i in collection.collectionEquipments)
        {
            string group_name = i.groupName ?? string.Empty;
            if (!list.Contains(group_name))
            {
                list.Add(group_name);
            }
        }
    }

    public static IEnumerable<Hero> GetCollectionHeroesFromCache() => collection.collectionHeroes;
    public static IEnumerable<Equipment> GetCollectionEquipmentsFromCache() => collection.collectionEquipments;

    public static int GetCountHeroes() => collection.collectionHeroes.Count();
    public static int GetCountEquipments() => collection.collectionEquipments.Count();

    public const int PAGE_SIZE = 100;

    /// <summary> Получить коллекцию героев сгруппированную по именам групп. </summary>
    public static IEnumerable<GroupCollectionElement> GetCollectionHeroesGroupedByGroupNames(int page)
    {
        List<GroupCollectionElement> result = [];
        IEnumerable<Hero> c = collection.collectionHeroes;
        if (page > 0)
        {
            c = [.. c.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE)];
        }

        foreach (string groupName in listGroupNameHero)
        {
            IEnumerable<Hero> heroes = groupName == string.Empty ? c.Where(a => a.groupName is null or "") : c.Where(a => a.groupName == groupName);
            List<CollectionElement> collectionElements = [];
            foreach (Hero hero in heroes)
            {
                if (hero.baseHero == null)
                {
                    logger.LogError("hero.DtoBaseHero is null");
                    throw new Exception();
                }
                collectionElements.Add(new CollectionElement(hero.id, hero.baseHeroId, hero.baseHero.rarity, hero.baseHero.name, hero.baseHero.isUnique, TypeCollectionElement.Hero));
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
        collection.collectionEquipments.Sort(DtoEquipmentComparer);
        IEnumerable<Equipment> c = collection.collectionEquipments;
        if (page > 0)
        {
            c = [.. c.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE)];
        }
        foreach (string groupName in listGroupNameEquipment)
        {
            IEnumerable<Equipment> equipments = groupName == string.Empty ? c.Where(a => a.groupName is null or "") : c.Where(a => a.groupName == groupName);
            List<CollectionElement> collectionElements = [];
            foreach (Equipment equipment in equipments)
            {
                if (equipment.baseEquipment == null)
                {
                    logger.LogError("Equipment.DtoBaseEquipment is null");
                    throw new Exception();
                }
                collectionElements.Add(new CollectionElement(equipment.id, equipment.baseEquipmentId, equipment.baseEquipment.rarity, equipment.baseEquipment.name, equipment.baseEquipment.isUnique, TypeCollectionElement.Equipment));
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
        Equipment? equipment = collection.collectionEquipments.FirstOrDefault(a => a.id == equipmentId);
        if (equipment == null)
        {
            logger.LogError("Equipment not found in collection. Id: {EquipmentId}", equipmentId.ToString());
            return false;
        }
        return equipment.heroId != null;
    }

    public static async Task<bool> EquipmentTakeOnAsync(Guid equipmentId, Guid heroId, bool? inAltSlot, CancellationToken cancellationToken)
    {
        Equipment? equipment = collection.collectionEquipments.FirstOrDefault(a => a.id == equipmentId);
        if (equipment == null)
        {
            logger.LogError("Equipment not found in collection. Id: {EquipmentId}", equipmentId.ToString());
            return false;
        }
        Hero? hero = collection.collectionHeroes.FirstOrDefault(a => a.id == heroId);
        if (hero == null)
        {
            logger.LogError("Hero not found in collection. Id: {heroId}", heroId.ToString());
            return false;
        }

        if (equipment.heroId != null)
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
                equipment.heroId = heroId;
                equipment.slotId = GetSlotId(equipment, inAltSlot);
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
        General.ESlotType slotTypeId = equipment.baseEquipment?.equipmentType?.slotType?.id ?? 0;
        return slotTypeId switch
        {
            General.ESlotType.weapon => inAltSlot == true ? General.ESlot.leftHand : General.ESlot.rightHand,     // Оружие
            General.ESlotType.ring => inAltSlot == true ? General.ESlot.ring2 : General.ESlot.ring1,    // Кольцо
            General.ESlotType.trinket => inAltSlot == true ? General.ESlot.trinket2 : General.ESlot.trinket1,  // Аксессуар
            _ => GameData.Container.Slots.First(a => a.slotTypeId == slotTypeId).id
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
        Equipment? equipment = collection.collectionEquipments.FirstOrDefault(a => a.id == equipmentId);
        if (equipment == null)
        {
            logger.LogError("Equipment not found in collection. Id: {EquipmentId}", equipmentId.ToString());
            return false;
        }

        // Сразу возвращаем успех если предмет и так не одет
        if (equipment.heroId == null)
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
                equipment.heroId = null;
                equipment.slotId = null;
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

    public static Hero? GetHero(Guid id)
    {
        try
        {
            return dictonaryHeroes[id];
        }
        catch (Exception ex)
        {
            logger.LogException(ex);
            return null;
        }
    }

    public static Equipment? GetEquipment(Guid id)
    {
        try
        {
            return dictonaryEquipments[id];
        }
        catch (Exception ex)
        {
            logger.LogException(ex);
            return null;
        }
    }

    private static readonly Comparer<Equipment> DtoEquipmentComparer = Comparer<Equipment>.Create(static (a, b) =>
    {
        BaseEquipment aBE = a.baseEquipment ?? throw new Exception("a.BaseEquipment is null");
        BaseEquipment bBE = b.baseEquipment ?? throw new Exception("b.BaseEquipment is null");
        EquipmentType aET = aBE.equipmentType ?? throw new Exception("a.EquipmentType is null");
        EquipmentType bET = bBE.equipmentType ?? throw new Exception("b.EquipmentType is null");
        SlotType aST = aET.slotType ?? throw new Exception("a.SlotType is null");
        SlotType bST = bET.slotType ?? throw new Exception("b.SlotType is null");

        // Сортировка по SlotType.Sorting
        int slotCompare = aST.sorting.CompareTo(bST.sorting);
        if (slotCompare != 0)
        {
            return slotCompare;
        }

        // Сортировка по Rarity (по убыванию)
        int rarityCompare = bBE.rarity.CompareTo(aBE.rarity);
        if (rarityCompare != 0)
        {
            return rarityCompare;
        }

        // Сортировка по IsUnique. Сначала true, потом false
        int uniqueCompare = bBE.isUnique.CompareTo(aBE.isUnique);
        if (uniqueCompare != 0)
        {
            return uniqueCompare;
        }

        // Сортировка по уровню (от большего к меньшему)
        int levelCompare = b.level.CompareTo(a.level);
        if (levelCompare != 0)
        {
            return levelCompare;
        }

        // Сортировка по Name
        return string.Compare(aBE.name, bBE.name, StringComparison.Ordinal);
    });
}
