using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Тип урона. </summary>
[Table(nameof(DbContextGame.DamageTypes), Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class DamageType
{
    /// <summary> Уникальный идентификатор. </summary>
    public int Id { get; init; }

    /// <summary> Уникальное наименование на английском. </summary>
    [MaxLength(256)]
    public required string Name { get; set; }

    /// <summary> Наименование на русском. </summary>
    [MaxLength(256)]
    public string? NameRu { get; set; }

    /// <summary> Подсказка для разработчика. </summary>
    [Column(TypeName = "text")]
    public string? DevHintRu { get; set; }


    /// <summary>
    /// Категория типа урона.
    /// 0 - без категории.
    /// 1 - физический.
    /// 2 - магический.
    /// </summary>
    [HasDefaultValue(0)]
    public int Category {get; set; }

    ///// <summary>// Типы оружия для этого типа урона. Вычисляемое свойство.
    ///// </summary>
    //[NotMapped]
    //public IReadOnlyCollection<EquipmentType> WeaponTypes => X_EquipmentType_DamageType?.Select(static x => x.EquipmentType).ToList() ?? [];
}
