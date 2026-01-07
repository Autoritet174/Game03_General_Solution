using General.DTO.Entities.GameData;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace General.DTO.Entities.Collection;

/// <summary>
/// Предмет экипировки из коллекции игрока. (MongoDb).
/// </summary>
public class DtoEquipment(Guid id, Guid userId, int baseEquipmentId, string? groupName, int? slotTypeId, Guid? heroId)
{
    public Guid Id { get; } = id;
    public Guid UserId { get; } = userId;
    public int BaseEquipmentId { get; } = baseEquipmentId;
    public DtoBaseEquipment? BaseEquipment { get; set; } = null;
    public string? GroupName { get; set; } = groupName;
    //public long Health1000 { get; set; }
    //public Dice Damage { get; set; }
    //public int Strength { get; set; }
    //public int Agility { get; set; }
    //public int Intelligence { get; set; }
    //public int Haste { get; set; }
    //public int Level { get; set; }

    /// <summary>
    /// Слот в который эпипирован предмет.
    /// </summary>
    public int? SlotId { get; set; } = slotTypeId;
    public DtoSlot? Slot { get; set; }

    /// <summary>
    /// Герой на которого экипирован предмет.
    /// </summary>
    public Guid? HeroId { get; set; } = heroId;
    public DtoHero? Hero { get; set; }
}
