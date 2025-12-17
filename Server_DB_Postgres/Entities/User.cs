namespace Server_DB_Users.Entities;

/// <summary>
/// Представляет пользователя системы.
/// </summary>
public class User
{
    /// <summary>
    /// Уникальный идентификатор пользователя.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Email адрес пользователя.
    /// Может быть null, если пользователь зарегистрирован через сторонний сервис.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Дата и время подтверждения email адреса.
    /// Null, если email еще не подтвержден.
    /// </summary>
    public DateTimeOffset? EmailVerifiedAt { get; set; }

    /// <summary>
    /// Хеш пароля пользователя.
    /// Может быть null, если используется аутентификация через сторонние сервисы.
    /// </summary>
    public string? PasswordHash { get; set; }

    /// <summary>
    /// Часовой пояс пользователя.
    /// Используется для корректного отображения времени в интерфейсе.
    /// </summary>
    public string? TimeZone { get; set; }

    /// <summary>
    /// Флаг, указывающий, является ли пользователь администратором системы.
    /// По умолчанию false.
    /// </summary>
    public bool IsAdmin { get; set; } = false;

    /// <summary>
    /// Коллекция блокировок (банов), примененных к пользователю.
    /// Связь "один ко многим" с сущностью <see cref="User_Ban"/>.
    /// </summary>
    public ICollection<User_Ban> Bans { get; set; } = [];
}
