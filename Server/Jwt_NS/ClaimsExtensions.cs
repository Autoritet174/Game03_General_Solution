using System.Security.Claims;

namespace Server.Jwt_NS;

/// <summary>
/// Предоставляет расширяющие методы для работы с утверждениями (claims) пользователя.
/// </summary>
public static class ClaimsExtensions
{
    /// <summary>
    /// Извлекает идентификатор пользователя в формате Guid из утверждений (claims).
    /// </summary>
    /// <param name="user">Объект <see cref="ClaimsPrincipal"/>, представляющий аутентифицированного пользователя.</param>
    /// <returns>
    /// Идентификатор пользователя в формате <see cref="Guid"/> или null, если:
    /// <list type="bullet">
    /// <item><description>Идентификатор не найден в утверждениях</description></item>
    /// <item><description>Идентификатор не может быть преобразован в Guid</description></item>
    /// <item><description>Утверждения не содержат идентификатора</description></item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// Метод ищет идентификатор пользователя в следующих утверждениях (в порядке приоритета):
    /// <list type="number">
    /// <item><description>"sub" (стандартное утверждение JWT для subject)</description></item>
    /// <item><description>"id" (альтернативное утверждение для идентификатора)</description></item>
    /// <item><description><see cref="ClaimTypes.NameIdentifier"/> (стандартный тип утверждения .NET для идентификатора)</description></item>
    /// </list>
    /// Метод выполняет безопасное преобразование строки в Guid, возвращая null при неудаче.
    /// </remarks>
    /// <example>
    /// <code>
    /// // Получение идентификатора пользователя из контроллера
    /// Guid? userId = User.GetGuid();
    /// if (userId.HasValue)
    /// {
    ///     // Использование идентификатора
    /// }
    /// </code>
    /// </example>
    public static Guid? GetGuid(this ClaimsPrincipal user)
    {
        string? id = user.FindFirst("sub")?.Value
                     ?? user.FindFirst("id")?.Value
                     ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return string.IsNullOrEmpty(id) ? null : Guid.TryParse(id, out Guid guid) ? guid : null;
    }
}
