using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Jwt_NS;


/// <summary>
/// Статический класс для хранения параметров аутентификации.
/// </summary>
public static class Jwt {

    /// <summary>
    /// Секретный ключ для генерации JWT токенов.
    /// </summary>
    private static SigningCredentials? signingCredentials;

    public static string Issuer { get; private set; } = "";
    public static string Audience { get; private set; } = "";
    public static string JwtKey { get; private set; } = "";
    public static SigningCredentials SigningCredentials { get => signingCredentials!; private set => signingCredentials = value; }

    public static double SecondsExp { get; private set; } = 60d * 60d * 24d * 365d;

    /// <summary>
    /// Метод для инициализации
    /// </summary>
    /// <param name="configuration"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void Initialize(IConfiguration configuration) {
        Issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException(configuration["Jwt:Issuer"]);
        Audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException(configuration["Jwt:Audience"]);
        JwtKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException(configuration["Jwt:Key"]);

        string s = Environment.GetEnvironmentVariable("") ?? "";

        string jwtKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException(configuration["Jwt:Key"]);
        SymmetricSecurityKey Key = new(Encoding.UTF8.GetBytes(jwtKey));
        SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
    }


    /// <summary>
    /// Генерирует JWT токен для указанного пользователя.
    /// </summary>
    /// <param name="username">Имя пользователя.</param>
    /// <returns>Строка токена JWT.</returns>
    public static string GenerateJwtToken(string username) {
        byte[] randomBytes = System.Security.Cryptography.RandomNumberGenerator.GetBytes(16);
        Guid secureGuid = new(randomBytes);

        // Создание набора требований (claims)
        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, secureGuid.ToString())
        ];

        // Настройка параметров токена
        JwtSecurityToken token = new(
            issuer: Jwt.Issuer,
            audience: Jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(Jwt.SecondsExp), // Время жизни токена
            signingCredentials: Jwt.SigningCredentials
        );

        // Генерация строки токена
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
