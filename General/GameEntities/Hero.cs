using static General.GameEntities.HeroBaseEntity;

namespace General.GameEntities;

public class HeroBaseEntity(string name, RarityLevel rarity)
{
    public enum RarityLevel : int
    {
        Common = 1,
        Uncommon = 2,
        Rare = 3,
        Epic = 4,
        Legendary = 5,
        Mythic = 6,
    }
    public string Name { get; } = name;
    public RarityLevel Rarity { get; set; } = rarity;
}
