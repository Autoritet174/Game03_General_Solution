namespace General.DTO.Entities.GameData;

public class DtoSlotType(int id, string name, bool haveAltSlot)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public bool HaveAltSlot { get; set; } = haveAltSlot;
}
