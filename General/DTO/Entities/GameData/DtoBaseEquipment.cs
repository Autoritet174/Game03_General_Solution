using System.ComponentModel.DataAnnotations.Schema;

namespace General.DTO.Entities.GameData;

/// <summary> Представляет базовую сущность предмета. </summary>
public class DtoBaseEquipment(int id, string name, int rarity, bool isUnique, int equipmentTypeId, Dice? health, Dice? damage, DtoEquipmentType? equipmentType = null)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public int Rarity { get; } = rarity;
    public bool IsUnique { get; } = isUnique;
    public int EquipmentTypeId { get; } = equipmentTypeId;
    public DtoEquipmentType? EquipmentType { get; set; } = equipmentType;

    #region Характеристики
    [Column(TypeName = "jsonb")]
    public Dice? Health { get; } = health;

    [Column(TypeName = "jsonb")]
    public Dice? Damage { get; } = damage;
    #endregion Характеристики
}
