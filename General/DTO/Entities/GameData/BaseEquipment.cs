using System.Collections.Generic;

namespace General.DTO.Entities.GameData;

/// <summary> Представляет базовую сущность предмета. </summary>
public class BaseEquipment(int Id, string Name, int Rarity, bool IsUnique, int EquipmentTypeId, int? SmithingMaterialId, Dictionary<EStatType, Dice>? PossibleStats)
{
    public int Id { get; set; } = Id;
    public string Name { get; set; } = Name;
    public int Rarity { get; set; } = Rarity;
    public bool IsUnique { get; set; } = IsUnique;
    public int EquipmentTypeId { get; set; } = EquipmentTypeId;
    public EquipmentType EquipmentType { get; set; } = null!;
    public int? SmithingMaterialId { get; set; } = SmithingMaterialId;
    public SmithingMaterial? SmithingMaterial { get; set; }
    public Dictionary<EStatType, Dice>? PossibleStats { get; set; } = PossibleStats;
}
