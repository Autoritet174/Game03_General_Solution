using General.DTO.Interfaces;
using Server_DB_Postgres.Entities.Users;
using System.Net;

namespace Server_DB_Postgres.Entities.Logs;

/// <summary> Лог авторизации пользователей. </summary>
public class AuthenticationLog : IVersion, ICreatedAt
{
    public Guid id { get; init; }

    /// <summary> <inheritdoc/> </summary>
    public long version { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset createdAt { get; set; }

    public string? email { get; set; }

    public Guid? userId { get; set; }
    public User? user { get; set; }

    /// <summary> Успешность авторизации. </summary>
    public required bool success { get; set; }

    public Guid? userDeviceId { get; set; }
    public UserDevice? userDevice { get; set; }

    public IPAddress? ip { get; set; }

    public Guid? userSessionId { get; set; }
    public UserSession? userSession { get; set; }
}
