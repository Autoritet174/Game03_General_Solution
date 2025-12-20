using General.DTO;
using System;

namespace General.DTO;

/// <summary>
/// Предмет экипировки из коллекции игрока. (MongoDb).
/// </summary>
public class DtoCollectionEquipment(string id, Guid userId, DtoBaseEquipment equipBase, string groupName, long health, long attack, int strength, int agility, int intelligence, int haste, int level)
{
    public string Id { get; } = id;
    public Guid UserId { get; } = userId;
    public DtoBaseEquipment EquipBase { get; } = equipBase;
    public string GroupName { get; } = groupName;
    public long Health { get; } = health;
    public long Attack { get; } = attack;
    public int Strength { get; } = strength;
    public int Agility { get; } = agility;
    public int Intelligence { get; } = intelligence;
    public int Haste { get; } = haste;
    public int Level { get; } = level;

}
