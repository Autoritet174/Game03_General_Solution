using General.DTO.Entities.GameData;
using System;

namespace General.DTO.Entities.Collection;

/// <summary>
/// Предмет экипировки из коллекции игрока. (MongoDb).
/// </summary>
public class DtoCollectionEquipment()
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public int BaseEquipmentId { get; set; }
    public DtoBaseEquipment? DtoBaseEquipment { get; set; }
    public string? GroupName { get; set; }
    //public long Health1000 { get; set; }
    //public Dice Damage { get; set; }
    //public int Strength { get; set; }
    //public int Agility { get; set; }
    //public int Intelligence { get; set; }
    //public int Haste { get; set; }
    //public int Level { get; set; }

}
