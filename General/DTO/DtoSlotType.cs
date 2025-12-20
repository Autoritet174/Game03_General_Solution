namespace General.DTO;

/// <summary>
/// Data Transfer Object. SlotType.
/// </summary>
/// <param name="id"></param>
/// <param name="name"></param>
public class DtoSlotType(int id, string name)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
}
