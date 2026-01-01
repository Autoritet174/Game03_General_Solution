using General.DTO;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Тип экипировки. </summary>
[Table("EquipmentTypes", Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class EquipmentType
{
    public int Id { get; set; }

    [MaxLength(256)]
    public required string Name { get; set; }

    [MaxLength(256)]
    public required string NameRu { get; set; }

    public ICollection<X_EquipmentType_DamageType> X_EquipmentType_DamageType { get; set; } = [];

    //[NotMapped]
    //public IReadOnlyCollection<DamageType> DamageTypes => X_EquipmentType_DamageType.Select(static x => x.DamageType).ToList() ?? [];

    [HasDefaultValue(0)]
    public int MassPhysical { get; set; }

    [HasDefaultValue(0)]
    public int MassMagical { get; set; }

    public int SlotTypeId { get; set; }
    /// <summary> Тип слота экипировки. Не индекс слота а его тип, то есть Кольцо или Браслет, а не Кольцо2 или Браслет1. </summary>
    [ForeignKey(nameof(SlotTypeId))]
    public SlotType? SlotType;

    [HasDefaultValue(false)]
    public bool CanCraftSmithing { get; set; } = false;

    [HasDefaultValue(false)]
    public bool CanCraftJewelcrafting { get; set; } = false;

    /// <summary> Трата очков действия за удар. </summary>
    [HasDefaultValue(0)]
    public int SpendActionPoints { get; set; } = 0;

    /// <summary> Блокирует ли оружие другую руку, если это оружие. </summary>
    public bool? BlockOtherHand { get; set; }

    [Column(TypeName = "jsonb")]
    public Dice? Damage { get; set; }
}
