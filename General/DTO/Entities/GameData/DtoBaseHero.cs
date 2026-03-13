using System.ComponentModel.DataAnnotations.Schema;

namespace General.DTO.Entities.GameData;

/// <summary> Data Transfer Object. Представляет базовую сущность игрового героя с основными характеристиками. </summary>
public class DtoBaseHero(int id, string name, int rarity, bool isUnique, int mainStat, Dice health1000, Dice damage1000, Dice strength1000, Dice agility1000, Dice intelligence1000, Dice critChance1000, Dice critPower1000, Dice haste1000, Dice versality1000, Dice endurancePhysical1000, Dice enduranceMagical1000, Dice initiative1000)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public int Rarity { get; } = rarity;
    public bool IsUnique { get; } = isUnique;
    public int MainStat { get; } = mainStat;

    #region Характеристики
    public Dice Health1000 { get; } = health1000;
    public Dice Damage1000 { get; } = damage1000;
    public Dice Strength1000 { get; } = strength1000;
    public Dice Agility1000 { get; } = agility1000;
    public Dice Intelligence1000 { get; } = intelligence1000;
    public Dice CritChance1000 { get; } = critChance1000;
    public Dice CritPower1000 { get; } = critPower1000;
    public Dice Haste1000 { get; } = haste1000;
    public Dice Versality1000 { get; } = versality1000;
    public Dice EndurancePhysical1000 { get; } = endurancePhysical1000;
    public Dice EnduranceMagical1000 { get; } = enduranceMagical1000;
    public Dice Initiative1000 { get; } = initiative1000;
    #endregion Характеристики

}
