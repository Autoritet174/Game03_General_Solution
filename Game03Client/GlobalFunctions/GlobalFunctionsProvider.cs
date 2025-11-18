using Game03Client.HttpRequester;
using General.GameEntities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static General.Enums;

namespace Game03Client.GlobalFunctions;

internal class GlobalFunctionsProvider(IHttpRequesterProvider httpRequesterProvider, GlobalFunctionsProviderCache globalFunctionsProviderCache) : IGlobalFunctionsProvider
{

    public IEnumerable<HeroBaseEntity> AllHeroes => globalFunctionsProviderCache._allHeroes;

    private static void Error(string error)
    {
        Console.WriteLine($"[{nameof(GlobalFunctionsProvider)}] {error}");
    }
    public async Task LoadListAllHeroes()
    {
        HttpRequesterResult? httpRequesterResult = await httpRequesterProvider.GetResponceAsync(General.Url.General.ListAllHeroes);
        if (httpRequesterResult == null)
        {
            Error("httpRequesterResult == null");
            return;
        }
        JObject? jObject = httpRequesterResult.JObject;
        if (jObject == null)
        {
            Error("jObject == null");
            return;
        }

        JToken? heroesToken = jObject["heroes"];
        if (heroesToken is not JArray heroesArray)
        {
            Error("heroesToken is not JArray heroesArray");
            return;
        }

        List<HeroBaseEntity> allHeroes = [];
        foreach (JObject heroObj in heroesArray.Cast<JObject>())
        {
            Guid id = new(heroObj["id"]?.ToString());
            string? name = heroObj["name"]?.ToString();
            if (name == null)
            {
                Error("name == null");
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
