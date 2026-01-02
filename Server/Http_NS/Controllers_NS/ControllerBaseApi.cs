using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using L = General.LocalizationKeys;

namespace Server.Http_NS.Controllers_NS;

/// <summary>
/// Базовый абстрактный класс для всех контроллеров API.
/// Предоставляет общие функции и настройки для API-контроллеров.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
[RequestSizeLimit(1_048_576)] // 1 МБ
public abstract class ControllerBaseApi : ControllerBase
{
    /// <summary>
    /// Возвращает ответ BadRequest (400) с ключом локализации для ошибки сервера.
    /// </summary>
    /// <param name="keyLocalization">Ключ локализации, идентифицирующий конкретную ошибку.</param>
    /// <returns>Ответ BadRequest с объектом, содержащим ключ локализации.</returns>
    /// <remarks>
    /// Используется для стандартизированной обработки ошибок с поддержкой локализации.
    /// </remarks>
    protected IActionResult BadRequestWithServerError(string keyLocalization)
    {
        return BadRequest(new { errorKey = keyLocalization });
    }

    ///// <summary>
    ///// Возвращает стандартный ответ BadRequest (400) для ошибки неверных учетных данных аутентификации.
    ///// </summary>
    ///// <returns>Ответ BadRequest с ключом локализации для ошибки неверных учетных данных.</returns>
    ///// <remarks>
    ///// Соответствует ключу локализации <see cref="L.Error.Server.InvalidCredentials"/>.
    ///// </remarks>
    //protected IActionResult BadRequestAuthInvalidCredentials()
    //{
    //    return BadRequestWithServerError(L.Error.Server.InvalidCredentials);
    //}

    /// <summary>
    /// Возвращает стандартный ответ BadRequest (400) для ошибки неверного ответа сервера.
    /// </summary>
    /// <returns>Ответ BadRequest с ключом локализации для ошибки неверного ответа.</returns>
    /// <remarks>
    /// Соответствует ключу локализации <see cref="L.Error.Server.InvalidResponse"/>.
    /// </remarks>
    protected IActionResult BadRequestInvalidResponse()
    {
        return BadRequestWithServerError(L.Error.Server.InvalidResponse);
    }
}
