namespace General.DTO.Entities.GameData;

public class X_Hero_CreatureType
{
    public int Id { get; set; }
    public int BaseHeroId { get; set; }
    public BaseHero BaseHero { get; set; } = null!;
    public int CreatureTypeId { get; set; }
    public CreatureType CreatureType { get; set; } = null!;
}
