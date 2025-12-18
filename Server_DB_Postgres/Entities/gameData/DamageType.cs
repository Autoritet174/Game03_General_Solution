using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Тип урона. </summary>
[Table("DamageTypes", Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class DamageType
{
    /// <summary> Уникальный идентификатор. </summary>
    public int Id { get; set; }

    /// <summary> Уникальное наименование на английском. </summary>
    [MaxLength(255)]
    public required string Name { get; set; }

    /// <summary> Наименование на русском. </summary>
    [MaxLength(255)]
    public string? NameRu { get; set; }

    /// <summary> Подсказка для разработчика. </summary>
    [Column(TypeName = "text")]
    public string? DevHintRu { get; set; }


    /// <summary> Навигационное свойство к <see cref="GameData.X_EquipmentType_DamageType"/>. </summary>
    public ICollection<X_EquipmentType_DamageType> X_EquipmentType_DamageType { get; set; } = [];

    ///// <summary>// Типы оружия для этого типа урона. Вычисляемое свойство.
    ///// </summary>
    //[NotMapped]
    //public IReadOnlyCollection<EquipmentType> WeaponTypes => X_EquipmentType_DamageType?.Select(static x => x.EquipmentType).ToList() ?? [];
}
