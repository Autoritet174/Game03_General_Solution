namespace General.DTO.Entities.GameData;

public class SlotType
{
    public ESlotType id { get; set; }
    public string name { get; set; } = string.Empty;
    public string? nameRu { get; set; }
    public bool haveAltSlot { get; set; }
    public int sorting { get; set; }
}
