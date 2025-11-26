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

namespace Game03Client.GameData;

/// <summary>
/// Реализация <see cref="IGameData"/>, предоставляющая функциональность для
/// загрузки и доступа к глобальным игровым данным, таким как список героев.
/// </summary>
/// <param name="httpRequesterProvider">Провайдер для выполнения HTTP-запросов.</param>
/// <param name="globalFunctionsProviderCache">Кэш для хранения глобальных данных.</param>
/// <param name="logger">Провайдер для ведения журнала.</param>
internal class GameDataProvider(IHttpRequester httpRequesterProvider, GameDataCache globalFunctionsProviderCache, ILogger logger) : IGameData
{
    #region Logger
    private readonly ILogger _logger = logger;
    private const string NAME_THIS_CLASS = nameof(GameDataProvider);
    private void Log(string message, string? keyLocal = null)
    {
        if (!keyLocal.IsEmpty())
        {
            message = $"{message}; {L.KEY_LOCALIZATION}:<{keyLocal}>";
        }

        _logger.LogEx(NAME_THIS_CLASS, message);
    }
    #endregion Logger

    /// <inheritdoc/>
    public IEnumerable<HeroBase> AllHeroes => globalFunctionsProviderCache._allHeroes;

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">Выбрасывается, если произошла ошибка при конвертации данных (например, неверный формат).</exception>
    public async Task LoadListAllHeroesAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        JObject? jObject = await httpRequesterProvider.GetJObjectAsync(Url.General.ListAllHeroes, cancellationToken);
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

        List<HeroBase> allHeroes = [];
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
            allHeroes.Add(new HeroBase(id, name, rarity, baseHealth, baseAttack));
        }

        globalFunctionsProviderCache._allHeroes = allHeroes.AsEnumerable();
    }

    /// <inheritdoc/>
    public HeroBase GetHeroById(Guid guid)
    {
        return globalFunctionsProviderCache._allHeroes.FirstOrDefault(a => a.Id == guid);
    }
}
