using Microsoft.AspNetCore.Identity;

namespace Server.Utilities;

/// <summary>
/// Предоставляет утилиты для хеширования и верификации паролей.
/// </summary>
/// <remarks>
/// Класс использует <see cref="PasswordHasher{TUser}"/> из ASP.NET Core Identity
/// для безопасного хеширования паролей с использованием современных алгоритмов.
/// </remarks>
public static class PassHasher
{
    private static readonly PasswordHasher<object> _passwordHasher = new();

    /// <summary>
    /// Создает хеш пароля для указанного пользователя.
    /// </summary>
    /// <param name="user">Идентификатор пользователя или любая строка, связанная с пользователем.
    /// Используется для добавления соли (salt) к хешу.</param>
    /// <param name="password">Пароль в открытом виде, который необходимо захэшировать.</param>
    /// <returns>Хешированная версия пароля, готовая для безопасного хранения.</returns>
    /// <remarks>
    /// Метод использует стандартный алгоритм хеширования ASP.NET Core Identity,
    /// который включает соль (salt) и может автоматически обновлять алгоритмы при необходимости.
    /// </remarks>
    /// <example>
    /// <code>
    /// string userId = "user123";
    /// string password = "MySecurePassword123";
    /// string hashedPassword = PassHasher.Create(userId, password);
    /// // Сохранить hashedPassword в базу данных
    /// </code>
    /// </example>
    public static string Create(string user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    /// <summary>
    /// Проверяет, соответствует ли предоставленный пароль хранимому хешу.
    /// </summary>
    /// <param name="user">Идентификатор пользователя или строка, использованная при создании хеша.</param>
    /// <param name="hashedPassword">Хранимый хеш пароля из базы данных.</param>
    /// <param name="providedPassword">Пароль, предоставленный пользователем для проверки.</param>
    /// <returns>
    /// <c>true</c>, если предоставленный пароль соответствует хешу; в противном случае <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Метод выполняет безопасное сравнение паролей, устойчивое к атакам по времени.
    /// Автоматически определяет и поддерживает различные версии алгоритмов хеширования,
    /// что позволяет обновлять старые хеши при следующем успешном входе пользователя.
    /// </remarks>
    /// <example>
    /// <code>
    /// string userId = "user123";
    /// string storedHash = "..."; // Из базы данных
    /// string userInput = "MySecurePassword123";
    /// 
    /// bool isValid = PassHasher.Verify(userId, storedHash, userInput);
    /// if (isValid)
    /// {
    ///     // Пароль верный
    /// }
    /// </code>
    /// </example>
    public static bool Verify(string user, string hashedPassword, string providedPassword)
    {
        PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}
