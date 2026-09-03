namespace General.DTO.Entities.GameData;

public class BaseHero(int id, string name, int rarity, bool isUnique, EMainStat mainStat, bool isPlayable, Dice health, Dice damage, Dice strength, Dice agility, Dice intelligence, Dice critChance, Dice critMultiplier, Dice haste, Dice versality, Dice endurancePhysical, Dice enduranceMagical, Dice initiative)
{
    public int id { get; set; } = id;
    public string name { get; set; } = name;
    public int rarity { get; set; } = rarity;
    public bool isUnique { get; set; } = isUnique;
    public EMainStat mainStat { get; set; } = mainStat;
    public bool isPlayable { get; set; } = isPlayable;
    public Dice health { get; set; } = health;
    public Dice damage { get; set; } = damage;
    public Dice strength { get; set; } = strength;
    public Dice agility { get; set; } = agility;
    public Dice intelligence { get; set; } = intelligence;
    public Dice critChance { get; set; } = critChance;
    public Dice critMultiplier { get; set; } = critMultiplier;
    public Dice haste { get; set; } = haste;
    public Dice versality { get; set; } = versality;
    public Dice endurancePhysical { get; set; } = endurancePhysical;
    public Dice enduranceMagical { get; set; } = enduranceMagical;
    public Dice initiative { get; set; } = initiative;
}
