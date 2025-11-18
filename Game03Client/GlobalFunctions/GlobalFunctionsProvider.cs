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
using static General.Enums;
using L = General.LocalizationKeys;

namespace Game03Client.GlobalFunctions;

internal class GlobalFunctionsProvider(IHttpRequesterProvider httpRequesterProvider, GlobalFunctionsProviderCache globalFunctionsProviderCache, ILoggerProvider logger) : IGlobalFunctionsProvider
{
    #region Logger
    private readonly ILoggerProvider _logger = logger;
    private const string NAME_THIS_CLASS = nameof(GlobalFunctionsProvider);
    private void Log(string message, string? keyLocal = null)
    {
        if (!keyLocal.IsEmpty)
        {
            message = $"{message}; {L.KEY_LOCALIZATION}:<{keyLocal}>";
        }

        _logger.LogEx(NAME_THIS_CLASS, message);
    }
    #endregion Logger

    public IEnumerable<HeroBaseEntity> AllHeroes => globalFunctionsProviderCache._allHeroes;
    public async Task LoadListAllHeroesAsync(CancellationToken cancellationToken)
    {
        JObject? jObject = await httpRequesterProvider.GetJObjectAsync(General.Url.General.ListAllHeroes, cancellationToken);
        if (jObject == null)
        {
            return;
        }

        JToken? heroesToken = jObject["heroes"];
        if (heroesToken is not JArray heroesArray)
        {
            Log("heroesToken is not JArray heroesArray");
            return;
        }

        List<HeroBaseEntity> allHeroes = [];
        foreach (JObject heroObj in heroesArray.Cast<JObject>())
        {
            Guid id = new(heroObj["id"]?.ToString());
            string? name = heroObj["name"]?.ToString();
            if (name == null)
            {
                Log("name == null");
                continue;
            }
            float baseHealth = (float)Convert.ToDouble(heroObj["baseHealth"]);
            float baseAttack = (float)Convert.ToDouble(heroObj["baseAttack"]);
            var rarity = (RarityLevel)Convert.ToInt32(heroObj["rarity"]);
            allHeroes.Add(new HeroBaseEntity(id, name, rarity, baseHealth, baseAttack));
        }

        globalFunctionsProviderCache._allHeroes = allHeroes.AsEnumerable();

        //globalFunctionsProviderCache.AllHeroes.Sort(static (a, b) =>
        //{
        //    int result = b.Rarity.CompareTo(a.Rarity);
        //    if (result != 0)
        //    {// сортировка по редкости по убыванию
        //        return result;
        //    }

        //    // сортировка по имени по возрастанию
        //    return a.Name.CompareTo(b.Name);
        //});
    }

    public HeroBaseEntity GetHeroById(Guid guid)
    {
        return globalFunctionsProviderCache._allHeroes.First(a => a.Id == guid);
    }
    //public IEnumerable<HeroBaseEntity> GetData() {
    //    return globalFunctionsProviderCache.AllHeroes.AsEnumerable();
    //}
}
