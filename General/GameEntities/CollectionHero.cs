using System;
using System.Collections.Generic;
using System.Text;

namespace General.GameEntities;

public class CollectionHero(string id, Guid owner_id, Guid hero_id, string groupName, long health, long attack, long strength, long agility, long intelligence, long haste)
{
    public string Id { get; private set; } = id;
    public Guid Owner_id { get; private set; } = owner_id;
    public Guid Hero_id { get; private set; } = hero_id;
    public string GroupName { get; private set; } = groupName;
    public long Health { get; private set; } = health;
    public long Attack { get; private set; } = attack;
    public long Strength { get; private set; } = strength;
    public long Agility { get; private set; } = agility;
    public long Intelligence { get; private set; } = intelligence;
    public long Haste { get; private set; } = haste;

}
