namespace General.DTO.Entities.GameData;

public class DtoSlotType(ESlotType id, string name, bool haveAltSlot)
{
    public ESlotType Id { get; } = id;
    public string Name { get; } = name;
    public bool HaveAltSlot { get; set; } = haveAltSlot;
}
