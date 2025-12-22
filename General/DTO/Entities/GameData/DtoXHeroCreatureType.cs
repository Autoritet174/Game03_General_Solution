namespace General.DTO.Entities.GameData;

public class DtoXHeroCreatureType(int dtoHeroId, int dtoCreatureTypeId)
{
    public int DtoHeroId { get; } = dtoHeroId;
    public DtoBaseHero? DtoBaseHero { get; set; } = null;

    public int DtoCreatureTypeId { get; } = dtoCreatureTypeId;
    public DtoCreatureType? DtoCreatureType { get; set; } = null;
}
