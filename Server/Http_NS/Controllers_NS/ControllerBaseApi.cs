using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Http_NS.Controllers_NS;

/// <summary>
/// Абстрактный базовый контроллер для всех API-контроллеров.
/// Предоставляет унифицированные методы ответов (успех, ошибка) и применяет авторизацию по умолчанию ко всем действиям.
/// </summary>
/// <remarks>
/// Все контроллеры, наследующие от этого класса, автоматически требуют аутентификацию (через атрибут [Authorize]).
/// Также автоматически устанавливается префикс маршрута "api/[controller]" и тип контроллера как API-контроллер.
/// </remarks>
[Route("api/[controller]")]
[ApiController]
[Authorize]
[RequestSizeLimit(1_048_576)] // 1 МБ
public abstract class ControllerBaseApi : ControllerBase
{
    /// <summary>
    /// Возвращает ответ с ошибкой на основе перечисления <see cref="General.ServerErrors.Error"/>.
    /// Используется для стандартизированного возврата кодов ошибок.
    /// </summary>
    /// <param name="serverResponse">Значение перечисления, представляющее тип ошибки сервера.</param>
    /// <returns>HTTP-ответ с кодом 400 и объектом { code = (long)serverResponse }.</returns>
    /// <remarks>
    /// Метод преобразует значение перечисления в число (long) и возвращает его как поле "code".
    /// Это позволяет клиенту интерпретировать тип ошибки по числовому коду.
    /// </remarks>
    /// <example>
    /// Если serverResponse = Error.InvalidInput, и значение = 1001, то ответ:
    /// {
    ///   "code": 1001
    /// }
    /// </example>
    protected IActionResult BadRequestWithServerError(General.ServerErrors.Error serverResponse)
    {
        // Преобразование перечисления в long позволяет избежать проблем с сериализацией
        // и обеспечивает единый формат кодов ошибок на стороне клиента.
        // Это особенно важно, если клиент ожидает числовые коды, а не строки.
        return BadRequest(new { code = (long)serverResponse });
    }

    /// <summary>
    /// Возвращает ответ с общей ошибкой аутентификации (код 400 Bad Request).
    /// Используется при неверных учётных данных или других ошибках входа.
    /// </summary>
    /// <remarks>
    /// Метод возвращает HTTP-статус 400 и унифицированный объект ошибки с числовым кодом.
    /// В целях безопасности **не уточняет**, что именно пошло не так — 
    /// существует ли email, или неверен пароль, или учётная запись заблокирована.
    /// Это предотвращает атаки по перебору пользователей (user enumeration).
    /// 
    /// Пример ответа:
    /// {
    ///   "code": 2
    /// }
    /// где 2 — это значение перечисления <see cref="General.ServerErrors.Error.AuthInvalidCredentials"/>.
    /// </remarks>
    /// <returns>HTTP-ответ с кодом состояния 400 и кодом ошибки аутентификации.</returns>
    protected IActionResult BadRequestAuthInvalidCredentials()
    {
        // Возвращаем обобщённую ошибку аутентификации (400 Bad Request)
        // чтобы не раскрывать детали: email не существует, пароль неверен и т.д.
        return BadRequest(new { code = (long)General.ServerErrors.Error.AuthInvalidCredentials });
    }

    /// <summary>
    /// Возвращает ответ с кодом 415 (Unsupported Media Type).
    /// Используется, когда клиент отправляет данные в неподдерживаемом формате (например, не application/json).
    /// </summary>
    /// <remarks>
    /// Этот метод возвращает HTTP-статус 415 с телом, содержащим числовой код ошибки.
    /// Подходит для защиты API от запросов с неверным заголовком Content-Type.
    /// Пример ответа:
    /// {
    ///   "code": 4
    /// }
    /// где 4 — это значение перечисления <see cref="General.ServerErrors.Error.UnsupportedMediaType"/>.
    /// </remarks>
    /// <returns>HTTP-ответ с кодом состояния 415 и объектом ошибки.</returns>
    protected IActionResult BadRequestUnsupportedMediaType()
    {
        // Возвращаем 415 Unsupported Media Type, если Content-Type не является application/json
        return StatusCode(415, new { code = (long)General.ServerErrors.Error.UnsupportedMediaType });
    }

}
