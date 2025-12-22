using Game03Client.GameData;
using Game03Client.HttpRequester;
using Game03Client.Logger;
using General;
using General.DTO.Entities;
using General.DTO.Entities.Collection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using L = General.LocalizationKeys;

namespace Game03Client.PlayerCollection;

internal class PlayerCollectionProvider(
    PlayerCollectionCache playerCollectionCache,
    GameDataCache gameDataCache,
    IHttpRequester _httpRequester,
    ILogger _logger
    ) : IPlayerCollection
{
    #region Logger
    private const string NAME_THIS_CLASS = nameof(PlayerCollectionProvider);
    private void Log(string message, string? keyLocal = null)
    {
        if (!keyLocal.IsEmpty())
        {
            message = $"{message}; {L.KEY_LOCALIZATION}:<{keyLocal}>";
        }

        _logger.LogEx(NAME_THIS_CLASS, message);
    }

    [DoesNotReturn]
    private void LogAndThrow(string message, string? keyLocal = null) {
        try
        {
            Log(message, keyLocal);
        }
        catch { }
        throw new Exception(message);
    }
    #endregion Logger

    public async Task<bool> LoadAllCollectionFromServerAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }

        // Получить коллекцию героев игрока
        string? response = await _httpRequester.GetResponseAsync(General.Url.Collection.All, cancellationToken);
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
            i.DtoBaseEquipment = gameDataCache.DtoContainer.DtoBaseEquipments.FirstOrDefault(a => a.Id == i.BaseEquipmentId);
        }

        foreach (DtoHero i in c.DtoCollectionHeroes)
        {
            i.DtoBaseHero = gameDataCache.DtoContainer.DtoBaseHeroes.FirstOrDefault(a => a.Id == i.BaseHeroId);
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
            i.DtoEquipment13 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment13Id);
            i.DtoEquipment14 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment14Id);
            i.DtoEquipment15 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment15Id);
            i.DtoEquipment16 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment16Id);
            i.DtoEquipment17 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment17Id);
            i.DtoEquipment18 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment18Id);
            i.DtoEquipment19 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment19Id);
            i.DtoEquipment20 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment20Id);
            i.DtoEquipment21 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment21Id);
            i.DtoEquipment22 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment22Id);
            i.DtoEquipment23 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment23Id);
            i.DtoEquipment24 = c.DtoCollectionEquipments.FirstOrDefault(a => a.Id == i.Equipment24Id);
        }

        playerCollectionCache.collection = c;

        RefreshListGroupNameHero();
        RefreshListGroupNameEquipment();
        return true;
    }

    public void RefreshListGroupNameHero()
    {
        List<string> list = playerCollectionCache.listGroupNameHero;
        list.Clear();
        list.Add(string.Empty); // группа по умолчанию
        foreach (DtoHero i in playerCollectionCache.collection.DtoCollectionHeroes)
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
        List<string> list = playerCollectionCache.listGroupNameEquipment;
        list.Clear();
        list.Add(string.Empty); // группа по умолчанию
        foreach (DtoEquipment i in playerCollectionCache.collection.DtoCollectionEquipments)
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
        return playerCollectionCache.collection.DtoCollectionHeroes;
    }

    public int GetCountHeroes()
    {
        return playerCollectionCache.collection.DtoCollectionHeroes.Count;
    }

    /// <summary>
    /// Получить коллекцию героев сгруппированную по именам групп.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<GroupCollectionElement> GetCollectionHeroesGroupedByGroupNames()
    {
        List<GroupCollectionElement> result = [];
        foreach (string groupName in playerCollectionCache.listGroupNameHero)
        {
            IEnumerable<DtoHero> heroes;
            if (groupName == string.Empty)
            {
                heroes = playerCollectionCache.collection.DtoCollectionHeroes.Where(a => a.GroupName is null or "");
            }
            else {
                heroes = playerCollectionCache.collection.DtoCollectionHeroes.Where(a => a.GroupName == groupName);
            }

            heroes = heroes.OrderByDescending(a => a.Rarity)
                .ThenBy(a => a.Level)
                .ThenBy(a =>
                {
                    if (a.DtoBaseHero == null)
                    {
                        LogAndThrow("a.DtoBaseHero is null");
                    }
                    return a.DtoBaseHero.Name;
                });

            List<CollectionElement> collectionElements = [];
            foreach (DtoHero hero in heroes)
            {
                if (hero.DtoBaseHero == null)
                {
                    LogAndThrow("hero.DtoBaseHero is null");
                }
                collectionElements.Add(new CollectionElement(hero.Id, hero.BaseHeroId, hero.Rarity, hero.DtoBaseHero.Name, "hero"));
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

    public IEnumerable<GroupCollectionElement> GetCollectionEquipmentesGroupByGroups()
    {
        List<GroupCollectionElement> result = [];
        foreach (string groupName in playerCollectionCache.listGroupNameEquipment)
        {
            IEnumerable<DtoEquipment> equipments;
            if (groupName == string.Empty)
            {
                equipments = playerCollectionCache.collection.DtoCollectionEquipments.Where(a => a.GroupName is null or "");
            }
            else
            {
                equipments = playerCollectionCache.collection.DtoCollectionEquipments.Where(a => a.GroupName == groupName);
            }

            equipments.OrderByDescending(a =>
                {
                    if (a.DtoBaseEquipment == null)
                    {
                        LogAndThrow("a.DtoBaseEquipment is null");
                    }
                    return a.DtoBaseEquipment.Rarity;
                }).ThenBy(a =>
                {
                    if (a.DtoBaseEquipment == null)
                    {
                        LogAndThrow("a.DtoBaseEquipment is null");
                    }
                    return a.DtoBaseEquipment.Name;
                });

            List<CollectionElement> collectionElements = [];
            foreach (DtoEquipment Equipment in equipments)
            {
                if (Equipment.DtoBaseEquipment == null)
                {
                    LogAndThrow("Equipment.DtoBaseEquipment is null");
                }
                collectionElements.Add(new CollectionElement(Equipment.Id, Equipment.BaseEquipmentId, Equipment.DtoBaseEquipment.Rarity, Equipment.DtoBaseEquipment.Name, "Equipment"));
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
