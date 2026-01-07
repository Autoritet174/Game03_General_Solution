using System.ComponentModel.DataAnnotations.Schema;

namespace General.DTO.Entities.GameData;

/// <summary> Data Transfer Object. Представляет базовую сущность игрового героя с основными характеристиками. </summary>
public class DtoBaseHero(int id, string name, int rarity, bool isUnique, int mainStat, Dice? health, Dice? damage)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public int Rarity { get; } = rarity;
    public bool IsUnique { get; } = isUnique;
    public int MainStat { get; } = mainStat;

    #region Характеристики
    [Column(TypeName = "jsonb")]
    public Dice? Health { get; } = health;

    [Column(TypeName = "jsonb")]
    public Dice? Damage { get; } = damage;
    #endregion Характеристики

}
