using System;
using static General.Enums;

namespace General.GameEntities;

public class HeroBaseEntity(Guid id, string name, RarityLevel rarity)
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public RarityLevel Rarity { get; } = rarity;
}
