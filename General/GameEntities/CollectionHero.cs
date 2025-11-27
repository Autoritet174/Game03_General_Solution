using System;

namespace General.GameEntities;

public class CollectionHero(string id, Guid owner_id, HeroBase heroBase, string groupName,
    long health, long attack,
    long strength, long agility, long intelligence, long haste, int level)
{
    public string Id { get; } = id;
    public Guid Owner_id { get; } = owner_id;
    public HeroBase HeroBase { get; } = heroBase;
    public string GroupName { get; } = groupName;
    public long Health { get; } = health;
    public long Attack { get; } = attack;
    public long Strength { get; } = strength;
    public long Agility { get; } = agility;
    public long Intelligence { get; } = intelligence;
    public long Haste { get; } = haste;
    public int Level { get; } = level;

}
