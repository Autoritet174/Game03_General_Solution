using NpgsqlTypes;
using Server_DB_Postgres.Entities.Users;
using Server_DB_Postgres.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Server_DB_Postgres.Entities.Logs;

/// <summary> Лог авторизации пользователей. </summary>
[Table("UserAuthorizations", Schema = nameof(Logs))]
public class UserAuthorization : IVersion, ICreatedAt
{
    /// <summary> Уникальный идентификатор. </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary> Email при авторизации. </summary>
    [MaxLength(255)]
    public string? Email { get; set; }

    /// <summary> Идентификатор пользователя. </summary>
    public Guid? UserId { get; set; }
    /// <summary> Навигационное свойство к <see cref="Users.User"/>. </summary>
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    /// <summary> Успешность авторизации. </summary>
    public required bool Success { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public long Version { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary> Идентификатор устройства пользователя. </summary>
    public Guid? UserDeviceId { get; set; }
    /// <summary> Навигационное свойство к <see cref="Users.UserDevice"/>. </summary>
    [ForeignKey(nameof(UserDeviceId))]
    public UserDevice? UserDevice { get; set; }

    /// <summary> IP-адрес устройства. </summary>
    public IPAddress? Ip { get; set; }
}
