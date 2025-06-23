using General.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Server.Jwt_NS;

namespace Server.Http_NS.Controllers_NS.Users;

/// <summary>
/// Контроллер для аутентификации пользователей.
/// </summary>
/// <remarks>
/// Конструктор контроллера.
/// </remarks>
/// <param name="configuration">Конфигурация приложения для получения секретного ключа и параметров JWT.</param>
public class AuthenticationController() : ControllerBaseApi
{

    /// <summary>
    /// Выполняет аутентификацию и возвращает JWT токен.
    /// </summary>
    /// <param name="request">Модель с именем пользователя и паролем.</param>
    /// <returns>JWT токен при успешной аутентификации или ошибка 401.</returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Authentication([FromBody] Login request)
    {
        using MySqlConnection connection = new(General.DataBase.ConnectionString_GameData);
        await connection.OpenAsync();







        // Проверяем корректность имени пользователя и пароля
        if (string.Equals(request.Email, "SuperAdmin@mail.ru", StringComparison.OrdinalIgnoreCase) &&
            string.Equals(request.Password, "testPassword", StringComparison.Ordinal))
        {

        }
        else
        {
            // Неверные учетные данные
            return Unauthorized();
        }

        // Создаем JWT токен
        string token = Jwt.GenerateJwtToken(request.Email);

        // Возвращаем токен в формате JSON
        return Ok(new { token });
    }

}
