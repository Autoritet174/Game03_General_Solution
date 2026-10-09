using System.Collections.Generic;

namespace General.DTO.Entities.GameData;

public class BaseHero()
{
    public required int id { get; set; }
    public required string name { get; set; }
    public required int rarity { get; set; }
    public required bool isUnique { get; set; }
    public required EMainStat mainStat { get; set; }
    public required bool isPlayable { get; set; }
    public required Dice health { get; set; }
    public required Dice damage { get; set; }
    public required Dice strength { get; set; }
    public required Dice agility { get; set; }
    public required Dice intelligence { get; set; }
    public required Dice critChance { get; set; }
    public required Dice critMultiplier { get; set; }
    public required Dice haste { get; set; }
    public required Dice versality { get; set; }
    public required Dice endurancePhysical { get; set; }
    public required Dice enduranceMagical { get; set; }
    public required Dice initiative { get; set; }
    public required Dice threat { get; set; }
    public required List<Ability> abilities { get; set; } = [];
}
