using System;

namespace Game03Client.DTO;

/// <summary>
/// Герой из коллекции игрока.
/// </summary>
public class DtoCollectionHero(Guid id, Guid userId, DtoBaseHero heroBase, string groupName, int rarity, long health, long attack, int strength, int agility, int intelligence, int haste, int level, long experienceNow)
{
    public Guid Id { get; } = id;
    public Guid UserId { get; } = userId;
    public DtoBaseHero HeroBase { get; } = heroBase;
    public string GroupName { get; } = groupName;
    public int Rarity { get; } = rarity;
    public long Health { get; } = health;
    public long Attack { get; } = attack;
    public int Strength { get; } = strength;
    public int Agility { get; } = agility;
    public int Intelligence { get; } = intelligence;
    public int Haste { get; } = haste;
    public int Level { get; } = level;
    public long ExperienceNow { get; } = experienceNow;
}
