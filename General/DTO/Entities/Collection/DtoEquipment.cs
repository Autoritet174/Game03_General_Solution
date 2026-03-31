using General.DTO.Entities.GameData;
using System;
using System.Collections.Generic;

namespace General.DTO.Entities.Collection;

/// <summary>
/// Предмет экипировки из коллекции игрока. (MongoDb).
/// </summary>
public class DtoEquipment(Guid id, Guid userId, int baseEquipmentId, string? groupName, Guid? heroId, ESlot? slotId, Dictionary<EStatType, List<float>>? stats, int level)
{
    public Guid Id { get; } = id;
    public Guid UserId { get; } = userId;
    public int BaseEquipmentId { get; } = baseEquipmentId;
    public DtoBaseEquipment? BaseEquipment { get; set; } = null;
    public string? GroupName { get; set; } = groupName;
    public Guid? HeroId { get; set; } = heroId;
    public ESlot? SlotId { get; set; } = slotId;

    public int Level { get; set; } = level;

    public Dictionary<EStatType, List<float>>? Stats { get; set; } = stats;
}
