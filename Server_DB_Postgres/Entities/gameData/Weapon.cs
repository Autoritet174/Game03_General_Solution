using General;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres.Entities.gameData;

/// <summary>
/// Экипировка. Оружие.
/// </summary>
[Table("Weapons", Schema = nameof(gameData))]
[Index(nameof(Name), IsUnique = true)]
public class Weapon
{

    /// <summary>
    /// Первичный ключ. Автоинкремент.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Уникальное наименование на английском.
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
    /// Урон. Формат DND кубиков, 2d2.
    /// </summary>
    [MaxLength(255)]
    public required string Damage { get; set; }

    /// <summary>
    /// Идентификатор WeaponType.
    /// </summary>
    public int WeaponTypeId { get; set; }

    /// <summary>
    /// Сущность WeaponType.
    /// </summary>
    [ForeignKey(nameof(WeaponTypeId))]
    public required EquipmentType WeaponTypes { get; set; }
}
