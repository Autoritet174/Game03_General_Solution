using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace General.DTO.Entities.GameData;

/// <summary> Представляет базовую сущность предмета. </summary>
public class DtoBaseEquipment(int id, string name, int rarity, bool isUnique, int equipmentTypeId, DtoEquipmentType? equipmentType, Dictionary<EStatType, Dice>? PossibleStats)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public int Rarity { get; } = rarity;
    public bool IsUnique { get; } = isUnique;
    public int EquipmentTypeId { get; } = equipmentTypeId;
    public DtoEquipmentType? EquipmentType { get; set; } = equipmentType;

    public Dictionary<EStatType, Dice>? PossibleStats { get; set; } = PossibleStats;
}
