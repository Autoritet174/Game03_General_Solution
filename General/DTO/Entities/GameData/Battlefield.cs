namespace General.DTO.Entities.GameData;

public class Battlefield
{
    public required EBattleFiled Id { get; init; }
    public required string Name { get; init; }
    public required string EnumName { get; init; }
    public required int MaxHeroCount { get; init; }
    public required int MaxEnemyCount { get; init; }
}
