// GameDataCache/HeroesCacheService.cs
using Microsoft.EntityFrameworkCore;
using Server_DB_Data;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Server.GameDataCache;


public interface IHeroCacheService
{

    string HeroesJson { get; }

    Task InitializeAsync(IServiceProvider serviceProvider);
}


public class HeroesCacheService(ILogger<HeroesCacheService> logger) : IHeroCacheService
{

    private readonly ILogger<HeroesCacheService> _logger = logger;


    private string? _json = null;


    public async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        // Создаём временный scope, чтобы безопасно получить DbContext
        using IServiceScope scope = serviceProvider.CreateScope();
        DbContext_Game03Data _db = scope.ServiceProvider.GetRequiredService<DbContext_Game03Data>();

        // Загружаем данные о героях из базы данных (без отслеживания для производительности)
        var data = await _db.Heroes
            .AsNoTracking()
            .Select(h => new
            {
                h.Id,
                h.Name,
                h.Rarity,
                h.BaseHealth,
                h.BaseAttack,
            })
            .ToListAsync();

        // Создаём JSON-массив для хранения героев
        JsonArray jsonArray = [];

        // Преобразуем каждого героя в JSON-объект и добавляем в массив
        foreach (var item in data)
        {
            JsonObject obj = new()
            {
                ["id"] = item.Id,
                ["name"] = item.Name,
                ["rarity"] = (int)item.Rarity,
                ["baseHealth"] = (int)item.BaseHealth,
                ["baseAttack"] = (int)item.BaseAttack,
            };

            jsonArray.Add(obj);
        }

        // Оборачиваем массив в корневой JSON-объект с ключом "heroes"
        JsonObject root = new()
        {
            ["heroes"] = jsonArray
        };

        // Сериализуем в строку без отступов для уменьшения размера
        _json = root.ToJsonString(new JsonSerializerOptions
        {
            WriteIndented = false
        });

        // Логируем успешную загрузку данных в кэш
        //_logger.LogInformation($"[{nameof(HeroesCacheService)}] Герои успешно загружены из базы в кеш.");
    }


    public string HeroesJson => _json
        ?? throw new InvalidOperationException("Кэш героев не инициализирован. Вызовите InitializeAsync перед использованием.");
}
