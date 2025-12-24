using General.DTO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Экипировка. Базовые болванки. Мечи, топоры, шлемы, щиты, кольцо, браслеты, амулеты и так далее. </summary>
[Table("BaseEquipments", Schema = nameof(GameData))]
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

    /// <summary> Идентификатор EquipmentType. </summary>
    public int EquipmentTypeId { get; set; }
    /// <summary> Сущность EquipmentType. </summary>
    [ForeignKey(nameof(EquipmentTypeId))]
    public EquipmentType? EquipmentTypes { get; set; }

    public int? SmithingMaterialId { get; set; }
    [ForeignKey(nameof(SmithingMaterialId))]
    public SmithingMaterial? SmithingMaterial { get; set; }

    #region Характеристики
    [Column(TypeName = "jsonb")]
    public Dice? Health { get; set; }

    [Column(TypeName = "jsonb")]
    public Dice? Damage { get; set; }
    #endregion Характеристики
}
