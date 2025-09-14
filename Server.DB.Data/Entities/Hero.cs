using static General.Enums;

namespace Server.DB.Data.Entities;

public class Hero
{
    public required Guid Id { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset UpdatedAt { get; set; }
    public required string Name { get; set; }
    public required RarityLevel Rarity { get; set; }
    public ICollection<HeroCreatureType> CreatureTypes { get; set; } = [];
    public required float BaseHealth { get; set; }
    public required float BaseAttack { get; set; }
}
