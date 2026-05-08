using General.DTO.Entities.GameData;
using General.DTO.Interfaces;
using System;
using System.Collections.Generic;
using static General.LocalizationKeys.Error;

namespace General.DTO.Entities.Collection;

public class Equipment : IVersion, ICreatedAt, IUpdatedAt
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? GroupName { get; set; }
    public long Version { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public int BaseEquipmentId { get; set; }
    public BaseEquipment BaseEquipment { get; set; } = null!;
    public Guid? HeroId { get; set; }
    public Hero Hero { get; set; } = null!;
    public ESlot? SlotId { get; set; }
    public Slot Slot { get; set; } = null!;
    public int Level { get; set; } = 1;
    public Dictionary<EStatType, List<float>>? Stats { get; set; }

    public Equipment CreateCopy()
    {
        return (Equipment)MemberwiseClone();
    }
}
