using General.DTO.Interfaces;

namespace Server_DB_Postgres.Entities.Users;

/// <summary>
/// Представляет публичный ключ Passkey, привязанный к аккаунту пользователя.
/// </summary>
public sealed class UserAccesskey : ICreatedAt, IUpdatedAt, IVersion
{
    public Guid id { get; init; }

    public required Guid userId { get; init; }
    public User? user { get; init; }

    /// <summary>
    /// Уникальный идентификатор учетных данных, сгенерированный устройством.
    /// </summary>
    public required byte[] descriptorId { get; init; }

    /// <summary>
    /// Публичный ключ в бинарном формате.
    /// </summary>
    public required byte[] publicKey { get; init; }

    /// <summary>
    /// Счетчик использований для защиты от атак воспроизведения.
    /// </summary>
    public uint signatureCounter { get; set; }

    /// <summary>
    /// Тип устройства или дружественное имя (например, "My iPhone 15").
    /// </summary>
    public string? deviceName { get; init; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset createdAt { get; set; }


    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset updatedAt { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public long version { get; set; }
}
