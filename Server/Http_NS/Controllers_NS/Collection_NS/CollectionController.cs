using General;
using General.DTO.Entities;
using General.DTO.Entities.Collection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Server.Jwt_NS;
using Server_DB_Postgres;
using System.Text.Json;

namespace Server.Http_NS.Controllers_NS.Collection_NS;

/// <summary> Контроллер для управления коллекциями пользователя. </summary>
public class CollectionController(DbContextGame dbContext) : ControllerBaseApi
{
    /// <summary> Получает коллекцию текущего пользователя. </summary>
    [EnableRateLimiting(Consts.RATE_LIMITER_POLICY_COLLECTION), HttpPost("All")]
    public async Task<IActionResult> GetAllCollectionAsync(CancellationToken cancellationToken)
    {
        //"https://localhost:7227/api/Collection/All"
        Guid? userId = User.GetGuid();
        if (userId == null)
        {
            return Unauthorized();
        }

        List<DtoEquipment> equipments = await dbContext.Equipments.AsNoTracking().Where(a => a.UserId == userId).Select(a => new
            DtoEquipment(a.Id, a.UserId, a.BaseEquipmentId, a.GroupName, a.HeroId, a.SlotId)).ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        List<DtoHero> heroes = await dbContext.Heroes.AsNoTracking().Where(a => a.UserId == userId).Select(a => new DtoHero(a.Id, a.UserId, a.BaseHeroId, a.GroupName, a.Level, a.ExperienceNow, a.Strength_1000, a.Agility_1000, a.Intelligence_1000, a.CritChance_1000, a.CritMultiplier_1000, a.Haste_1000, a.Versality_1000, a.EndurancePhysical_1000, a.EnduranceMagical_1000, a.Health_1000, a.Initiative_1000)).ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        DtoContainerCollection container = new(equipments, heroes);
        return Ok(JsonSerializer.Serialize(container, GlobalJsonOptions.jsonOptions));
    }

}
