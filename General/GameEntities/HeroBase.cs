using System;
using static General.Enums;

namespace General.GameEntities;

/// <summary>
/// Представляет базовую сущность игрового героя с основными характеристиками.
/// Использует синтаксис первичного конструктора (Primary Constructor) C# 12/13.
/// </summary>
/// <param name="id">Уникальный идентификатор героя.</param>
/// <param name="name">Имя героя (на английском).</param>
/// <param name="rarity">Уровень редкости героя, определяемый перечислением <see cref="RarityLevel"/>.</param>
/// <param name="baseHealth">Базовое значение здоровья героя.</param>
/// <param name="baseAttack">Базовое значение атаки героя.</param>
public class HeroBaseEntity(Guid id, string name, RarityLevel rarity, float baseHealth, float baseAttack)
{

    /// <summary>
    /// Уникальный идентификатор героя.
    /// </summary>
    public Guid Id { get; } = id;

    /// <summary>
    /// Имя героя на английском языке.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Уровень редкости героя.
    /// </summary>
    public RarityLevel Rarity { get; } = rarity;

    /// <summary>
    /// Базовое значение здоровья героя.
    /// </summary>
    public float BaseHealth { get; } = baseHealth;

    /// <summary>
    /// Базовое значение атаки героя.
    /// </summary>
    public float BaseAttack { get; } = baseAttack;

}
