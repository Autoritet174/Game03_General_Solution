using General.DTO.Entities.GameData;
using General.DTO.Interfaces;
using System.Collections.Generic;

namespace General.DTO.Entities.Collection;

public class Equipment : ICreatedAt, IUpdatedAt, IVersion
{
    public Guid id { get; init; }
    public required Guid userId { get; init; }
    public string? groupName { get; set; }
    public long version { get; set; }
    public DateTimeOffset createdAt { get; set; }
    public DateTimeOffset updatedAt { get; set; }
    public required int baseEquipmentId { get; init; }
    public BaseEquipment baseEquipment { get; set; } = null!;
    public Guid? heroId { get; set; }
    //public Hero Hero { get; set; } = null!;
    public ESlot? slotId { get; set; }
    public int level { get; set; } = 1;
    public Dictionary<EStatType, List<float>>? stats { get; set; }

    public Equipment CreateCopy()
    {
        return (Equipment)MemberwiseClone();
    }
}
