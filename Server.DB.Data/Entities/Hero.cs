using System.ComponentModel.DataAnnotations;

namespace Server.DB.Data.Entities;

public class Hero
{
    public enum RarityLevel : int
    {
        Common = 1,
        Uncommon = 2,
        Rare = 3,
        Epic = 4,
        Legendary = 5
    }
    public required Guid Id { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset UpdatedAt { get; set; }
    public required string Name { get; set; }

    [Range(1, 5)]
    public required RarityLevel Rarity { get; set; }

    public ICollection<HeroCreatureType> CreatureTypes { get; set; } = [];

}
