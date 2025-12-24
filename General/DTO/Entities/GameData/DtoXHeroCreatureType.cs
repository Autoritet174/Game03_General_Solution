namespace General.DTO.Entities.GameData;

public class DtoXHeroCreatureType(int dtoBaseHeroId, int dtoCreatureTypeId)
{
    public int DtoBaseHeroId { get; } = dtoBaseHeroId;
    public DtoBaseHero? DtoBaseHero { get; set; } = null;

    public int DtoCreatureTypeId { get; } = dtoCreatureTypeId;
    public DtoCreatureType? DtoCreatureType { get; set; } = null;
}
