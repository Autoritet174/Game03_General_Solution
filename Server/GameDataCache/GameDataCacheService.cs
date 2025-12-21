using General.DTO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Server_DB_Postgres;

namespace Server.GameDataCache;

public interface IGameDataCacheService
{
    string GameDataJson { get; }

    Task RefreshGameDataJsonAsync(IServiceProvider serviceProvider);
}

public class GameDataCacheService(ILogger<GameDataCacheService> logger) : IGameDataCacheService
{
    private readonly ILogger<GameDataCacheService> _logger = logger;

    public async Task RefreshGameDataJsonAsync(IServiceProvider serviceProvider)
    {
        var _db = DbContext_Game.Create();

        //Task<List<DtoBaseHero>> heroes = _db.BaseHeroes.AsNoTracking().Select(static h =>
        //    new DtoBaseHero(h.Id, h.Name, h.Rarity, new Dice(h.Health), new Dice(h.Damage))
        //    ).ToListAsync();

        //Task<List<DtoSlotType>> slotType = _db.SlotTypes.AsNoTracking().Select(static h =>
        //   new DtoSlotType(h.Id, h.Name)
        //   ).ToListAsync();

        //Task<List<DtoBaseEquipment>> baseEquipment = _db.BaseEquipments.AsNoTracking().Select(static h =>
        //   new DtoBaseEquipment(h.Id, h.Name, h.Rarity, h.ma)
        //   ).ToListAsync();


        //DtoGameDataContainer container = new(await heroes, await slotType);

        // Настройки сериализации для Newtonsoft.Json
        var settings = new JsonSerializerSettings
        {
            // Использование CamelCase для всех полей, если атрибуты не заданы явно
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            // Компактный вид (аналог WriteIndented = false)
            Formatting = Formatting.None,
            // Игнорирование null значений для уменьшения веса JSON
            NullValueHandling = NullValueHandling.Ignore
        };

        //GameDataJson = JsonConvert.SerializeObject(container, settings);

        //if (_logger.IsEnabled(LogLevel.Information))
        //{
        //    _logger.LogInformation("Инициализация завершена. Использован Newtonsoft.Json. Загружено героев: {Count}", heroes.Count);
        //}
    }

    /// <summary> Возвращает JSON-строку с данными. </summary>
    /// <returns> JSON-строка со всеми константными игровыми данными необходимыми на стороне клиента. </returns>
    /// <exception cref="InvalidOperationException">
    /// Возникает, если кэш не был инициализирован.
    /// </exception>
    public string GameDataJson { get => field ?? throw new InvalidOperationException("Кэш не инициализирован. Вызовите InitializeAsync перед использованием."); private set; }
}
