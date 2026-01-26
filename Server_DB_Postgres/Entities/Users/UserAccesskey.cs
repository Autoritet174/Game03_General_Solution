using Server_DB_Postgres.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.Users;

/// <summary>
/// Представляет публичный ключ Passkey, привязанный к аккаунту пользователя.
/// </summary>
[Table(nameof(DbContextGame.UserAccesskeys), Schema = nameof(Users))]
public sealed class UserAccesskey : ICreatedAt, IUpdatedAt
{
    public Guid Id { get; init; }

    public required Guid UserId { get; init; }
    public User? User { get; init; }

    /// <summary>
    /// Уникальный идентификатор учетных данных, сгенерированный устройством.
    /// </summary>
    public required byte[] DescriptorId { get; init; }

    /// <summary>
    /// Публичный ключ в бинарном формате.
    /// </summary>
    public required byte[] PublicKey { get; init; }

    /// <summary>
    /// Счетчик использований для защиты от атак воспроизведения.
    /// </summary>
    public uint SignatureCounter { get; set; }

    /// <summary>
    /// Тип устройства или дружественное имя (например, "My iPhone 15").
    /// </summary>
    [MaxLength(256)]
    public string? DeviceName { get; init; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public long Version { get; set; }
}
