using General.DTO.Interfaces;
using Server_DB_Postgres.Entities.Users;
using System.Net;

namespace Server_DB_Postgres.Entities.Logs;

/// <summary> Лог авторизации пользователей. </summary>
public class AuthenticationLog : IVersion, ICreatedAt
{
    public Guid Id { get; init; }

    /// <summary> <inheritdoc/> </summary>
    public long Version { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset CreatedAt { get; set; }

    public string? Email { get; set; }

    public Guid? UserId { get; set; }
    public User? User { get; set; }

    /// <summary> Успешность авторизации. </summary>
    public required bool Success { get; set; }

    public Guid? UserDeviceId { get; set; }
    public UserDevice? UserDevice { get; set; }

    public IPAddress? Ip { get; set; }

    public Guid? UserSessionId { get; set; }
    public UserSession? UserSession { get; set; }
}
