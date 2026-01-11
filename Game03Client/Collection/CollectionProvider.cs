using General.DTO.Entities;
using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using LOGGER = Game03Client.LOGGER<Game03Client.Collection.CollectionProvider>;

namespace Game03Client.Collection;

public class CollectionProvider
{

    private static readonly List<string> listGroupNameHero = [];
    private static readonly List<string> listGroupNameEquipment = [];
    private static DtoContainerCollection collection = null!;
    public static async Task<bool> LoadAllCollectionFromServerAsync(string accessToken, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }

        // Получить коллекцию героев игрока
        string? response = await HttpRequester.GetResponseAsync(General.Url.Collection.All, null, cancellationToken) ?? throw new ArgumentNullException();
        DtoContainerCollection c = JsonConvert.DeserializeObject<DtoContainerCollection>(response) ?? throw new ArgumentNullException();

        IEnumerable<DtoBaseEquipment> baseEquipments = GameData.Container.BaseEquipments;
        foreach (DtoEquipment i in c.CollectionEquipments)
        {
            i.BaseEquipment = baseEquipments.FirstOrDefault(a => a.Id == i.BaseEquipmentId);
        }

        IEnumerable<DtoBaseHero> baseHeroes = GameData.Container.BaseHeroes;
        foreach (DtoHero i in c.CollectionHeroes)
        {
            i.BaseHero = baseHeroes.FirstOrDefault(a => a.Id == i.BaseHeroId);
        }

        IEnumerable<DtoSlot> slots = GameData.Container.Slots;
        foreach (DtoEquipment i in c.CollectionEquipments)
        {
            i.Slot = slots.FirstOrDefault(a => a.Id == i.SlotId);
            i.Hero = c.CollectionHeroes.FirstOrDefault(a => a.Id == i.HeroId);
        }


        collection = c;

        RefreshListGroupNameHero();
        RefreshListGroupNameEquipment();

        // Сортировка героев по редкости, уровню и имени
        c.CollectionHeroes = [.. c.CollectionHeroes
            .OrderByDescending(a => a.Rarity)
            .ThenBy(a => a.Level)
            .ThenBy(a =>
            {
                if (a.BaseHero == null)
                {
                    LOGGER.LogError("a.DtoBaseHero is null");
                    throw new Exception();
                }
                return a.BaseHero.Name;
            })];

        // Сортировка экипировки по редкости и имени

        c.CollectionEquipments = [.. c.CollectionEquipments
            .OrderByDescending(a =>
            {
                if (a.BaseEquipment == null)
                {
                    LOGGER.LogError("a.DtoBaseEquipment is null");
            throw new Exception();
                }
                return a.BaseEquipment.Rarity;
            })
            .ThenBy(a =>
            {
                if (a.BaseEquipment == null)
                {
                    LOGGER.LogError("a.DtoBaseEquipment is null");
                    throw new Exception();
                }
                return a.BaseEquipment.Name;
            })];

        return true;
    }

    public static void RefreshListGroupNameHero()
    {
        List<string> list = listGroupNameHero;
        list.Clear();
        list.Add(string.Empty);
        foreach (DtoHero i in collection.CollectionHeroes)
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
        foreach (DtoEquipment i in collection.CollectionEquipments)
        {
            string group_name = i.GroupName ?? string.Empty;
            if (!list.Contains(group_name))
            {
                list.Add(group_name);
            }
        }
    }

    public static IEnumerable<DtoHero> GetCollectionHeroesFromCache() => collection.CollectionHeroes;
    public static IEnumerable<DtoEquipment> GetCollectionEquipmentsFromCache() => collection.CollectionEquipments;

    public static int GetCountHeroes() => collection.CollectionHeroes.Count();
    public static int GetCountEquipments() => collection.CollectionEquipments.Count();

    public const int PAGE_SIZE = 100;

    /// <summary> Получить коллекцию героев сгруппированную по именам групп. </summary>
    public static IEnumerable<GroupCollectionElement> GetCollectionHeroesGroupedByGroupNames(int page)
    {
        List<GroupCollectionElement> result = [];
        IEnumerable<DtoHero> c = collection.CollectionHeroes;
        if (page > 0)
        {
            c = [.. c.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE)];
        }

        foreach (string groupName in listGroupNameHero)
        {
            IEnumerable<DtoHero> heroes = groupName == string.Empty ? c.Where(a => a.GroupName is null or "") : c.Where(a => a.GroupName == groupName);
            List<CollectionElement> collectionElements = [];
            foreach (DtoHero hero in heroes)
            {
                if (hero.BaseHero == null)
                {
                    LOGGER.LogError("hero.DtoBaseHero is null");
                    throw new Exception();
                }
                collectionElements.Add(new CollectionElement(hero.Id, hero.BaseHeroId, hero.Rarity, hero.BaseHero.Name, hero.BaseHero.IsUnique, TypeCollectionElement.Hero));
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
        IEnumerable<DtoEquipment> c = collection.CollectionEquipments;
        if (page > 0)
        {
            c = [.. c.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE)];
        }
        foreach (string groupName in listGroupNameEquipment)
        {
            IEnumerable<DtoEquipment> equipments = groupName == string.Empty ? c.Where(a => a.GroupName is null or "") : c.Where(a => a.GroupName == groupName);
            List<CollectionElement> collectionElements = [];
            foreach (DtoEquipment equipment in equipments)
            {
                if (equipment.BaseEquipment == null)
                {
                    LOGGER.LogError("Equipment.DtoBaseEquipment is null");
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
}
