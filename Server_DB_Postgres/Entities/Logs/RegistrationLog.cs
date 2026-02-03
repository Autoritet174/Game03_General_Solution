using Server_DB_Postgres.Entities.Users;
using Server_DB_Postgres.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Server_DB_Postgres.Entities.Logs;

/// <summary> Лог авторизации пользователей. </summary>
[Table(nameof(DbContextGame.RegistrationLogs), Schema = nameof(Logs))]
public class RegistrationLog : IVersion, ICreatedAt
{
    public Guid Id { get; init; }

    /// <summary> <inheritdoc/> </summary>
    public long Version { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset CreatedAt { get; set; }

    [MaxLength(256)]
    public string? Email { get; set; }

    public Guid? UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    /// <summary> Успешность регистрации. </summary>
    public required bool Success { get; set; }


    public Guid? UserDeviceId { get; set; }
    [ForeignKey(nameof(UserDeviceId))]
    public UserDevice? UserDevice { get; set; }

    public IPAddress? Ip { get; set; }
}
