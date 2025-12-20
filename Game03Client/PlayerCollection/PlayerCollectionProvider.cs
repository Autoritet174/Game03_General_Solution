using Game03Client.GameData;
using Game03Client.HttpRequester;
using Game03Client.Logger;
using General;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using L = General.LocalizationKeys;

namespace Game03Client.PlayerCollection;

internal class PlayerCollectionProvider(PlayerCollectionCache _collectionCache, IHttpRequester _httpRequester, ILogger _logger, IGameData _gameData) : IPlayerCollection
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
    #endregion Logger

    public async Task<bool> LoadAllCollectionFromServer(CancellationToken cancellationToken)
    {

        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }

        // Получить коллекцию героев игрока
        JObject? jObject = await _httpRequester.GetJObjectAsync(General.Url.Collection.All, cancellationToken);
        if (jObject == null)
        {
            Log("jObject == null");
            return false;
        }

        // герои
        JToken? heroes = jObject["heroes"];
        if (heroes != null)
        {
            _collectionCache.listHero.Clear();
            foreach (JToken a in heroes)
            {
                Guid id = a["id"].GetGuid();
                Guid userId = a["userId"].GetGuid();
                int baseHeroId = a["baseHeroId"].GetInt();
                string groupName = a["groupName"].GetString();
                int rarity = a["rarity"].GetInt();
                long health = a["health"].GetLong();
                long attack = a["attack"].GetLong();
                int strength = a["strength"].GetInt();
                int agility = a["agility"].GetInt();
                int intelligence = a["intelligence"].GetInt();
                int haste = a["haste"].GetInt();
                int level = a["level"].GetInt();
                long experience = a["experience"].GetInt();


                DtoCollectionHero cHero = new(id, userId, _gameData.GetHeroById(baseHeroId), groupName, rarity, health, attack,
                    strength, agility, intelligence, haste, level, experience);
                _collectionCache.listHero.Add(cHero);
            }
            RefreshListGroupName();
        }

        // экипировка
        JToken? equipment = jObject["equipment"];
        if (equipment != null)
        {
            _collectionCache.listEquipment.Clear();
            foreach (JToken a in equipment)
            {
                Guid id = a["id"].GetGuid();
                Guid userId = a["userId"].GetGuid();
                int baseEquipmentId = a["baseEquipmentId"].GetInt();
                string groupName = a["groupName"].GetString();
                int rarity = a["rarity"].GetInt();
                long health = a["health"].GetLong();
                long attack = a["attack"].GetLong();
                int strength = a["strength"].GetInt();
                int agility = a["agility"].GetInt();
                int intelligence = a["intelligence"].GetInt();
                int haste = a["haste"].GetInt();
                int level = a["level"].GetInt();
                //long experience = a["experience"].GetInt();


                DtoCollectionEquipment cEquipment = new(id, userId, _gameData.GetHeroById(baseHeroId), groupName, rarity, health, attack,
                    strength, agility, intelligence, haste, level, experience);
                _collectionCache.listEquipment.Add(cEquipment);
            }
            RefreshListGroupName();
        }

        return true;
    }

    public void RefreshListGroupName()
    {
        // Создать список уникальных групп
        List<string> list = _collectionCache.listHeroGroupName;
        list.Clear();
        list.Add(string.Empty); // группа по умолчанию
        foreach (DtoCollectionHero hero in _collectionCache.listHero)
        {
            string group_name = hero.GroupName;
            if (!list.Contains(group_name))
            {
                list.Add(group_name);
            }
        }
    }

    public IEnumerable<DtoCollectionHero> GetCollectionHeroesFromCache()
    {
        return _collectionCache.listHero;
    }
    public int GetCountHeroes()
    {
        return _collectionCache.listHero.Count;
    }

    /// <summary>
    /// Получить коллекцию героев сгруппированную по именам групп.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<GroupCollectionElement> GetCollectionHeroesGroupedByGroupNames()
    {
        List<GroupCollectionElement> result = [];
        foreach (string groupName in _collectionCache.listHeroGroupName)
        {
            IOrderedEnumerable<DtoCollectionHero> heroes = _collectionCache.listHero.Where(a => a.GroupName == groupName).OrderByDescending(a => a.Rarity).ThenBy(a => a.Level).ThenBy(a => a.HeroBase.Name);

            List<CollectionElement> collectionElements = [];
            foreach (DtoCollectionHero? hero in heroes)
            {
                collectionElements.Add(new CollectionElement(hero.Id, hero.HeroBase.Id, hero.Rarity, hero.HeroBase.Name, "hero"));
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

    //public IEnumerable<GroupCollectionElement> GetCollectionHeroesGroupByGroups()
    //{
    //    throw new NotImplementedException();
    //}
}
