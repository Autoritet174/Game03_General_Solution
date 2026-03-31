namespace General.DTO.Entities.GameData;

public class DtoSlotType(ESlotType id, string name, bool haveAltSlot, int Sorting)
{
    public ESlotType Id { get; } = id;
    public string Name { get; } = name;
    public bool HaveAltSlot { get; set; } = haveAltSlot;
    public int Sorting { get; set; } = Sorting;
}
