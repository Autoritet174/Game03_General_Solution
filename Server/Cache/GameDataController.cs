using Microsoft.AspNetCore.Mvc;
using Server.Http_NS.Controllers_NS;

namespace Server.Cache;

/// <summary>
/// Контроллер с общими методами.
/// </summary>
/// <param name="cache"></param>
public class GameDataController(CacheService cache) : ControllerBaseApi
{
    private static ContentResult? result;
    private static readonly Lock locker = new();

    /// <summary> Возвращает в ответе список всех константных игровых данных нужных на клиенте игры. </summary>
    [HttpPost]
    public IActionResult Main()
    {
        if (result == null)
        {
            lock (locker)
            {
                result ??= Content(cache.GameDataJson, General.GlobalHelper.APPLICATION_JSON);
            }
        }

        return result;
    }
}
