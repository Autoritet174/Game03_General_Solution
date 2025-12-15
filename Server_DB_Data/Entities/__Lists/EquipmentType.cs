using Microsoft.EntityFrameworkCore;
using Server_DB_Data.Entities.X_Cross;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Data.Entities.__Lists;

/// <summary>
/// Тип экипировки.
/// </summary>
[Table("EquipmentTypes", Schema = nameof(__Lists))]
[Index(nameof(Name), IsUnique = true)]
public class EquipmentType
{
    /// <summary>
    /// Первичный ключ.
    /// </summary>
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Уникальное наименование на английском.
    /// </summary>
    [Required, MaxLength(255)]
    public required string Name { get; set; }

    /// <summary>
    /// Уникальное наименование на русском.
    /// </summary>
    [Required, MaxLength(255)]
    public required string NameRu { get; set; }


    /// <summary>
    /// Навигационное свойство к DamageTypes если экипировка наносит урон.
    /// </summary>
    public ICollection<X_EquipmentType_DamageType> X_EquipmentType_DamageType { get; set; } = [];

    /// <summary>
    /// Типы урона для этого типа экипировки. Вычисляемое свойство.
    /// </summary>
    [NotMapped]
    public IReadOnlyCollection<DamageType> DamageTypes => X_EquipmentType_DamageType.Select(static x => x.DamageType).ToList() ?? [];

    /// <summary>
    /// Масса предмета в граммах как если бы предмет был из железа.
    /// </summary>
    [Required]
    public required int Mass = 0;


    /// <summary>
    /// Идентификатор <see cref="__Lists.SlotType"/>.
    /// </summary>
    [Required]
    public required int SlotTypeId { get; set; }

    /// <summary>
    /// Тип слота экипировки.
    /// Сущность <see cref="__Lists.SlotType"/>.
    /// </summary>
    [Required, ForeignKey(nameof(SlotTypeId))]
    public required SlotType SlotType;

    /// <summary>
    /// Можно создать через Кузнечное дело.
    /// </summary>
    [Required]
    public bool CanCraftSmithing { get; set; }

    /// <summary>
    /// Можно создать через Ювелирное дело.
    /// </summary>
    [Required]
    public bool CanCraftJewelcrafting { get; set; }

    /// <summary>
    /// Атака оружия
    /// </summary>
    [MaxLength(255)]
    public string? Attack { get; set; }

    /// <summary>
    /// Трата очков действия за удар.
    /// </summary>
    [Required]
    public int SpendActionPoints { get; set; }
}
