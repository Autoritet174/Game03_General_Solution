using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres.Entities.gameData;

/// <summary> Экипировка. Базовые болванки. </summary>
[Table("BaseEquipments", Schema = nameof(gameData))]
[Index(nameof(Name), IsUnique = true)]
public class BaseEquipment
{
    /// <summary> Уникальный идентификатор. </summary>
    public int Id { get; set; }

    /// <summary> Уникальное наименование на английском. </summary>
    [MaxLength(255)]
    public required string Name { get; set; }

    /// <summary> Редкость. </summary>
    public int Rarity { get; set; }

    /// <summary> Уникальный для одного аккаунта. </summary>
    [HasDefaultValue(false)]
    public bool IsUnique { get; set; }


    /// <summary> Урон. Формат DND кубиков, 2d2. </summary>
    [MaxLength(255)]
    public string? Damage { get; set; }

    /// <summary> Идентификатор EquipmentType. </summary>
    public int EquipmentTypeId { get; set; }

    /// <summary> Сущность EquipmentType. </summary>
    [ForeignKey(nameof(EquipmentTypeId))]
    public required EquipmentType EquipmentTypes { get; set; }
}
