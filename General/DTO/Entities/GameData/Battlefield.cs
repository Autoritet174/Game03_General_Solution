namespace General.DTO.Entities.GameData;

public class Battlefield
{
    public required EBattlefield id { get; init; }
    public required string name { get; init; }
    public required string enumName { get; init; }
    public required int maxHeroCount { get; init; }
    public required int maxEnemyCount { get; init; }
}
