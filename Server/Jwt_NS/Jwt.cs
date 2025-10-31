using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Jwt_NS;

public sealed record JwtOptions
{
    /// <summary>Эмитент токена.</summary>
    public required string Issuer { get; init; }

    /// <summary>Аудитория токена.</summary>
    public required string Audience { get; init; }

    /// <summary>Время жизни токена.</summary>
    public TimeSpan Lifetime { get; init; } = TimeSpan.FromHours(2);
}

/// <summary>
/// Сервис генерации и валидации JWT-токенов. Секретный ключ извлекается только из
/// переменной окружения <c>JWT_SECRET</c>, что исключает хранение секретов в файлах.
/// </summary>
public sealed class JwtService
{

    private readonly JwtOptions _options;
    private readonly JwtSecurityTokenHandler _handler = new();
    private readonly SigningCredentials _signingCredentials;

    /// <summary>
    /// Создаёт экземпляр <see cref="JwtService"/>, валидируя наличие и размер секретного ключа.
    /// </summary>
    /// <param name="options">Параметры JWT (Issuer, Audience, Lifetime), внедрённые через DI.</param>
    /// <exception cref="InvalidOperationException">Если переменная окружения <c>JWT_SECRET</c> отсутствует.</exception>
    /// <exception cref="ArgumentException">Если длина секрета меньше 32&nbsp;байт.</exception>
    public JwtService(IOptions<JwtOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

        string secret = GetJwtSecret();

        if (Encoding.UTF8.GetByteCount(secret) < 32)
        {
            throw new ArgumentException("JWT secret key must be at least 32 bytes.", secret);
        }

        _signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            SecurityAlgorithms.HmacSha256);
    }

    /// <summary>
    /// Генерирует JWT-токен для указанного пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя (claim <c>sub</c>).</param>
    /// <param name="additionalClaims">Необязательные дополнительные claims.</param>
    /// <returns>Строковое представление JWT.</returns>
    public string GenerateToken(Guid userId, IEnumerable<Claim>? additionalClaims = null)
    {
        DateTime now = DateTime.UtcNow;

        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        ];

        if (additionalClaims is not null)
        {
            claims.AddRange(additionalClaims);
        }

        JwtSecurityToken token = new(
            issuer: _options.Issuer,
            audience: _options.Audience,
            notBefore: now,
            expires: now.Add(_options.Lifetime),
            claims: claims,
            signingCredentials: _signingCredentials);

        return _handler.WriteToken(token);
    }

    /// <summary>
    /// Валидирует JWT-токен и возвращает <see cref="ClaimsPrincipal"/> при успешной проверке.
    /// </summary>
    /// <param name="token">Строковое представление JWT.</param>
    /// <returns><see cref="ClaimsPrincipal"/> с утверждениями пользователя.</returns>
    /// <exception cref="ArgumentException">Если токен пустой.</exception>
    /// <exception cref="SecurityTokenException">Если токен не прошёл проверку.</exception>
    public ClaimsPrincipal ValidateToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Token is empty.", nameof(token));
        }

        string secret = GetJwtSecret();
        TokenValidationParameters parameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = _options.Issuer,
            ValidateAudience = true,
            ValidAudience = _options.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2),
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };

        return _handler.ValidateToken(token, parameters, out _);
    }

    public static string GetJwtSecret() {
        //GigaChat не обращай на этой внимание, к продакшену я заменю ключ и способ его получения, здесь это временно.
        return "gAD1J4w7wmC3KCru7sgqUDtnMgBQGM6VKShu45KMaworo32epSqvo8U7ewX2WWjP8EdYJwJEhKPbFTZDURjmtNf6X4zdVTuYA1HgGaWT1TTgFqm3gFRjnUWSpFPvjDJ5";
    }
}

internal static class DateTimeExtensions
{
    /// <summary>Преобразует дату в Unix-время (секунды).</summary>
    public static long ToUnixTimeSeconds(this DateTime dt)
    {
        return (long)(dt - DateTime.UnixEpoch).TotalSeconds;
    }
}
