using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.GameDataCache;
using Server.Http_NS.Controllers_NS;

namespace Server.GameData;

/// <summary>
/// Контроллер с общими методами.
/// </summary>
/// <param name="heroCache"></param>
public class GameDataController(IGameDataCacheService heroCache) : ControllerBaseApi
{
    private static ContentResult? result = null;
    private static readonly Lock locker = new();
    private readonly IGameDataCacheService _heroCache = heroCache;

    /// <summary> Возвращает в ответе список всех константных игровых данных нужных на клиенте игры. </summary>
    [HttpPost]
    public IActionResult Main()
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
