namespace General.DTO.Entities.GameData;

public class DtoSlot(int id, string name, int slotTypeId)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public int SlotTypeId { get; } = slotTypeId;
    public DtoSlotType? SlotType { get; set; } = null;
}
