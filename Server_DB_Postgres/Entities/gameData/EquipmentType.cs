using General;
using General.DTO;
using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Тип экипировки. </summary>
[Table(nameof(DbContextGame.EquipmentTypes), Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class EquipmentType
{
    public int Id { get; init; }

    [MaxLength(256)]
    public required string Name { get; set; }

    [MaxLength(256)]
    public required string NameRu { get; set; }


    //[NotMapped]
    //public IReadOnlyCollection<DamageType> DamageTypes => X_EquipmentType_DamageType.Select(static x => x.DamageType).ToList() ?? [];

    [Default(0)] public int MassPhysical { get; set; }

    [Default(0)] public int MassMagical { get; set; }

    public General.ESlotType SlotTypeId { get; set; }
    /// <summary> Тип слота экипировки. Не индекс слота а его тип, то есть Кольцо или Браслет, а не Кольцо2 или Браслет1. </summary>
    [ForeignKey(nameof(SlotTypeId))]
    public SlotType SlotType { get; set; } = null!;

    [Default(false)] public bool CanCraftSmithing { get; set; }

    [Default(false)] public bool CanCraftJewelcrafting { get; set; }

    /// <summary> Трата очков действия за удар. </summary>
    [Default(0)] public int SpendActionPoints { get; set; }

    /// <summary> Блокирует ли оружие другую руку, если это оружие. </summary>
    public bool? BlockOtherHand { get; set; }

    [Jsonb] public Dice? Damage { get; set; } // У оружия есть урон, а у брони нет и у других видов экипировки

    /// <summary>
    /// Характеристики возможные при дропе экипировки.
    /// </summary>
    [Jsonb] public Dictionary<EStatType, Dice>? PossibleStats { get; set; }
}
