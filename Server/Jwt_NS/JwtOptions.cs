namespace Server.Jwt_NS;


/// <summary>
/// 
/// </summary>
public sealed record JwtOptions
{
    /// <summary>Эмитент токена.</summary>
    public required string Issuer { get; set; }

    /// <summary>Аудитория токена.</summary>
    public required string Audience { get; set; }

    /// <summary>Время жизни токена.</summary>
    public TimeSpan Lifetime { get; init; } = TimeSpan.FromHours(2);
}
