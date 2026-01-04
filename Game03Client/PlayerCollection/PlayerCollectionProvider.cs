using Game03Client.GameData;
using Game03Client.HttpRequester;
using Game03Client.Logger;
using General.DTO.Entities;
using General.DTO.Entities.Collection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        string? response = await httpRequester.GetResponseAsync(General.Url.Collection.All, cancellationToken, jwtToken: jwtToken);
        if (response == null)
        {
            return false;
        }
        DtoContainerCollection? dto_container = JsonConvert.DeserializeObject<DtoContainerCollection>(response);
        if (dto_container == null)
        {
            return false;
        }
        DtoContainerCollection c = dto_container;

        foreach (DtoEquipment i in c.DtoCollectionEquipments)
        {
            i.DtoBaseEquipment = gameDataProvider.DtoContainer.DtoBaseEquipments.FirstOrDefault(a => a.Id == i.BaseEquipmentId);
        }

        foreach (DtoHero i in c.DtoCollectionHeroes)
        {
            i.DtoBaseHero = gameDataProvider.DtoContainer.DtoBaseHeroes.FirstOrDefault(a => a.Id == i.BaseHeroId);
            i.DtoEquipment1 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment1Id);
            i.DtoEquipment2 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment2Id);
            i.DtoEquipment3 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment3Id);
            i.DtoEquipment4 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment4Id);
            i.DtoEquipment5 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment5Id);
            i.DtoEquipment6 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment6Id);
            i.DtoEquipment7 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment7Id);
            i.DtoEquipment8 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment8Id);
            i.DtoEquipment9 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment9Id);
            i.DtoEquipment10 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment10Id);
            i.DtoEquipment11 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment11Id);
            i.DtoEquipment12 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment12Id);
            //i.DtoEquipment13 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment13Id);
            //i.DtoEquipment14 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment14Id);
            //i.DtoEquipment15 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment15Id);
            //i.DtoEquipment16 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment16Id);
            //i.DtoEquipment17 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment17Id);
            //i.DtoEquipment18 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment18Id);
            //i.DtoEquipment19 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment19Id);
            //i.DtoEquipment20 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment20Id);
            //i.DtoEquipment21 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment21Id);
            //i.DtoEquipment22 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment22Id);
            //i.DtoEquipment23 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment23Id);
            //i.DtoEquipment24 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment24Id);
        }

        collection = c;

        RefreshListGroupNameHero();
        RefreshListGroupNameEquipment();

        // Сортировка героев по редкости, уровню и имени
        c.DtoCollectionHeroes = [.. c.DtoCollectionHeroes
            .OrderByDescending(a => a.Rarity)
            .ThenBy(a => a.Level)
            .ThenBy(a =>
            {
                if (a.DtoBaseHero == null)
                {
                    logger.LogAndThrow("a.DtoBaseHero is null");
                }
                return a.DtoBaseHero.Name;
            })];

        // Сортировка экипировки по редкости и имени

        c.DtoCollectionEquipments = [.. c.DtoCollectionEquipments
            .OrderByDescending(a =>
            {
                if (a.DtoBaseEquipment == null)
                {
                    logger.LogAndThrow("a.DtoBaseEquipment is null");
                }
                return a.DtoBaseEquipment.Rarity;
            })
            .ThenBy(a =>
            {
                if (a.DtoBaseEquipment == null)
                {
                    logger.LogAndThrow("a.DtoBaseEquipment is null");
                }
                return a.DtoBaseEquipment.Name;
            })];

        return true;
    }

    public void RefreshListGroupNameHero()
    {
        List<string> list = listGroupNameHero;
        list.Clear();
        list.Add(string.Empty);
        foreach (DtoHero i in collection.DtoCollectionHeroes)
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
        foreach (DtoEquipment i in collection.DtoCollectionEquipments)
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
        return collection.DtoCollectionHeroes;
    }
    public IEnumerable<DtoEquipment> GetCollectionEquipmentsFromCache()
    {
        return collection.DtoCollectionEquipments;
    }

    public int GetCountHeroes()
    {
        return collection.DtoCollectionHeroes.Count;
    }
    public int GetCountEquipments()
    {
        return collection.DtoCollectionEquipments.Count;
    }

    public const int PAGE_SIZE = 100;

    /// <summary> Получить коллекцию героев сгруппированную по именам групп. </summary>
    public IEnumerable<GroupCollectionElement> GetCollectionHeroesGroupedByGroupNames(int page)
    {
        List<GroupCollectionElement> result = [];
        List<DtoHero> c = collection.DtoCollectionHeroes;
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
                if (hero.DtoBaseHero == null)
                {
                    logger.LogAndThrow("hero.DtoBaseHero is null");
                }
                collectionElements.Add(new CollectionElement(hero.Id, hero.BaseHeroId, hero.Rarity, hero.DtoBaseHero.Name, hero.DtoBaseHero.IsUnique, TypeCollectionElement.Hero));
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
        List<DtoEquipment> c = collection.DtoCollectionEquipments;
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
                if (equipment.DtoBaseEquipment == null)
                {
                    logger.LogAndThrow("Equipment.DtoBaseEquipment is null");
                }
                collectionElements.Add(new CollectionElement(equipment.Id, equipment.BaseEquipmentId, equipment.DtoBaseEquipment.Rarity, equipment.DtoBaseEquipment.Name, equipment.DtoBaseEquipment.IsUnique, TypeCollectionElement.Equipment));
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
