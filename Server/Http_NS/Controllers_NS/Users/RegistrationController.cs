using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Xml.Linq;
namespace Server.Http_NS.Controllers_NS.Users;

public class RegistrationController
    //(
    //UserManager<IdentityUser> userManager,
    //SignInManager<IdentityUser> signInManager)
    : ControllerBaseApi
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] General.Requests.Login model)
    {
        if (string.IsNullOrEmpty(model.Email))
        {
            return CBA_BadRequest(General.GF.ServerErrors.EmailEmpty);
        }

        if (string.IsNullOrEmpty(model.Password))
        {
            return CBA_BadRequest(General.GF.ServerErrors.PasswordEmpty);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!GF.IsValidEmail(model.Email))
        {
            return CBA_BadRequest(General.GF.ServerErrors.EmailBad);
        }


        // Проверка, существует ли пользователь с таким email
        bool exist = await GF_DataBase.IsExistsEmail(model.Email);
        if (!exist)
        {
            return CBA_BadRequest(General.GF.ServerErrors.EmailExists);
        }



        // Создание нового пользователя
        await using MySqlConnection connection = new(General.DataBase.ConnectionString_UsersData);
        await connection.OpenAsync();
        const string sql = "INSERT INTO users (email, password_hash) VALUES (@email, @password_hash)";

        await using MySqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@email", model.Email);
        command.Parameters.AddWithValue("@password_hash", UserRegAuth_NS.Password.Create(model.Email, model.Password));

        int count = await command.ExecuteNonQueryAsync();

        //IdentityResult result = await userManager.CreateAsync(user, model.Password);

        if (count > 0)
        {
            // Дополнительные действия после успешной регистрации
            // Например, отправка email подтверждения или добавление ролей

            // await _userManager.AddToRoleAsync(user, "User");
            // await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(new { message = "Регистрация прошла успешно" });
        }

        return BadRequest(ModelState);
    }

}

