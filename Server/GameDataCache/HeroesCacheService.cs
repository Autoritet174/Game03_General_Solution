// GameDataCache/HeroesCacheService.cs
using Microsoft.EntityFrameworkCore;
using Server_DB_Data;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Server.GameDataCache;

/// <summary>
/// Интерфейс сервиса кэширования данных о героях.
/// </summary>
public interface IHeroCacheService
{
    /// <summary>
    /// JSON-строка со всеми героями.
    /// </summary>
    string HeroesJson { get; }

    /// <summary>
    /// Инициализирует кэш данных о героях.
    /// </summary>
    /// <param name="serviceProvider">Провайдер служб для получения зависимостей.</param>
    Task InitializeAsync(IServiceProvider serviceProvider);
}

/// <summary>
/// Сервис для кэширования данных о героях в формате JSON.
/// </summary>
public class HeroesCacheService(ILogger<HeroesCacheService> logger) : IHeroCacheService
{
    /// <summary>
    /// Логгер для записи событий сервиса.
    /// </summary>
    private readonly ILogger<HeroesCacheService> _logger = logger;

    /// <summary>
    /// Кэшированная JSON-строка с данными о героях.
    /// </summary>
    private string? _json = null;

    /// <summary>
    /// Инициализирует кэш, загружая данные о героях из базы данных.
    /// </summary>
    /// <param name="serviceProvider">Провайдер служб для создания scope и получения DbContext.</param>
    /// <exception cref="InvalidOperationException">
    /// Возникает при невозможности получить DbContext из serviceProvider.
    /// </exception>
    public async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        // Создаём временный scope, чтобы безопасно получить DbContext
        using IServiceScope scope = serviceProvider.CreateScope();
        DbContext_Game03Data _db = scope.ServiceProvider.GetRequiredService<DbContext_Game03Data>();

        // Загружаем данные о героях из базы данных (без отслеживания для производительности)
        var data = await _db.Heroes
            .AsNoTracking()  // Отключаем отслеживание изменений для увеличения производительности
            .Select(h => new
            {
                h.Id,
                h.Name,
                h.Rarity,
                h.Health,
                h.Damage,
            })
            .ToListAsync();  // Асинхронное выполнение запроса и преобразование в список

        // Создаём JSON-массив для хранения героев
        JsonArray jsonArray = [];

        // Преобразуем каждого героя в JSON-объект и добавляем в массив
        foreach (var item in data)
        {
            JsonObject obj = new()
            {
                ["id"] = item.Id,
                ["name"] = item.Name,
                ["rarity"] = (int)item.Rarity,  // Приведение enum к int для сериализации
                ["health"] = item.Health,
                ["damage"] = item.Damage,
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
            WriteIndented = false  // Отключаем форматирование для уменьшения объема данных
        });

        // Логируем успешную загрузку данных в кэш
        //_logger.LogInformation($"[{nameof(HeroesCacheService)}] Герои успешно загружены из базы в кеш.");
    }

    /// <summary>
    /// Возвращает JSON-строку с данными о героях.
    /// </summary>
    /// <returns>JSON-строка со всеми героями.</returns>
    /// <exception cref="InvalidOperationException">
    /// Возникает, если кэш не был инициализирован.
    /// </exception>
    public string HeroesJson => _json
        ?? throw new InvalidOperationException("Кэш героев не инициализирован. Вызовите InitializeAsync перед использованием.");
}
