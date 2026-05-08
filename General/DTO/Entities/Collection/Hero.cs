using General.DTO.Entities.GameData;
using System;

namespace General.DTO.Entities.Collection;

/// <summary>
/// Герой из коллекции игрока.
/// </summary>
public class Hero
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public long Version { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public int BaseHeroId { get; set; }
    public BaseHero BaseHero { get; set; } = null!;
    public string? GroupName { get; set; }
    public int Level { get; set; } = 1;
    public float Experience { get; set; } = 0;
    public float Health { get; set; }
    public float Strength { get; set; }
    public float Agility { get; set; }
    public float Intelligence { get; set; }
    public float CritChance { get; set; }
    public float CritMultiplier { get; set; }
    public float Haste { get; set; }
    public float Versality { get; set; }
    public float EndurancePhysical { get; set; }
    public float EnduranceMagical { get; set; }
    public float Initiative { get; set; }
    public Hero CreateCopy()
    {
        return (Hero)MemberwiseClone();
    }
}
