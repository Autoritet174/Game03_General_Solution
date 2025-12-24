using System.ComponentModel.DataAnnotations.Schema;

namespace General.DTO.Entities.GameData;

/// <summary> Представляет базовую сущность предмета. </summary>
public class DtoBaseEquipment(int id, string name, int rarity, bool isUnique, int dtoEquipmentTypeId, Dice? Health, Dice? Damage)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public int Rarity { get; } = rarity;
    public bool IsUnique { get; } = isUnique;
    public int DtoEquipmentTypeId { get; } = dtoEquipmentTypeId;
    public DtoEquipmentType? DtoEquipmentType { get; set; } = null;

    #region Характеристики
    [Column(TypeName = "jsonb")]
    public Dice? Health { get; set; } = Health;

    [Column(TypeName = "jsonb")]
    public Dice? Damage { get; set; } = Damage;
    #endregion Характеристики
}
