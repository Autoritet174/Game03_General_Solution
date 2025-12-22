using General.DTO.Entities;
using General.DTO.Entities.Collection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Jwt_NS;
using Server_DB_Postgres;

namespace Server.Http_NS.Controllers_NS.Collection_NS;

/// <summary> Контроллер для управления коллекциями пользователя. </summary>
public class CollectionController(DbContext_Game dbContext) : ControllerBaseApi
{
    /// <summary> Получает коллекцию текущего пользователя. </summary>
    [EnableRateLimiting(Consts.RATE_LIMITER_POLICY_COLLECTION), HttpPost(nameof(General.Url.Collection.All))]
    public async Task<IActionResult> GetAllCollection()
    {
        Guid? userId = User.GetGuid();
        if (userId == null)
        {
            return Unauthorized();
        }

        List<DtoCollectionEquipment> equipments = await dbContext.Equipments.AsNoTracking().Where(a => a.UserId == userId).Select(a => new
        DtoCollectionEquipment
        {
            Id = a.Id,
            UserId = a.UserId,
            BaseEquipmentId = a.BaseEquipmentId,
            GroupName = a.GroupName
        }).ToListAsync();

        List<DtoCollectionHero> heroes = await dbContext.Heroes.AsNoTracking().Where(a => a.UserId == userId).Select(a => new DtoCollectionHero
        {
            Id = a.Id,
            BaseHeroId = a.BaseHeroId,
            UserId = a.UserId,
            Rarity = a.Rarity,
            GroupName = a.GroupName,
            Level = a.Level,
            ExperienceNow = a.ExperienceNow,
            Health1000 = a.Health1000,
            Strength = a.Strength,
            Agility = a.Agility,
            Intelligence = a.Intelligence,
            CritChance = a.CritChance,
            CritPower = a.CritPower,
            Haste = a.Haste,
            Versality = a.Versality,
            EndurancePhysical = a.EndurancePhysical,
            EnduranceMagical = a.EnduranceMagical,
            ResistDamagePhysical = a.ResistDamagePhysical,
            ResistDamageMagical = a.ResistDamageMagical,
            Damage = a.Damage,
            Equipment1Id = a.Equipment1Id,
            Equipment2Id = a.Equipment2Id,
            Equipment3Id = a.Equipment3Id,
            Equipment4Id = a.Equipment4Id,
            Equipment5Id = a.Equipment5Id,
            Equipment6Id = a.Equipment6Id,
            Equipment7Id = a.Equipment7Id,
            Equipment8Id = a.Equipment8Id,
            Equipment9Id = a.Equipment9Id,
            Equipment10Id = a.Equipment10Id,
            Equipment11Id = a.Equipment11Id,
            Equipment12Id = a.Equipment12Id,
            Equipment13Id = a.Equipment13Id,
            Equipment14Id = a.Equipment14Id,
            Equipment15Id = a.Equipment15Id,
            Equipment16Id = a.Equipment16Id,
            Equipment17Id = a.Equipment17Id,
            Equipment18Id = a.Equipment18Id,
            Equipment19Id = a.Equipment19Id,
            Equipment20Id = a.Equipment20Id,
            Equipment21Id = a.Equipment21Id,
            Equipment22Id = a.Equipment22Id,
            Equipment23Id = a.Equipment23Id,
            Equipment24Id = a.Equipment24Id,
        }).ToListAsync();

        DtoContainerCollection container = new()
        {
            DtoCollectionEquipments = equipments,
            DtoCollectionHeros = heroes
        };

        string json = JsonConvert.SerializeObject(container, General.G.JsonSerializerSettings);
        return Ok(json);
        //return Ok();
    }

}
