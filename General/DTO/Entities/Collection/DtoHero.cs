using General.DTO.Entities.GameData;
using System;

namespace General.DTO.Entities.Collection;

/// <summary>
/// Герой из коллекции игрока.
/// </summary>
public class DtoHero(Guid id, Guid userId, int baseHeroId, string? groupName, int level, long experienceNow, long strength_1000, long agility_1000, long intelligence_1000, long critChance_1000, long critMultiplier_1000, long haste_1000, long versality_1000, long edurancePhysical_1000, long enduranceMagical_1000, long health_1000, long initiative_1000, DtoBaseHero? dtoBaseHero = null)
{
    public Guid Id { get; } = id;
    public Guid UserId { get; } = userId;
    public int BaseHeroId { get; } = baseHeroId;
    public DtoBaseHero? BaseHero { get; set; } = dtoBaseHero;
    public string? GroupName { get; } = groupName;
    public int Level { get; } = level;
    public long ExperienceNow { get; } = experienceNow;


    #region Характеристики
    // --------------Характеристики числовые----------------
    public long Health_1000 { get; } = health_1000;
    public long Strength_1000 { get; } = strength_1000;
    public long Agility_1000 { get; } = agility_1000;
    public long Intelligence_1000 { get; } = intelligence_1000;
    public long CritChance_1000 { get; } = critChance_1000;
    public long CritMultiplier_1000 { get; } = critMultiplier_1000;
    public long Haste_1000 { get; } = haste_1000;
    public long Versality_1000 { get; } = versality_1000;
    public long EndurancePhysical_1000 { get; } = edurancePhysical_1000;
    public long EnduranceMagical_1000 { get; } = enduranceMagical_1000;
    public long Initiative_1000 { get; } = initiative_1000;
    #endregion Характеристики

    #region Resistances


    #endregion Resistances

}
