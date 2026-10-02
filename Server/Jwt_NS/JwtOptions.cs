namespace Server.Jwt_NS;


/// <summary>
/// 
/// </summary>
public sealed record JwtOptions
{
    /// <summary>Эмитент токена.</summary>
    public required string issuer { get; set; }

    /// <summary>Аудитория токена.</summary>
    public required string audience { get; set; }

    /// <summary>Время жизни токена.</summary>
    public TimeSpan lifetime { get; init; } = TimeSpan.FromHours(2);
}
