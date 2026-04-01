using General.DTO.Entities.GameData;
using System;

namespace General.DTO.Entities.Collection;

/// <summary>
/// Герой из коллекции игрока.
/// </summary>
public class DtoHero(Guid id, Guid userId, int baseHeroId, string? groupName, int level, float experienceNow, float strength, float agility, float intelligence, float critChance, float critMultiplier, float haste, float versality, float edurancePhysical, float enduranceMagical, float health, float initiative, DtoBaseHero? dtoBaseHero)
{
    public Guid Id { get; } = id;
    public Guid UserId { get; } = userId;
    public int BaseHeroId { get; } = baseHeroId;
    public DtoBaseHero? BaseHero { get; set; } = dtoBaseHero;
    public string? GroupName { get; } = groupName;
    public int Level { get; } = level;
    public float ExperienceNow { get; } = experienceNow;


    #region Характеристики
    // --------------Характеристики числовые----------------
    public float Health { get; } = health;
    public float Strength { get; } = strength;
    public float Agility { get; } = agility;
    public float Intelligence { get; } = intelligence;
    public float CritChance { get; } = critChance;
    public float CritMultiplier { get; } = critMultiplier;
    public float Haste { get; } = haste;
    public float Versality { get; } = versality;
    public float EndurancePhysical { get; } = edurancePhysical;
    public float EnduranceMagical { get; } = enduranceMagical;
    public float Initiative { get; } = initiative;
    #endregion Характеристики

    #region Resistances


    #endregion Resistances

    public DtoHero CreateCopy()
    {
        return (DtoHero)MemberwiseClone();
    }
}
