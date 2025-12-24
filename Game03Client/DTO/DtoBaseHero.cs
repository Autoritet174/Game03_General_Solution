using General;

namespace Game03Client.DTO;

/// <summary> Представляет базовую сущность игрового героя с основными характеристиками. </summary>
public class DtoBaseHero(int id, string name, int rarity, Dice health, Dice damage)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public int Rarity { get; } = rarity;
    public Dice Health { get; } = health;
    public Dice Damage { get; } = damage;

}
