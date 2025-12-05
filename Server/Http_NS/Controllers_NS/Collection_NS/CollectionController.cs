using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Server.Jwt_NS;

namespace Server.Http_NS.Controllers_NS.Collection_NS;

/// <summary>
/// Контроллер для управления коллекциями пользователя.
/// </summary>
public class CollectionController(Server_DB_UserData.MongoRepository _mongoRepository) : ControllerBaseApi
{
    /// <summary>
    /// Получает коллекцию героев текущего пользователя.
    /// </summary>
    /// <returns>
    /// Возвращает результат операции HTTP:
    /// - 200 OK с коллекцией героев при успешном выполнении
    /// - 401 Unauthorized, если пользователь не аутентифицирован или не имеет действительного идентификатора
    /// </returns>
    /// <remarks>
    /// Метод доступен только аутентифицированным пользователям.
    /// Применяется ограничение скорости запросов (rate limiting) с политикой "login".
    /// Запрос выполняется асинхронно к MongoDB репозиторию для получения данных о героях.
    /// </remarks>
    /// <response code="200">Коллекция героев успешно получена</response>
    /// <response code="401">Пользователь не аутентифицирован</response>
    [EnableRateLimiting("login")]
    [HttpPost("Heroes")]
    public async Task<IActionResult> Heroes()
    {
        Guid? userId = User.GetGuid();
        if (userId == null)
        {
            return Unauthorized();
        }

        Task<List<object>> taskHeroes = _mongoRepository.GetHeroesByUserIdAsync(userId.Value);
        //Task<List<object>> taskHeroes = _mongoRepository.GetHeroesByUserIdAsync(userId.Value);

        List<object> heroes = await taskHeroes;
        //List<object> result = await taskHeroes;
        return Ok(new { heroes });
    }
}
