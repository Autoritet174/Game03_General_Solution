using Microsoft.AspNetCore.Mvc;
using Server.GameDataCache;

namespace Server.Http_NS.Controllers_NS;

/// <summary>
/// Контроллер для аутентификации пользователей.
/// </summary>
public class GetListAllHeroesController() : ControllerBaseApi
{
    private static ContentResult? result = null;
    /// <summary>
    /// Возвращает в ответе список всех героев.
    /// </summary>
    /// <param name="request">Данные для входа.</param>
    /// <returns>JWT-токен при успешной аутентификации или код ошибки.</returns>
    public async Task<IActionResult> GetListAllHeroes()
    {
        await GF.DelayWithOutDebug500();
        result ??= Content(ListAllHeroes.Json, "application/json");
        return result;
    }
}
