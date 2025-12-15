using General;
using Microsoft.EntityFrameworkCore;
using Server_DB_Data.Entities.__Lists;
using Server_DB_Data.Entities.X_Cross;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Data.Entities._Heroes;

/// <summary>
/// Базовая версия героя.
/// </summary>
[Table("Heroes", Schema = nameof(_Heroes))]
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
    /// Основной стат который повышает урон. Сила(1) или Ловкость(2) или Интеллект(3).
    /// </summary>
    [Required]
    public required int MainStat { get; set; }

    /// <summary>
    /// Навигационное свойство к CreatureTypes.
    /// </summary>
    public ICollection<X_Hero_CreatureType> X_Hero_CreatureType { get; set; } = [];

    /// <summary>
    /// Типы существ героя. Вычисляемое свойство.
    /// </summary>
    [NotMapped]
    public IReadOnlyCollection<CreatureType> CreatureTypes => X_Hero_CreatureType?.Select(static x => x.CreatureTypes).ToList() ?? [];
}
