namespace Server;

/// <summary>
/// Константы на весь сервер.
/// </summary>
public static class Consts
{
    /// <summary>
    /// Максимальная длина емаил.
    /// </summary>
    public const int EMAIL_MAX_LENGTH = 254;

    /// <summary>
    /// Максимальная длина пароля.
    /// </summary>
    public const int PASSWORD_MAX_LENGTH = 128;

    /// <summary>
    /// Максимальное количество попыток аутентификации.
    /// </summary>
    public const int MAX_LOGIN_ATTEMPTS = 5;

    public const string RATE_LIMITER_POLICY_AUTH = nameof(RATE_LIMITER_POLICY_AUTH);

    public const string RATE_LIMITER_POLICY_COLLECTION = nameof(RATE_LIMITER_POLICY_COLLECTION);
    public static readonly Guid GuidAdmin = Guid.Parse("113ae534-2310-40e3-a895-f3747ea976ca");
}
