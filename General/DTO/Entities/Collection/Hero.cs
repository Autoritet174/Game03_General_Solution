using General.DTO.Entities.GameData;
using General.DTO.Interfaces;

namespace General.DTO.Entities.Collection;

/// <summary>
/// Герой из коллекции игрока.
/// </summary>
public class Hero : ICreatedAt, IUpdatedAt, IVersion
{
    public Guid Id { get; set; }
    public required Guid UserId { get; init; }
    public long Version { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public required int BaseHeroId { get; init; }
    public BaseHero BaseHero { get; set; } = null!;
    public string? GroupName { get; set; }
    public int Level { get; set; } = 1;
    public float Experience { get; set; } = 0;
    public required float Health { get; init; }
    public required float Strength { get; init; }
    public required float Agility { get; init; }
    public required float Intelligence { get; init; }
    public required float CritChance { get; init; }
    public required float CritMultiplier { get; init; }
    public required float Haste { get; init; }
    public required float Versality { get; init; }
    public required float EndurancePhysical { get; init; }
    public required float EnduranceMagical { get; init; }
    public required float Initiative { get; init; }

    public Hero CreateCopy()
    {
        return (Hero)MemberwiseClone();
    }
}
