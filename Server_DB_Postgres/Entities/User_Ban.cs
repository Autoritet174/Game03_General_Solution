namespace Server_DB_Users.Entities;

/// <summary>
/// Представляет запись о блокировке (бане) пользователя.
/// </summary>
public class User_Ban
{
    /// <summary>
    /// Уникальный идентификатор записи о блокировке.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Идентификатор пользователя, к которому применена блокировка.
    /// </summary>
    public required Guid UserId { get; set; }

    /// <summary>
    /// Дата и время создания записи о блокировке.
    /// </summary>
    public required DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Дата и время окончания блокировки.
    /// Null, если блокировка бессрочная.
    /// </summary>
    public DateTimeOffset? ExpiresAt { get; set; }

    /// <summary>
    /// Идентификатор причины блокировки.
    /// Null, если причина не указана или используется системная причина.
    /// </summary>
    public Guid? UserBansReasonsId { get; set; }

    /// <summary>
    /// Навигационное свойство к пользователю, к которому применена блокировка.
    /// Связь "многие к одному" с сущностью <see cref="User"/>.
    /// </summary>
    public User? User { get; set; }
}
