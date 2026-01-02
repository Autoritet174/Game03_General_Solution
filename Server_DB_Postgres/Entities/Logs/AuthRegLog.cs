using Server_DB_Postgres.Entities.Users;
using Server_DB_Postgres.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres.Entities.Logs;

/// <summary> Лог авторизации пользователей. </summary>
[Table("AuthRegLogs", Schema = nameof(Logs))]
public class AuthRegLog : IVersion, ICreatedAt
{
    /// <summary> Уникальный идентификатор. </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary> Email при авторизации. </summary>
    [MaxLength(256)]
    public string? Email { get; set; }

    public required Guid? UserId { get; set; }
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

    [HasDefaultValue(true)]
    public bool ActionIsAuthentication { get; set; } = true;
}
