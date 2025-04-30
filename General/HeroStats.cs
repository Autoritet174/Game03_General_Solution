namespace General;

/// <summary>
/// Базовые характеристики героя
/// </summary>
public class HeroStats(int id, string name, float baseHealth, float baseAttack) {
    public int Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public float BaseHealth { get; private set; } = baseHealth;
    public float BaseAttack { get; private set; } = baseAttack;
}
