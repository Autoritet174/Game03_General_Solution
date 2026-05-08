namespace General.DTO.Entities.GameData;

public class DamageType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NameRu { get; set; }
    public string? DevHintRu { get; set; }
    public int Category { get; set; } = 0;
}
