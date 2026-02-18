using General.DTO.Entities.GameData;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace General.DTO.Entities.Collection;

/// <summary>
/// Предмет экипировки из коллекции игрока. (MongoDb).
/// </summary>
public class DtoEquipment(Guid id, Guid userId, int baseEquipmentId, string? groupName, Guid? heroId, int? slotId)
{
    public Guid Id { get; } = id;
    public Guid UserId { get; } = userId;
    public int BaseEquipmentId { get; } = baseEquipmentId;
    public DtoBaseEquipment? BaseEquipment { get; set; } = null;
    public string? GroupName { get; set; } = groupName;
    public Guid? HeroId { get; set; } = heroId;
    public int? SlotId { get; set; } = slotId;
}
