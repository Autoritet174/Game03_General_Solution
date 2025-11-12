using Microsoft.AspNetCore.Mvc;
using Server.GameDataCache;

namespace Server.Http_NS.Controllers_NS;

/// <summary>
/// Контроллер с общими методами.
/// </summary>
/// <param name="heroCache"></param>
[Route("api/[controller]/[action]")]
public class GeneralController(IHeroCacheService heroCache) : ControllerBaseApi
{
    private static ContentResult? result = null;
    private static readonly Lock locker = new();
    private readonly IHeroCacheService _heroCache = heroCache;

    /// <summary>
    /// Возвращает в ответе список всех героев.
    /// </summary>
    /// <returns></returns>
    public IActionResult ListAllHeroes()
    {
        if (result == null)
        {
            lock (locker)
            {
                result ??= Content(_heroCache.HeroesJson, "application/json");
            }
        }

        return result;
    }
}
