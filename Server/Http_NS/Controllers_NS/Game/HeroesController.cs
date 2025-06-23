using Dapper;
using General;
using General.DataBaseModels;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
namespace Server.Http_NS.Controllers_NS.Game;

/// <summary>
/// Контроллер для получения списка всех героев. Использует кэш в памяти.
/// </summary>
public class HeroesController : ControllerBaseApi {

    /// <summary>
    /// Глобальный кэшированный список героев.
    /// </summary>
    private static List<HeroStats>? _cachedHeroes;

    public static void Init() {
        _cachedHeroes = GetHeroesFromDatabase().Result;
    }

    /// <summary>
    /// Обрабатывает POST-запрос "GetAll" и возвращает кэшированный список всех героев.
    /// </summary>
    /// <returns>JSON-объект с данными по умолчанию и списком героев.</returns>
    [HttpPost("GetAll")]
    public IActionResult GetAll() {
        var response = new {
            Status = "Success",
            Timestamp = DateTime.UtcNow,
            Heroes = _cachedHeroes
        };
        
        return Ok(response);
    }

    /// <summary>
    /// метод получения данных о героях из базы данных.
    /// </summary>
    /// <returns>Список героев.</returns>
    private static async Task<List<HeroStats>> GetHeroesFromDatabase() {
        using MySqlConnection connection = new(DataBase.ConnectionString_GameData);
        await connection.OpenAsync();
        IEnumerable<HeroStats> heroes = await connection.QueryAsync<HeroStats>(HeroStats.Sql);
        return [.. heroes];
    }
}
