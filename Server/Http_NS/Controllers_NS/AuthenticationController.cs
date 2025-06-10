using Dapper;
using General;
using General.DataBaseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Server.Jwt_NS;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Server.Http_NS.Controllers_NS;

/// <summary>
/// Контроллер для аутентификации пользователей.
/// </summary>
/// <remarks>
/// Конструктор контроллера.
/// </remarks>
/// <param name="configuration">Конфигурация приложения для получения секретного ключа и параметров JWT.</param>
[ApiController]
[Route("[controller]")]
public class AuthenticationController() : ControllerBase {

    /// <summary>
    /// Выполняет аутентификацию и возвращает JWT токен.
    /// </summary>
    /// <param name="request">Модель с именем пользователя и паролем.</param>
    /// <returns>JWT токен при успешной аутентификации или ошибка 401.</returns>
    [HttpPost("Authentication")]
    [AllowAnonymous]
    public async Task<IActionResult> Authentication([FromBody] LoginRequest request) {
        using MySqlConnection connection = new(DataBase.ConnectionString);
        await connection.OpenAsync();
       
        // Проверяем корректность имени пользователя и пароля
        if (/*request.Username != "testUser" || */request.Password != "testPassword") {
            // Неверные учетные данные
            return Unauthorized();
        }

        // Создаем JWT токен
        string token = GenerateJwtToken(request.Username);

        // Возвращаем токен в формате JSON
        return Ok(new { token });
    }




    /// <summary>
    /// Генерирует JWT токен для указанного пользователя.
    /// </summary>
    /// <param name="username">Имя пользователя.</param>
    /// <returns>Строка токена JWT.</returns>
    private static string GenerateJwtToken(string username) {
        // Создание набора требований (claims)
        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        // Настройка параметров токена
        JwtSecurityToken token = new(
            issuer: JwtCash.Issuer,
            audience: JwtCash.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(JwtCash.SecondsExp), // Время жизни токена
            signingCredentials: JwtCash.SigningCredentials
        );

        // Генерация строки токена
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
