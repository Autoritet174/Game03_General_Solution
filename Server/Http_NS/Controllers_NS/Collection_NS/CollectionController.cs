using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Server.Jwt_NS;
using Server_DB_Postgres;

namespace Server.Http_NS.Controllers_NS.Collection_NS;

/// <summary> Контроллер для управления коллекциями пользователя. </summary>
public class CollectionController(DbContext_Game dbContext) : ControllerBaseApi
{
    /// <summary> Получает коллекцию текущего пользователя. </summary>
    [EnableRateLimiting("login"), HttpPost("All")]
    public async Task<IActionResult> All()
    {
        Guid? userId = User.GetGuid();
        if (userId == null)
        {
            return Unauthorized();
        }

        var heroes = dbContext.Heroes.AsNoTracking().Where(a => a.UserId == userId).Select(a => new
        {
            a.Id,
            a.BaseHeroId,
            a.Health,
            a.Attack,
            a.Strength,
            a.Agility,
            a.Intelligence,
            a.CritChance,
            a.CritPower,
            a.Haste,
            a.Versality,
            a.EndurancePhysical,
            a.EnduranceMagical,
            a.ResistDamagePhysical,
            a.ResistDamageMagical,
            a.Equipment1Id,
            a.Equipment2Id,
            a.Equipment3Id,
            a.Equipment4Id,
            a.Equipment5Id,
            a.Equipment6Id,
            a.Equipment7Id,
            a.Equipment8Id,
            a.Equipment9Id,
            a.Equipment10Id,
            a.Equipment11Id,
            a.Equipment12Id,
            a.Equipment13Id,
            a.Equipment14Id,
            a.Equipment15Id,
            a.Equipment16Id,
            a.Equipment17Id,
            a.Equipment18Id,
            a.Equipment19Id,
            a.Equipment20Id,
            a.Equipment21Id,
            a.Equipment22Id,
            a.Equipment23Id,
            a.Equipment24Id,

        });
        var equipments = dbContext.Equipments.AsNoTracking().Where(a => a.UserId == userId).Select(a => new
        {
            a.Id,
            a.BaseEquipmentId
        });

        return Ok(new { heroes, equipments });
    }
}
