using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Server.Jwt_NS;


/// <summary>
/// Статический класс для хранения параметров аутентификации.
/// </summary>
public static class JwtCash {

    /// <summary>
    /// Секретный ключ для генерации JWT токенов.
    /// </summary>
    //public const string SecretKey = "SuperSecretKey12345"; // Ключ должен быть достаточно длинным
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

        string jwtKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException(configuration["Jwt:Key"]);
        SymmetricSecurityKey Key = new(Encoding.UTF8.GetBytes(jwtKey));
        SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
    }

}
