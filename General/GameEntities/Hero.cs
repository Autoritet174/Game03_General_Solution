using System;
using static General.Enums;

namespace General.GameEntities;

public class HeroBaseEntity(Guid id, string name, Enums.RarityLevel rarity, float baseHealth, float baseAttack)
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public RarityLevel Rarity { get; } = rarity;
    public float BaseHealth { get; } = baseHealth;
    public float BaseAttack { get; } = baseAttack;

}
