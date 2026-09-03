using General.DTO.Entities.GameData;
using General.DTO.Interfaces;

namespace General.DTO.Entities.Collection;

/// <summary>
/// Герой из коллекции игрока.
/// </summary>
public class Hero : ICreatedAt, IUpdatedAt, IVersion
{
    public Guid id { get; set; }
    public required Guid userId { get; init; }
    public long version { get; set; }
    public DateTimeOffset createdAt { get; set; }
    public DateTimeOffset updatedAt { get; set; }
    public required int baseHeroId { get; init; }
    public BaseHero baseHero { get; set; } = null!;
    public string? groupName { get; set; }
    public int level { get; set; } = 1;
    public float experience { get; set; } = 0;
    public required float health { get; init; }
    public required float strength { get; init; }
    public required float agility { get; init; }
    public required float intelligence { get; init; }
    public required float critChance { get; init; }
    public required float critMultiplier { get; init; }
    public required float haste { get; init; }
    public required float versality { get; init; }
    public required float endurancePhysical { get; init; }
    public required float enduranceMagical { get; init; }
    public required float initiative { get; init; }
    public required float damage { get; init; }

    public Hero CreateCopy()
    {
        return (Hero)MemberwiseClone();
    }
}
