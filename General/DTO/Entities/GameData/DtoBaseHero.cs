using System.ComponentModel.DataAnnotations.Schema;

namespace General.DTO.Entities.GameData;

/// <summary> Data Transfer Object. Представляет базовую сущность игрового героя с основными характеристиками. </summary>
public class DtoBaseHero(int id, string name, int rarity, bool isUnique, int mainStat, Dice? Health, Dice? Damage)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public int Rarity { get; } = rarity;
    public bool IsUnique { get; } = isUnique;
    public int MainStat { get; } = mainStat;

    #region Характеристики
    [Column(TypeName = "jsonb")]
    public Dice? Health { get; set; } = Health;

    [Column(TypeName = "jsonb")]
    public Dice? Damage { get; set; } = Damage;
    #endregion Характеристики

}
