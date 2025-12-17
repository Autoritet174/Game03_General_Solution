using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres.Entities.gameData;

/// <summary>
/// Тип экипировки.
/// </summary>
[Table("EquipmentTypes", Schema = nameof(gameData))]
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
    [MaxLength(255)]
    public required string Name { get; set; }

    /// <summary>
    /// Уникальное наименование на русском.
    /// </summary>
    [MaxLength(255)]
    public required string NameRu { get; set; }


    /// <summary>
    /// Навигационное свойство к DamageTypes если экипировка наносит урон.
    /// </summary>
    public ICollection<x_EquipmentType_DamageType> X_EquipmentType_DamageType { get; set; } = [];

    /// <summary>
    /// Типы урона для этого типа экипировки. Вычисляемое свойство.
    /// </summary>
    [NotMapped]
    public IReadOnlyCollection<DamageType> DamageTypes => X_EquipmentType_DamageType.Select(static x => x.DamageType).ToList() ?? [];

    /// <summary>
    /// Масса предмета в граммах как если бы предмет был из железа.
    /// </summary>
    [HasDefaultValue(0)]
    public int Mass { get; set; }


    /// <summary>
    /// Идентификатор <see cref="__Lists.SlotType"/>.
    /// </summary>
    public int SlotTypeId { get; set; }

    /// <summary>
    /// Тип слота экипировки.
    /// Сущность <see cref="__Lists.SlotType"/>.
    /// </summary>
    [ForeignKey(nameof(SlotTypeId))]
    public required SlotType SlotType;

    /// <summary>
    /// Можно создать через Кузнечное дело.
    /// </summary>
    [HasDefaultValue(false)]
    public bool CanCraftSmithing { get; set; }

    /// <summary>
    /// Можно создать через Ювелирное дело.
    /// </summary>
    [HasDefaultValue(false)]
    public bool CanCraftJewelcrafting { get; set; }

    /// <summary>
    /// Атака оружия
    /// </summary>
    [MaxLength(255)]
    public string? Attack { get; set; }

    /// <summary>
    /// Трата очков действия за удар.
    /// </summary>
    [HasDefaultValue(0)]
    public int SpendActionPoints { get; set; }
}
