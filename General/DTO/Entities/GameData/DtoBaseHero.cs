namespace General.DTO.Entities.GameData;

/// <summary> Data Transfer Object. Представляет базовую сущность игрового героя с основными характеристиками. </summary>
public class DtoBaseHero(int id, string name, int rarity, bool isUnique, int mainStat, Dice health, Dice damage, Dice strength, Dice agility, Dice intelligence, Dice critChance, Dice critMultiplier, Dice haste, Dice versality, Dice endurancePhysical, Dice enduranceMagical, Dice initiative)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public int Rarity { get; } = rarity;
    public bool IsUnique { get; } = isUnique;
    public int MainStat { get; } = mainStat;

    #region Характеристики
    public Dice Health { get; } = health;
    public Dice Damage { get; } = damage;
    public Dice Strength { get; } = strength;
    public Dice Agility { get; } = agility;
    public Dice Intelligence { get; } = intelligence;
    public Dice CritChance { get; } = critChance;
    public Dice CritMultiplier { get; } = critMultiplier;
    public Dice Haste { get; } = haste;
    public Dice Versality { get; } = versality;
    public Dice EndurancePhysical { get; } = endurancePhysical;
    public Dice EnduranceMagical { get; } = enduranceMagical;
    public Dice Initiative { get; } = initiative;
    #endregion Характеристики

}
