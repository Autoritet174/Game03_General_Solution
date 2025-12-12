using General;
using Microsoft.EntityFrameworkCore;
using Server_DB_Data.Entities.__Lists;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Data.Entities._Equipment;

/// <summary>
/// Экипировка. Оружие.
/// </summary>
[Table("Weapons", Schema = nameof(_Equipment))]
[Index(nameof(Name), IsUnique = true)]
public class Weapon : IEntity
{
    #region Entity

    /// <summary>
    /// Первичный ключ. Автоинкремент.
    /// </summary>
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Уникальное наименование на английском.
    /// </summary>
    [Required, MaxLength(255)]
    public required string Name { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Required]
    public Enums.RarityLevel Rarity { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Required]
    public bool IsUnique { get; set; } = false;

    #endregion Entity

    //------------------------------------------------------------

    /// <summary>
    /// Урон. Формат DND кубиков, 2d2.
    /// </summary>
    [Required, MaxLength(255)]
    public required string Damage { get; set; }

    /// <summary>
    /// Идентификатор WeaponType.
    /// </summary>
    [Required]
    public required int WeaponTypeId { get; set; }

    /// <summary>
    /// Сущность WeaponType.
    /// </summary>
    [Required, ForeignKey(nameof(WeaponTypeId))]
    public required EquipmentType WeaponTypes { get; set; }
}
