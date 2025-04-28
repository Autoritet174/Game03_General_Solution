using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Jwt_NS;

/// <summary>
/// Менеджер JWT валидации токенов.
/// </summary>
public class JwtValidator {
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly TokenValidationParameters _validationParameters;

    /// <summary>
    /// Инициализация валидатора, кэширование ключей и параметров.
    /// </summary>
    public JwtValidator() {
        _tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.UTF8.GetBytes(JwtCash.JwtKey);

        _validationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = JwtCash.Issuer,//configuration["Jwt:Issuer"],
            ValidAudience = JwtCash.Audience,//configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // Нет допустимого времени расхождения
        };
    }

    /// <summary>
    /// Проверяет валидность токена и возвращает ClaimsPrincipal при успехе.
    /// </summary>
    /// <param name="token">JWT токен</param>
    /// <returns>ClaimsPrincipal или null</returns>
    public ClaimsPrincipal? ValidateToken(string token) {
        try {
            //var principal = _tokenHandler.ValidateToken(token, _validationParameters, out _);
            return _tokenHandler.ValidateToken(token, _validationParameters, out _);
        }
        catch {
            // Ошибка валидации токена
            return null;
        }
    }
}
