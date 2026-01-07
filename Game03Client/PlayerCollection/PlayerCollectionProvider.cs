using Game03Client.GameData;
using Game03Client.HttpRequester;
using Game03Client.Logger;
using General.DTO.Entities;
using General.DTO.Entities.Collection;
using General.DTO.Entities.GameData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Game03Client.PlayerCollection;

public class PlayerCollectionProvider(
    GameDataProvider gameDataProvider,
    HttpRequesterProvider httpRequester,
    LoggerProvider<PlayerCollectionProvider> logger
    )
{

    private readonly List<string> listGroupNameHero = [];
    private readonly List<string> listGroupNameEquipment = [];
    private DtoContainerCollection collection = null!;
    public async Task<bool> LoadAllCollectionFromServerAsync(CancellationToken cancellationToken, string jwtToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }

        // Получить коллекцию героев игрока
        string? response = await httpRequester.GetResponseAsync(General.Url.Collection.All, cancellationToken, jwtToken: jwtToken) ?? throw new ArgumentNullException();
        DtoContainerCollection c = JsonConvert.DeserializeObject<DtoContainerCollection>(response) ?? throw new ArgumentNullException();

        IEnumerable<DtoBaseEquipment> baseEquipments = gameDataProvider.Container.BaseEquipments;
        foreach (DtoEquipment i in c.CollectionEquipments)
        {
            i.BaseEquipment = baseEquipments.FirstOrDefault(a => a.Id == i.BaseEquipmentId);
        }

        IEnumerable<DtoBaseHero> baseHeroes = gameDataProvider.Container.BaseHeroes;
        foreach (DtoHero i in c.CollectionHeroes)
        {
            i.BaseHero = baseHeroes.FirstOrDefault(a => a.Id == i.BaseHeroId);
        }

        IEnumerable<DtoSlot> slots = gameDataProvider.Container.Slots;
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
                    logger.LogAndThrow("a.DtoBaseHero is null");
                }
                return a.BaseHero.Name;
            })];

        // Сортировка экипировки по редкости и имени

        c.CollectionEquipments = [.. c.CollectionEquipments
            .OrderByDescending(a =>
            {
                if (a.BaseEquipment == null)
                {
                    logger.LogAndThrow("a.DtoBaseEquipment is null");
                }
                return a.BaseEquipment.Rarity;
            })
            .ThenBy(a =>
            {
                if (a.BaseEquipment == null)
                {
                    logger.LogAndThrow("a.DtoBaseEquipment is null");
                }
                return a.BaseEquipment.Name;
            })];

        return true;
    }

    public void RefreshListGroupNameHero()
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

    public void RefreshListGroupNameEquipment()
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

    public IEnumerable<DtoHero> GetCollectionHeroesFromCache()
    {
        return collection.CollectionHeroes;
    }
    public IEnumerable<DtoEquipment> GetCollectionEquipmentsFromCache()
    {
        return collection.CollectionEquipments;
    }

    public int GetCountHeroes()
    {
        return collection.CollectionHeroes.Count();
    }
    public int GetCountEquipments()
    {
        return collection.CollectionEquipments.Count();
    }

    public const int PAGE_SIZE = 100;

    /// <summary> Получить коллекцию героев сгруппированную по именам групп. </summary>
    public IEnumerable<GroupCollectionElement> GetCollectionHeroesGroupedByGroupNames(int page)
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
                    logger.LogAndThrow("hero.DtoBaseHero is null");
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
    public IEnumerable<GroupCollectionElement> GetCollectionEquipmentesGroupByGroups(int page)
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
                    logger.LogAndThrow("Equipment.DtoBaseEquipment is null");
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
