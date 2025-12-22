namespace General.DTO.Entities.GameData;

/// <summary> Представляет базовую сущность предмета. </summary>
public class DtoBaseEquipment(int id, string name, int rarity, bool isUnique, int dtoEquipmentTypeId, Stats? stats)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public int Rarity { get; } = rarity;
    public bool IsUnique { get; } = isUnique;
    public int DtoEquipmentTypeId { get; } = dtoEquipmentTypeId;
    public DtoEquipmentType? DtoEquipmentType { get; set; } = null;
    public Stats? Stats { get; } = stats;
}
