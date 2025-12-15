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
public class EquipBase(int id, string name, RarityLevel rarity, int mass, int slotTypeId, bool canCraftJewelcrafting, bool canCraftSmithing, string attack)
{

    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public int Id { get; } = id;

    /// <summary>
    /// Наименование на английском языке.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Уровень редкости.
    /// </summary>
    public RarityLevel Rarity { get; } = rarity;

    public int Mass { get; } = mass;
    public int SlotTypeId { get; } = slotTypeId;
    public bool CanCraftJewelcrafting { get; } = canCraftJewelcrafting;
    public bool CanCraftSmithing { get; } = canCraftSmithing;
    public string Attack { get; } = attack;


}
