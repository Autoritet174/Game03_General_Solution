namespace General.DTO.Entities.GameData;

public class Slot
{
    public ESlot id { get; set; }
    public string name { get; set; } = string.Empty;
    public ESlotType slotTypeId { get; set; }
    public SlotType? slotType { get; set; }
    public bool mainSlot { get; set; }
}

