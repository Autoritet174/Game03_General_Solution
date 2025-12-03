namespace Server_DB_Data.Entities;

/// <summary>
/// Базовая версия героя.
/// </summary>
public class Hero : Entity
{
#pragma warning disable
    public ICollection<HeroCreatureType> CreatureTypes { get; set; } = [];
    public required string Health { get; set; }
    public required string Attack { get; set; }
}
