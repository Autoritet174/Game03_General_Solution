using Game03Client.HttpRequester;
using Game03Client.IniFile;
using Game03Client.Logger;
using General;
using General.GameEntities;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using L = General.LocalizationKeys;

namespace Game03Client.PlayerCollection;

internal class PlayerCollectionProvider(PlayerCollectionCache _collectionCache, IIniFileProvider _iniFile, IHttpRequesterProvider _httpRequester, ILoggerProvider _logger) : IPlayerCollectionProvider
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
    public async Task LoadAllCollectionFromServer(CancellationToken cancellationToken) {

        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        // Получить коллекцию героев игрока
        JObject? jObject = await _httpRequester.GetJObjectAsync(General.Url.Collection.Heroes, cancellationToken);
        if (jObject == null)
        {
            Log("jObject == null");
            return;
        }

        JToken? heroes = jObject["heroes"];
        if (heroes == null)
        {
            Log("heroes == null");
            return;
        }

        // Перевести всех героев в C# коллекцию
        _collectionCache.listHero.Clear();
        foreach (JToken h in heroes)
        {
            string id = h["id"]?.ToString() ?? string.Empty;
            Guid owner_id = new(h["owner_id"]?.ToString());
            Guid hero_id = new(h["hero_id"]?.ToString());
            string group_name = h["group_name"]?.ToString()?.Trim() ?? string.Empty;
            long health = Convert.ToInt64(h["health"]?.ToString());
            long attack = Convert.ToInt64(h["attack"]?.ToString());
            long strength = Convert.ToInt64(h["strength"]?.ToString());
            long agility = Convert.ToInt64(h["agility"]?.ToString());
            long intelligence = Convert.ToInt64(h["intelligence"]?.ToString());
            long haste = Convert.ToInt64(h["haste"]?.ToString());

            CollectionHero cHero = new(id, owner_id, hero_id, group_name, health, attack, strength, agility, intelligence, haste);
            _collectionCache.listHero.Add(cHero);
        }

    }
}
