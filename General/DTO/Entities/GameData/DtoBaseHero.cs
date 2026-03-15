using System.ComponentModel.DataAnnotations.Schema;

namespace General.DTO.Entities.GameData;

/// <summary> Data Transfer Object. Представляет базовую сущность игрового героя с основными характеристиками. </summary>
public class DtoBaseHero(int id, string name, int rarity, bool isUnique, int mainStat, Dice health_1000, Dice damage_1000, Dice strength_1000, Dice agility_1000, Dice intelligence_1000, Dice critChance_1000, Dice critMultiplier_1000, Dice haste_1000, Dice versality_1000, Dice endurancePhysical_1000, Dice enduranceMagical_1000, Dice initiative_1000)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public int Rarity { get; } = rarity;
    public bool IsUnique { get; } = isUnique;
    public int MainStat { get; } = mainStat;

    #region Характеристики
    public Dice Health_1000 { get; } = health_1000;
    public Dice Damage_1000 { get; } = damage_1000;
    public Dice Strength_1000 { get; } = strength_1000;
    public Dice Agility_1000 { get; } = agility_1000;
    public Dice Intelligence_1000 { get; } = intelligence_1000;
    public Dice CritChance_1000 { get; } = critChance_1000;
    public Dice CritMultiplier_1000 { get; } = critMultiplier_1000;
    public Dice Haste_1000 { get; } = haste_1000;
    public Dice Versality_1000 { get; } = versality_1000;
    public Dice EndurancePhysical_1000 { get; } = endurancePhysical_1000;
    public Dice EnduranceMagical_1000 { get; } = enduranceMagical_1000;
    public Dice Initiative_1000 { get; } = initiative_1000;
    #endregion Характеристики

}
