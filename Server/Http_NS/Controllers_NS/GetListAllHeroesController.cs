using Microsoft.AspNetCore.Mvc;
using Server.GameDataCache;

namespace Server.Http_NS.Controllers_NS;

/// <summary>
/// Контроллер для аутентификации пользователей.
/// </summary>
public class GetListAllHeroesController(IHeroCacheService heroCache) : ControllerBaseApi
{
    private static ContentResult? result = null;
    private readonly IHeroCacheService _heroCache = heroCache;

    /// <summary>
    /// Возвращает в ответе список всех героев.
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> GetListAllHeroes()
    {
        await GF.DelayWithOutDebug500();
        //result ??= Content(ListAllHeroes.Json, "application/json");
        result ??= Content(_heroCache.HeroesJson, "application/json");
        return result;
    }
}
