using General;
using Microsoft.EntityFrameworkCore;
using Server_DB_Data.Entities.X_Cross;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Data.Entities._Heroes;

/// <summary>
/// Базовая версия героя.
/// </summary>
[Table("heroes", Schema = "_heroes")]
[Index(nameof(Name), IsUnique = true)]
public class Hero : IEntity
{
    #region Entity

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// <inheritdoc/>
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
    /// Здоровье. Формат DND кубиков, 2d2.
    /// </summary>
    [Required, MaxLength(255)]
    public required string Health { get; set; }

    /// <summary>
    /// Урон. Формат DND кубиков, 2d2.
    /// </summary>
    [Required, MaxLength(255)]
    public required string Damage { get; set; }

    /// <summary>
    /// Список из типов существ к котором принадлежит герой.
    /// </summary>
    [Required]
    public ICollection<X_HeroCreatureType> CreatureTypes { get; set; } = [];

}
