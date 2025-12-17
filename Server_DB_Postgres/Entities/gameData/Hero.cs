using General;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres.Entities.gameData;

/// <summary>
/// Базовая версия героя.
/// </summary>
[Table("Heroes", Schema = nameof(gameData))]
[Index(nameof(Name), IsUnique = true)]
public class Hero 
{

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [MaxLength(255)]
    public required string Name { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public Enums.RarityLevel Rarity { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [HasDefaultValue(false)]
    public bool IsUnique { get; set; }


    //------------------------------------------------------------

    /// <summary>
    /// Здоровье. Формат DND кубиков, 2d2.
    /// </summary>
    [MaxLength(255)]
    public required string Health { get; set; }

    /// <summary>
    /// Урон. Формат DND кубиков, 2d2.
    /// </summary>
    [MaxLength(255)]
    public required string Damage { get; set; }

    /// <summary>
    /// Основной стат который повышает урон. Сила(1) или Ловкость(2) или Интеллект(3).
    /// </summary>
    [HasDefaultValue(0)]
    public required int MainStat { get; set; }

    /// <summary>
    /// Навигационное свойство к CreatureTypes.
    /// </summary>
    public ICollection<x_Hero_CreatureType> X_Hero_CreatureType { get; set; } = [];

    /// <summary>
    /// Типы существ героя. Вычисляемое свойство.
    /// </summary>
    [NotMapped]
    public IReadOnlyCollection<CreatureType> CreatureTypes => X_Hero_CreatureType?.Select(static x => x.CreatureTypes).ToList() ?? [];
}
