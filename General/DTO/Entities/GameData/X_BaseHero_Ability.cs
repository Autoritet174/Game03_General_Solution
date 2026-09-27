namespace General.DTO.Entities.GameData;

public class X_BaseHero_Ability
{
    public int id { get; set; }
    public int baseHeroId { get; set; }
    public BaseHero baseHero { get; set; } = null!;
    public int abilityId { get; set; }
    public Ability ability { get; set; } = null!;
}
