namespace General.DTO.Entities.GameData;

public class DtoXHeroCreatureType(int baseHeroId, int creatureTypeId, DtoBaseHero? baseHero = null, DtoCreatureType? creatureType = null)
{
    public int BaseHeroId { get; } = baseHeroId;
    public DtoBaseHero? BaseHero { get; } = baseHero;

    public int CreatureTypeId { get; } = creatureTypeId;
    public DtoCreatureType? CreatureType { get; } = creatureType;
}
