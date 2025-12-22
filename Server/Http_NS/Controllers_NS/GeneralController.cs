using Microsoft.AspNetCore.Mvc;
using Server.GameDataCache;

namespace Server.Http_NS.Controllers_NS;

/// <summary>
/// Контроллер с общими методами.
/// </summary>
/// <param name="heroCache"></param>
[Route("api/[controller]/[action]")]
public class GeneralController(IGameDataCacheService heroCache) : ControllerBaseApi
{
    private static ContentResult? result = null;
    private static readonly Lock locker = new();
    private readonly IGameDataCacheService _heroCache = heroCache;

    /// <summary> Возвращает в ответе список всех константных игровых данных нужных на клиенте игры. </summary>
    [HttpPost]
    public IActionResult GameData()
    {
        if (result == null)
        {
            lock (locker)
            {
                result ??= Content(_heroCache.GameDataJson, "application/json");
            }
        }

        return result;
    }
}
