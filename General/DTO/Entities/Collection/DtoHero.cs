using General.DTO.Entities.GameData;
using System;

namespace General.DTO.Entities.Collection;

/// <summary>
/// Герой из коллекции игрока.
/// </summary>
public class DtoHero(Guid id, Guid userId, int baseHeroId, string? groupName, int level, long experienceNow, long strength1000, long agility1000, long intelligence1000, long critChance1000, long critPower1000, long haste1000, long versality1000, long edurancePhysical1000, long enduranceMagical1000, long health1000, DtoBaseHero? dtoBaseHero = null)
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
    public long Health1000 { get; } = health1000;
    public long Strength1000 { get; } = strength1000;
    public long Agility1000 { get; } = agility1000;
    public long Intelligence1000 { get; } = intelligence1000;
    public long CritChance1000 { get; } = critChance1000;
    public long CritPower1000 { get; } = critPower1000;
    public long Haste1000 { get; } = haste1000;
    public long Versality1000 { get; } = versality1000;
    public long EndurancePhysical1000 { get; } = edurancePhysical1000;
    public long EnduranceMagical1000 { get; } = enduranceMagical1000;
    #endregion Характеристики

    #region Resistances


    #endregion Resistances

}
