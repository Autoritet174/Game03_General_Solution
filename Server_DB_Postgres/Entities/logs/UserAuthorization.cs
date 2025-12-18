using Server_DB_Postgres.Entities.users;
using Server_DB_Postgres.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.logs;

/// <summary> Лог авторизации пользователей. </summary>
[Table("UserAuthorizations", Schema = nameof(logs))]
public class UserAuthorization : IVersion, ICreatedAt
{
    /// <summary> Уникальный идентификатор. </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary> Email при авторизации. </summary>
    [MaxLength(255)]
    public required string Email { get; set; }

    /// <summary> Идентификатор пользователя. </summary>
    public Guid? UserId { get; set; }
    /// <summary> Навигационное свойство к <see cref="users.User"/>. </summary>
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    /// <summary> Успешность авторизации. </summary>
    public required bool Success { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public long Version { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset CreatedAt { get; set; }
}
