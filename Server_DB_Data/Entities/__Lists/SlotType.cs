using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Data.Entities.__Lists;

/// <summary>
/// Тип слота экипировки.
/// </summary>
[Table("SlotTypes", Schema = nameof(__Lists))]
[Index(nameof(Name), IsUnique = true)]
public class SlotType
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
    /// Наименование на русском.
    /// </summary>
    [MaxLength(255)]
    public string? NameRu { get; set; }


    /// <summary>
    /// Навигационное свойство к <see cref="__Lists.EquipmentType"/>.
    /// </summary>
    public ICollection<EquipmentType> EquipmentTypes { get; set; } = [];
}
