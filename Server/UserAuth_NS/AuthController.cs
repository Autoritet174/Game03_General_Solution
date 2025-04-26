using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Server.UserAuth_NS;

/// <summary>
/// Контроллер для аутентификации пользователей.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase {
    //private static bool _inited = false;
    //private static string _issuer;
    //private static string _audience;
    //private static SymmetricSecurityKey _key;
    //private static SigningCredentials _signingCredentials;
    //private static readonly object _initLock = new object();


    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="configuration">Конфигурация приложения для получения секретного ключа и параметров JWT.</param>
    public AuthController(IConfiguration configuration) {
        // Используем lock для потокобезопасной инициализации
        //if (!_inited) {
        //    lock (_initLock) {
        //        if (!_inited) {  // Double-check lock
        //            _issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("configuration[\"Jwt:Issuer\"]");
        //            _audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("configuration[\"Jwt:Audience\"]");
        //            string jwt_key = configuration["Jwt:Key"] ?? throw new ArgumentNullException("configuration[\"Jwt:Key\"]");
        //            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt_key));
        //            _signingCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        //            _inited = true;
        //        }
        //    }
        //}
    }


    /// <summary>
    /// Выполняет аутентификацию и возвращает JWT токен.
    /// </summary>
    /// <param name="request">Модель с именем пользователя и паролем.</param>
    /// <returns>JWT токен при успешной аутентификации или ошибка 401.</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request) {
        // Проверяем корректность имени пользователя и пароля
        if (request.Username != "testUser" || request.Password != "testPassword") {
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
    private string GenerateJwtToken(string username) {
        // Создание набора требований (claims)
        Claim[] claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Настройка параметров токена
        JwtSecurityToken token = new(
            issuer: AuthOptions.Issuer,
            audience: AuthOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30), // Время жизни токена
            signingCredentials: AuthOptions.SigningCredentials
        );

        // Генерация строки токена
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
