namespace General.DTO.Entities.GameData;

public class DtoSlot(ESlot id, string name, ESlotType slotTypeId)
{
    public ESlot Id { get; } = id;
    public string Name { get; } = name;
    public ESlotType SlotTypeId { get; } = slotTypeId;
    public DtoSlotType? SlotType { get; set; } = null;
}
