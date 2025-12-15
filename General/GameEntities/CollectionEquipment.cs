using System;

namespace General.GameEntities;

/// <summary>
/// Предмет экипировки из коллекции игрока. (MongoDb).
/// </summary>
/// <param name="id"></param>
/// <param name="owner_id"></param>
/// <param name="equipBase"></param>
/// <param name="groupName"></param>
/// <param name="health"></param>
/// <param name="attack"></param>
/// <param name="strength"></param>
/// <param name="agility"></param>
/// <param name="intelligence"></param>
/// <param name="haste"></param>
/// <param name="level"></param>
public class CollectionEquipment(string id, Guid owner_id, EquipBase equipBase, string groupName,
    long health, long attack,
    long strength, long agility, long intelligence, long haste, int level)
{
    public string Id { get; } = id;
    public Guid OwnerId { get; } = owner_id;
    public EquipBase EquipBase { get; } = equipBase;
    public string GroupName { get; } = groupName;
    public long Health { get; } = health;
    public long Attack { get; } = attack;
    public long Strength { get; } = strength;
    public long Agility { get; } = agility;
    public long Intelligence { get; } = intelligence;
    public long Haste { get; } = haste;
    public int Level { get; } = level;

}
