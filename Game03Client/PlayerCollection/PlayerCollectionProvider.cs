using Game03Client.GameData;
using Game03Client.HttpRequester;
using Game03Client.Logger;
using General;
using General.GameEntities;
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

    /// <summary>
    /// 
    /// </summary>
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

        JToken? heroes = jObject["heroes"];
        if (heroes == null)
        {
            Log("heroes == null");
            return false;
        }

        _collectionCache.listHero.Clear();
        foreach (JToken h in heroes)
        {
            string id = h["_id"].GetString();
            Guid owner_id = h["OwnerId"].GetGuid();
            int hero_id = h["HeroId"].GetInt();
            string group_name = h["GroupName"].GetString();
            long health = h["Health"].GetLong();
            long attack = h["Attack"].GetLong();
            long strength = h["Str"].GetLong();
            long agility = h["Agi"].GetLong();
            long intelligence = h["Int"].GetLong();
            long haste = h["Haste"].GetLong();
            int level = h["Level"].GetInt();


            CollectionHero cHero = new(id, owner_id, _gameData.GetHeroById(hero_id), group_name, health, attack,
                strength, agility, intelligence, haste, level);
            _collectionCache.listHero.Add(cHero);
        }


        RefreshListGroupName();

        return true;
    }

    public void RefreshListGroupName()
    {
        // Создать список уникальных групп
        List<string> list = _collectionCache.listGroupName;
        list.Clear();
        list.Add(string.Empty); // группа по умолчанию
        foreach (CollectionHero hero in _collectionCache.listHero)
        {
            string group_name = hero.GroupName;
            if (!list.Contains(group_name))
            {
                list.Add(group_name);
            }
        }
    }

    public IEnumerable<CollectionHero> GetCollectionHeroesFromCache()
    {
        return _collectionCache.listHero;
    }
    public int GetCountHeroes() {
        return _collectionCache.listHero.Count;
    }

    /// <summary>
    /// Получить коллекцию героев сгруппированную по именам групп.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<GroupHeroes> GetCollectionHeroesGroupByGroups()
    {
        List<GroupHeroes> result = [];
        foreach (string groupName in _collectionCache.listGroupName)
        {
            GroupHeroes groupHeroes = new(groupName,
                _collectionCache.listHero.Where(a => a.GroupName == groupName).OrderByDescending(a => a.HeroBase.Rarity).ThenBy(a => a.Level).ThenBy(a => a.HeroBase.Name));
            result.Add(groupHeroes);
            if (groupName == string.Empty)
            {
                groupHeroes.Priority = -1;
            }
        }
        return result.OrderByDescending(a=>a.Priority);
    }
}
