using System;
using static General.Enums;

namespace General.GameEntities;

public class HeroBaseEntity(Guid id, string name, Enums.RarityLevel rarity, float baseHealth, float baseAttack)
{
    /// <summary>
    /// Id героя.
    /// </summary>
    public Guid Id { get; } = id;

    /// <summary>
    /// Имя героя на англ.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Редкость героя.
    /// </summary>
    public RarityLevel Rarity { get; } = rarity;

    /// <summary>
    /// Здоровье.
    /// </summary>
    public float BaseHealth { get; } = baseHealth;

    /// <summary>
    /// Атака.
    /// </summary>
    public float BaseAttack { get; } = baseAttack;

}
