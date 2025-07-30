using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using Server.DB;
using Server.DB.Data.Repositories;
using Server.DB.Users.Entities;
using Server.GameDataCache;
using Server.Utilities;
using System.Text;
using System.Text.Json.Nodes;

namespace Server.Http_NS.Controllers_NS;


/// <summary>
/// Контроллер для аутентификации пользователей.
/// </summary>
public class GetListAllHeroesController(HeroRepository heroRepository) : ControllerBaseApi
{
    private readonly HeroRepository _heroRepository = heroRepository;

    /// <summary>
    /// Возвращает в ответе список всех героев.
    /// </summary>
    /// <param name="request">Данные для входа.</param>
    /// <returns>JWT-токен при успешной аутентификации или код ошибки.</returns>
    public async Task<IActionResult> GetListAllHeroes()
    {
        await GF.DelayWithOutDebug500();

        //return Ok(new { ListAllHeroes.Json });
        return Content(ListAllHeroes.Json, "application/json");
    }



}
