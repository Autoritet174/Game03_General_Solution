namespace General.DTO.Entities.GameData;

public class BaseHero(int Id, string Name, int Rarity, bool IsUnique, EMainStat MainStat, bool IsPlayable, Dice Health, Dice Damage, Dice Strength, Dice Agility, Dice Intelligence, Dice CritChance, Dice CritMultiplier, Dice Haste, Dice Versality, Dice EndurancePhysical, Dice EnduranceMagical, Dice Initiative)
{
    public int Id { get; set; } = Id;
    public string Name { get; set; } = Name;
    public int Rarity { get; set; } = Rarity;
    public bool IsUnique { get; set; } = IsUnique;
    public EMainStat MainStat { get; set; } = MainStat;
    public bool IsPlayable { get; set; } = IsPlayable;
    public Dice Health { get; set; } = Health;
    public Dice Damage { get; set; } = Damage;
    public Dice Strength { get; set; } = Strength;
    public Dice Agility { get; set; } = Agility;
    public Dice Intelligence { get; set; } = Intelligence;
    public Dice CritChance { get; set; } = CritChance;
    public Dice CritMultiplier { get; set; } = CritMultiplier;
    public Dice Haste { get; set; } = Haste;
    public Dice Versality { get; set; } = Versality;
    public Dice EndurancePhysical { get; set; } = EndurancePhysical;
    public Dice EnduranceMagical { get; set; } = EnduranceMagical;
    public Dice Initiative { get; set; } = Initiative;
}
