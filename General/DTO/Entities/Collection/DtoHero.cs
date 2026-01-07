using General.DTO.Entities.GameData;
using System;

namespace General.DTO.Entities.Collection;

/// <summary>
/// Герой из коллекции игрока.
/// </summary>
public class DtoHero(Guid id, Guid userId, int baseHeroId, string? groupName, int rarity, int level, long experienceNow, int strength, int agility, int intelligence, int critChance, int critPower, int haste, int versality, int edurancePhysical, int enduranceMagical, long health1000, Dice? damage, int resistDamagePhysical, int resistDamageMagical, DtoBaseHero? dtoBaseHero = null)
{
    public Guid Id { get; } = id;
    public Guid UserId { get; } = userId;
    public int BaseHeroId { get; } = baseHeroId;
    public DtoBaseHero? BaseHero { get; set; } = dtoBaseHero;
    public string? GroupName { get; } = groupName;
    public int Rarity { get; } = rarity;
    public int Level { get; } = level;
    public long ExperienceNow { get; } = experienceNow;


    #region Характеристики
    // --------------Характеристики числовые----------------
    public int Strength { get; } = strength;
    public int Agility { get; } = agility;
    public int Intelligence { get; } = intelligence;
    public int CritChance { get; } = critChance;
    public int CritPower { get; } = critPower;
    public int Haste { get; } = haste;
    public int Versality { get; } = versality;
    public int EndurancePhysical { get; } = edurancePhysical;
    public int EnduranceMagical { get; } = enduranceMagical;
    public long Health1000 { get; } = health1000;

    // --------------Характеристики кубика ДНД----------------
    public Dice? Damage { get; } = damage;
    #endregion Характеристики

    #region Resistances

    /// <summary> Сопротивление физическому урону. Выражается числом которое преобразовывается в проценты. </summary>
    public int ResistDamagePhysical { get; } = resistDamagePhysical;

    /// <summary> Сопротивление магическому урону. Выражается числом которое преобразовывается в проценты. </summary>
    public int ResistDamageMagical { get; } = resistDamageMagical;

    #endregion Resistances

}
