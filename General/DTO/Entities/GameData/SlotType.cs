namespace General.DTO.Entities.GameData;

public class SlotType
{
    public ESlotType Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NameRu { get; set; }
    public bool HaveAltSlot { get; set; }
    public int Sorting { get; set; }
}
