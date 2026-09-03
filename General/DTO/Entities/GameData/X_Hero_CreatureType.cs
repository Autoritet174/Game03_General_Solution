namespace General.DTO.Entities.GameData;

public class X_Hero_CreatureType
{
    public int id { get; set; }
    public int baseHeroId { get; set; }
    public BaseHero baseHero { get; set; } = null!;
    public int creatureTypeId { get; set; }
    public CreatureType creatureType { get; set; } = null!;
}
