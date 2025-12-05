using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Server.Http_NS.Controllers_NS.Users_NS;

/// <summary>
/// Контроллер регистрации.
/// </summary>
public class RegistrationController : ControllerBaseApi
{
    /// <summary>
    /// Регистрация.
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register()
    {
        //await Task.Delay(2000);

        //if (string.IsNullOrEmpty(model.Email))
        //{
        //    return CBA_BadRequest(General.ServerErrors.Response.Reg_Email_Empty);
        //}

        //if (string.IsNullOrEmpty(model.Password))
        //{
        //    return CBA_BadRequest(General.ServerErrors.Response.Reg_Password_Empty);
        //}

        //if (!ModelState.IsValid)
        //{// тут только проверка на email ли строка model.Email
        //    return CBA_BadRequest(General.ServerErrors.Response.Reg_Email_Bad);
        //    //return BadRequest(ModelState);
        //}

        //if (!GF.IsValidEmail(model.Email))
        //{
        //    return CBA_BadRequest(General.ServerErrors.Response.Reg_Email_Bad);
        //}

        //// ТУТ СДЕЛАТЬ ПРОВЕРКУ СЛОЖНОСТИ ПАРОЛЯ


        //// Проверка, существует ли пользователь с таким email
        //bool exist = await GF_DataBase.IsExistsEmail(model.Email);
        //if (!exist)
        //{
        //    return CBA_BadRequest(General.ServerErrors.Response.Reg_Email_Exists);
        //}


        //// Создание нового пользователя
        //await using MySqlConnection connection = new(General.DataBase.ConnectionString_UsersData);
        //await connection.OpenAsync();
        //const string sql = """
        //    INSERT INTO users (email, password_hash)
        //    VALUES (@email, @password_hash)
        //    """;

        //await using MySqlCommand command = new(sql, connection);
        //_ = command.Parameters.AddWithValue("@email", model.Email);
        //_ = command.Parameters.AddWithValue("@password_hash", PassHasher.Create(model.Email, model.Password));

        //int count = await command.ExecuteNonQueryAsync();

        //return count > 0 ? Ok() : CBA_BadRequest(General.ServerErrors.Response.Reg_Unknown);
        await Task.Delay(1);
        return Ok();
    }

}

