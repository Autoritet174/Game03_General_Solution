using General.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        await Task.Delay(2000);//Намеренная задержка от брутфорса

        if (!string.IsNullOrEmpty(request.Email) && !string.IsNullOrEmpty(request.Password))
        {
            bool isCorrect = await GF_DataBase.IsCorrectEmailPassword(request.Email, request.Password);
            if (isCorrect)
            {
                string token = Jwt.GenerateJwtToken(request.Email);
                return Ok(new { token });
            }
        }

        return Unauthorized();
    }

}
