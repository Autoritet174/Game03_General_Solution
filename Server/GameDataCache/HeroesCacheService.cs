// GameDataCache/HeroesCacheService.cs
using Microsoft.EntityFrameworkCore;
using Server_DB_Data;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Server.GameDataCache;

/// <summary>
/// Интерфейс для сервиса кэширования данных о героях.
/// </summary>
/// <remarks>
/// Реализация этого интерфейса должна обеспечивать инициализацию кэша и предоставлять 
/// доступ к данным о героях в формате JSON.
/// </remarks>
public interface IHeroCacheService
{
    /// <summary>
    /// Возвращает строку в формате JSON, содержащую данные о героях.
    /// </summary>
    /// <returns>Строка JSON с массивом героев под ключом "heroes".</returns>
    /// <exception cref="InvalidOperationException">Вызывается, если кэш не был инициализирован.</exception>
    /// <example>
    /// <code>
    /// string json = heroCacheService.HeroesJson;
    /// Console.WriteLine(json);
    /// </code>
    /// </example>
    string HeroesJson { get; }

    /// <summary>
    /// Асинхронно инициализирует кэш героев, загружая данные из базы данных.
    /// </summary>
    /// <returns>Задача (Task), представляющая асинхронную операцию инициализации.</returns>
    /// <remarks>
    /// Метод должен быть вызван перед первым использованием <see cref="HeroesJson"/>.
    /// Без инициализации доступ к <see cref="HeroesJson"/> вызовет исключение.
    /// </remarks>
    /// <example>
    /// <code>
    /// await heroCacheService.InitializeAsync();
    /// </code>
    /// </example>
    Task InitializeAsync();
}

/// <summary>
/// Сервис для кэширования данных о героях в формате JSON.
/// </summary>
/// <remarks>
/// Данный класс загружает данные о героях из базы данных <see cref="DbContext_Game03Data"/>
/// и кэширует их в виде строки JSON для быстрого доступа.
/// Использует <see cref="JsonObject"/> и <see cref="JsonArray"/> для формирования структуры.
/// Данные кэшируются один раз при инициализации и не обновляются в течение жизни приложения.
/// <para>
/// Сервис использует <see cref="ILogger{HeroesCacheService}"/> для записи информации о процессе инициализации.
/// Логирование помогает отслеживать успешную загрузку данных или диагностировать проблемы при старте приложения.
/// </para>
/// </remarks>
public class HeroesCacheService(DbContext_Game03Data db, ILogger<HeroesCacheService> logger) : IHeroCacheService
{
    /// <summary>
    /// Экземпляр контекста базы данных, используемого для получения данных о героях.
    /// </summary>
    private readonly DbContext_Game03Data _db = db;

    /// <summary>
    /// Средство записи логов для диагностики и наблюдения за работой сервиса.
    /// </summary>
    private readonly ILogger<HeroesCacheService> _logger = logger;

    /// <summary>
    /// Хранит сериализованную строку JSON с данными о героях. Значение <see langword="null"/> означает, что кэш не инициализирован.
    /// </summary>
    private string? _json = null;

    /// <summary>
    /// Инициализирует кэш, загружая данные о героях из базы данных и преобразуя их в формат JSON.
    /// </summary>
    /// <returns>Асинхронная задача без возвращаемого значения.</returns>
    /// <exception cref="Exception">Возможны исключения при выполнении запроса к базе данных (например, таймаут, ошибка подключения).</exception>
    /// <remarks>
    /// Данные загружаются с использованием <see cref="EntityFrameworkQueryableExtensions.AsNoTracking{TEntity}(IQueryable{TEntity})"/>
    /// для повышения производительности.
    /// <para>
    /// После успешной загрузки и сериализации данных, в журнал записывается информационное сообщение
    /// о том, что герои были загружены в кэш.
    /// </para>
    /// Формируется JSON-объект вида:
    /// <code>
    /// { "heroes": [ { "id": 1, "name": "Hero1", "rarity": 3, "baseHealth": 100, "baseAttack": 20 }, ... ] }
    /// </code>
    /// </remarks>
    /// <example>
    /// <code>
    /// var service = new HeroesCacheService(dbContext, logger);
    /// await service.InitializeAsync();
    /// </code>
    /// </example>
    public async Task InitializeAsync()
    {
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
        _logger.LogInformation($"[{nameof(HeroesCacheService)}] Герои успешно загружены из базы в кеш.");
    }

    /// <summary>
    /// Возвращает кэшированную строку JSON с данными о героях.
    /// </summary>
    /// <returns>Строка JSON, содержащая данные о героях.</returns>
    /// <exception cref="InvalidOperationException">
    /// Вызывается, если метод <see cref="InitializeAsync"/> не был вызван до обращения к свойству.
    /// </exception>
    /// <remarks>
    /// Данные возвращаются в формате:
    /// <code>
    /// { "heroes": [ { "id": 1, "name": "Герой", "rarity": 3, "baseHealth": 100, "baseAttack": 20 }, ... ] }
    /// </code>
    /// </remarks>
    /// <example>
    /// <code>
    /// string json = heroCacheService.HeroesJson;
    /// Console.WriteLine(json);
    /// </code>
    /// </example>
    public string HeroesJson => _json
        ?? throw new InvalidOperationException("Кэш героев не инициализирован. Вызовите InitializeAsync перед использованием.");
}
