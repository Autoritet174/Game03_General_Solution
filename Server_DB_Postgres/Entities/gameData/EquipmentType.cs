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
    /// <summary> Уникальный идентификатор. </summary>
    public int Id { get; set; }

    /// <summary> Уникальное наименование на английском. </summary>
    [MaxLength(255)]
    public required string Name { get; set; }

    /// <summary> Уникальное наименование на русском. </summary>
    [MaxLength(255)]
    public required string NameRu { get; set; }

    /// <summary> Навигационное свойство к DamageTypes если экипировка наносит урон. </summary>
    public ICollection<X_EquipmentType_DamageType> X_EquipmentType_DamageType { get; set; } = [];

    ///// <summary>// Типы урона для этого типа экипировки. Вычисляемое свойство.
    ///// </summary>
    //[NotMapped]
    //public IReadOnlyCollection<DamageType> DamageTypes => X_EquipmentType_DamageType.Select(static x => x.DamageType).ToList() ?? [];

    /// <summary> Масса предмета в граммах как если бы предмет был из железа. </summary>
    [HasDefaultValue(0)]
    public int MassPhysical { get; set; }

    /// <summary> Магическая масса предмета. </summary>
    [HasDefaultValue(0)]
    public int MassMagical { get; set; }

    /// <summary> Идентификатор <see cref="GameData.SlotType"/>. </summary>
    public int SlotTypeId { get; set; }
    /// <summary> Тип слота экипировки. Не индекс слота а его тип, то есть Кольцо или Браслет, а не Кольцо2 или Браслет1. Сущность <see cref="GameData.SlotType"/>. </summary>
    [ForeignKey(nameof(SlotTypeId))]
    public SlotType? SlotType;

    /// <summary> Можно создать через "Кузнечное дело". </summary>
    [HasDefaultValue(false)]
    public bool CanCraftSmithing { get; set; } = false;

    /// <summary> Можно создать через "Ювелирное дело". </summary>
    [HasDefaultValue(false)]
    public bool CanCraftJewelcrafting { get; set; } = false;


    /// <summary> Трата очков действия за удар. </summary>
    [HasDefaultValue(0)]
    public int SpendActionPoints { get; set; } = 0;

    /// <summary> Блокирует ли оружие другую руку, если это оружие. </summary>
    public bool? BlockOtherHand { get; set; }
}
