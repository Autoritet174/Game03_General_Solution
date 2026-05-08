namespace General.DTO.Entities.GameData;

public class Slot
{
    public ESlot Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ESlotType SlotTypeId { get; set; }
    public SlotType? SlotType { get; set; }
    public bool MainSlot { get; set; }
}

