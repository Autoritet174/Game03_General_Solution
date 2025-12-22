using Game03Client.HttpRequester;
using Game03Client.Logger;
using General;
using General.DTO.Entities;
using General.DTO.Entities.GameData;
using Newtonsoft.Json;
using System;
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


    /// <inheritdoc/>
    public async Task LoadGameData(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        string? response = await httpRequesterProvider.GetResponseAsync(Url.General.GameData, cancellationToken);
        if (response == null)
        {
            return;
        }
        DtoContainerGameData? dto_container = JsonConvert.DeserializeObject<DtoContainerGameData>(response);
        if (dto_container == null)
        {
            return;
        }
        DtoContainerGameData c = dto_container;

        foreach (DtoBaseEquipment i in c.DtoBaseEquipments)
        {
            i.DtoEquipmentType = c.DtoEquipmentTypes.FirstOrDefault(a => a.Id == i.DtoEquipmentTypeId);
        }
        foreach (DtoEquipmentType i in c.DtoEquipmentTypes)
        {
            i.DtoSlotType = c.DtoSlotTypes.FirstOrDefault(a => a.Id == i.DtoSlotTypeId);
        }
        foreach (DtoMaterialDamagePercent i in c.DtoMaterialDamagePercents)
        {
            i.DtoSmithingMaterial = c.DtoSmithingMaterials.FirstOrDefault(a => a.Id == i.DtoSmithingMaterialId);
            i.DtoDamageType = c.DtoDamageTypes.FirstOrDefault(a => a.Id == i.DtoDamageTypeId);
        }
        globalFunctionsProviderCache.DtoContainer = c;




        //JToken? heroesToken = jObject["heroes"];
        //if (heroesToken is not JArray heroesArray)
        //{
        //    Log("heroesToken is not JArray heroesArray");
        //    return;
        //}

        //List<DtoBaseHero> allHeroes = [];
        //foreach (JObject heroObj in heroesArray.Cast<JObject>())
        //{
        //    JToken? t_id = heroObj["id"];
        //    int id = t_id != null ? (int)t_id : 0;
        //    string? name = heroObj["name"]?.ToString();
        //    if (name == null)
        //    {
        //        Log("name == null");
        //        continue;
        //    }
        //    float baseHealth = (float)Convert.ToDouble(heroObj["baseHealth"]);
        //    float baseAttack = (float)Convert.ToDouble(heroObj["baseAttack"]);
        //    int rarity = Convert.ToInt32(heroObj["rarity"]);
        //    allHeroes.Add(new DtoBaseHero(id, name, rarity,false, 0, null));
        //}

        //globalFunctionsProviderCache.BaseHeroes = allHeroes.AsEnumerable();
    }
    public DtoContainerGameData GetDtoContainer()
    {
        return globalFunctionsProviderCache.DtoContainer;
    }
}
