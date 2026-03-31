using General;
using General.DTO;
using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Экипировка. Базовые болванки. Мечи, топоры, шлемы, щиты, кольцо, браслеты, амулеты и так далее. </summary>
[Table(nameof(DbContextGame.BaseEquipments), Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class BaseEquipment
{
    /// <summary> Уникальный идентификатор. </summary>
    public int Id { get; init; }

    /// <summary> Уникальное наименование на английском. </summary>
    [MaxLength(256)]
    public required string Name { get; set; }

    /// <summary> Редкость. </summary>
    public int Rarity { get; set; }

    /// <summary> Уникальный для одного аккаунта. </summary>
    [Default(false)]
    public bool IsUnique { get; set; }

    /// <summary> Идентификатор EquipmentType. </summary>
    public int EquipmentTypeId { get; set; }
    /// <summary> Сущность EquipmentType. </summary>
    [ForeignKey(nameof(EquipmentTypeId))]
    public EquipmentType EquipmentType { get; set; } = null!;

    public int? SmithingMaterialId { get; set; }
    [ForeignKey(nameof(SmithingMaterialId))]
    public SmithingMaterial? SmithingMaterial { get; set; }

    /// <summary>
    /// Характеристики возможные при дропе экипировки.
    /// </summary>
    [Jsonb] public Dictionary<EStatType, Dice>? PossibleStats { get; set; }
}
