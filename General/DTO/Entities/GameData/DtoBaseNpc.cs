namespace General.DTO.Entities.GameData;

public class DtoBaseNpc(int id, string name, int rarity, EMainStat MainStat, float health)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public int Rarity { get; set; } = rarity;
    public EMainStat MainStat { get; set; } = MainStat;
    public float Health { get; set; } = health;
}
