using System.Collections.Generic;

namespace General.DTO.Entities.GameData;

/// <summary> Представляет базовую сущность предмета. </summary>
public class BaseEquipment(int id, string name, int rarity, bool isUnique, int equipmentTypeId, int? smithingMaterialId, Dictionary<EStatType, Dice>? possibleStats)
{
    public int id { get; set; } = id;
    public string name { get; set; } = name;
    public int rarity { get; set; } = rarity;
    public bool isUnique { get; set; } = isUnique;
    public int equipmentTypeId { get; set; } = equipmentTypeId;
    public EquipmentType equipmentType { get; set; } = null!;
    public int? smithingMaterialId { get; set; } = smithingMaterialId;
    public SmithingMaterial? smithingMaterial { get; set; }
    public Dictionary<EStatType, Dice>? possibleStats { get; set; } = possibleStats;
}
