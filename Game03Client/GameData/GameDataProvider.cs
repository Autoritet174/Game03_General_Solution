using General.DTO;
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

    public IEnumerable<DtoBaseHero> BaseHeroes => globalFunctionsProviderCache.BaseHeroes;
    public IEnumerable<DtoSlotType> SlotTypes => globalFunctionsProviderCache.SlotTypes;
    public IEnumerable<DtoEquipmentType> EquipmentTypes => globalFunctionsProviderCache.EquipmentTypes;
    public IEnumerable<DtoBaseEquipment> BaseEquipments => globalFunctionsProviderCache.BaseEquipments;

    /// <inheritdoc/>
    public async Task LoadGameData(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        JObject? jObject = await httpRequesterProvider.GetJObjectAsync(Url.General.GameData, cancellationToken);
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

        List<DtoBaseHero> allHeroes = [];
        foreach (JObject heroObj in heroesArray.Cast<JObject>())
        {
            JToken? t_id = heroObj["id"];
            int id = t_id != null ? (int)t_id : 0;
            string? name = heroObj["name"]?.ToString();
            if (name == null)
            {
                Log("name == null");
                continue;
            }
            float baseHealth = (float)Convert.ToDouble(heroObj["baseHealth"]);
            float baseAttack = (float)Convert.ToDouble(heroObj["baseAttack"]);
            int rarity = Convert.ToInt32(heroObj["rarity"]);
            allHeroes.Add(new BaseHero(id, name, rarity, baseHealth, baseAttack));
        }

        globalFunctionsProviderCache.BaseHeroes = allHeroes.AsEnumerable();
    }

    /// <inheritdoc/>
    public DtoBaseHero GetHeroById(int id)
    {
        return globalFunctionsProviderCache.BaseHeroes.FirstOrDefault(a => a.Id == id);
    }

    Task IGameData.LoadGameData(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    DtoBaseHero IGameData.GetHeroById(int id)
    {
        throw new NotImplementedException();
    }
}
