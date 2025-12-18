using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres.Entities.users;

/// <summary> Представляет пользователя системы. </summary>
[Table("Users", Schema = nameof(users))]
[Index(nameof(Email), IsUnique = true)]
public class User : IVersion, ICreatedAt, IUpdatedAt
{
    /// <summary> Уникальный идентификатор. </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary> Email адрес пользователя. Может быть null, если пользователь зарегистрирован через сторонний сервис. </summary>
    [MaxLength(255)]
    public string? Email { get; set; }

    /// <summary> Дата и время подтверждения email адреса. Null, если email еще не подтвержден. </summary>
    public DateTimeOffset? EmailVerifiedAt { get; set; }

    /// <summary> Хеш пароля пользователя. </summary>
    [MaxLength(255)]
    public string? PasswordHash { get; set; }

    /// <summary> Часовой пояс пользователя. </summary>
    [MaxLength(255)]
    public string? TimeZone { get; set; }


    /// <summary> Флаг, указывающий, является ли пользователь администратором системы. </summary>
    [HasDefaultValue(false)]
    public bool IsAdmin { get; set; } = false;

    /// <summary> Коллекция <see cref="UserBan"/>. </summary>
    public ICollection<UserBan> UserBans { get; set; } = [];

    /// <summary> <inheritdoc/> </summary>
    public long Version { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset UpdatedAt { get; set; }
}
