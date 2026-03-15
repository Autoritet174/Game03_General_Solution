using System.ComponentModel.DataAnnotations.Schema;

namespace General.DTO.Entities.GameData;

/// <summary> Представляет базовую сущность предмета. </summary>
public class DtoBaseEquipment(int id, string name, int rarity, bool isUnique, int equipmentTypeId, Dice? health_1000, Dice? damage_1000, DtoEquipmentType? equipmentType = null)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public int Rarity { get; } = rarity;
    public bool IsUnique { get; } = isUnique;
    public int EquipmentTypeId { get; } = equipmentTypeId;
    public DtoEquipmentType? EquipmentType { get; set; } = equipmentType;

    #region Характеристики
    public Dice? Health_1000 { get; } = health_1000;

    public Dice? Damage_1000 { get; } = damage_1000;
    #endregion Характеристики
}
