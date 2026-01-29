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
    public async Task<IActionResult> GetAllCollection()
    {
        //"https://localhost:7227/api/Collection/All"
        Guid? userId = User.GetGuid();
        if (userId == null)
        {
            return Unauthorized();
        }

        List<DtoEquipment> equipments = await dbContext.Equipments.AsNoTracking().Where(a => a.UserId == userId).Select(a => new
        DtoEquipment(a.Id, a.UserId, a.BaseEquipmentId, a.GroupName, a.SlotId, a.HeroId)).ToListAsync();

        List<DtoHero> heroes = await dbContext.Heroes.AsNoTracking().Where(a => a.UserId == userId).Select(a => new DtoHero(a.Id, a.UserId, a.BaseHeroId, a.GroupName, a.Rarity, a.Level, a.ExperienceNow, a.Strength, a.Agility, a.Intelligence, a.CritChance, a.CritPower, a.Haste, a.Versality, a.EndurancePhysical, a.EnduranceMagical, a.Health1000, a.Damage, a.ResistDamagePhysical, a.ResistDamageMagical)).ToListAsync();

        DtoContainerCollection container = new(equipments, heroes);
        return Ok(JsonSerializer.Serialize(container, GlobalJsonOptions.jsonOptions));
    }

}
