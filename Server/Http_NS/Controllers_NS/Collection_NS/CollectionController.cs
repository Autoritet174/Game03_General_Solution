using General;
using General.DTO.Entities;
using General.DTO.Entities.Collection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Server.Jwt_NS;
using Server_DB_Postgres;

namespace Server.Http_NS.Controllers_NS.Collection_NS;

/// <summary> Контроллер для управления коллекциями пользователя. </summary>
public class CollectionController(DbContextGame dbContext) : ControllerBaseApi
{
    /// <summary> Получает коллекцию текущего пользователя. </summary>
    [EnableRateLimiting(Consts.RATE_LIMITER_POLICY_COLLECTION), HttpPost("All")]
    public async Task<IActionResult> GetAllCollectionAsync(CancellationToken cancellationToken)
    {
        Guid? userId = User.GetGuid();
        if (userId == null)
        {
            return Unauthorized();
        }

        List<Equipment> equipments = [.. dbContext.Equipments.AsNoTracking().Where(a => a.UserId == userId)];

        List<Hero> heroes = [.. dbContext.Heroes.AsNoTracking().Where(a => a.UserId == userId)];

        DtoContainerCollection container = new()
        {
            CollectionEquipments = equipments,
            CollectionHeroes = heroes
        };
        return Ok(JSON.Serialize(container));
    }

}
