namespace General.DTO.Entities.GameData;

public class DtoCreatureType(int id, string name)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
}
