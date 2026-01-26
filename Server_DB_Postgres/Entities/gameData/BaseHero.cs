using General.DTO;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Базовая версия героя. </summary>
[Table(nameof(DbContextGame.BaseHeroes), Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class BaseHero
{

    /// <summary> Уникальный идентификатор. </summary>
    public int Id { get; init; }

    /// <summary> Наименование на английском языке. </summary>
    [MaxLength(256)]
    public required string Name { get; set; }

    /// <summary> Уровень редкости. </summary>
    public int Rarity { get; set; }

    /// <summary> Уникальный для одного аккаунта. </summary>
    [HasDefaultValue(false)]
    public bool IsUnique { get; set; }

    /// <summary> Основной стат который повышает урон. Сила(1) или Ловкость(2) или Интеллект(3). </summary>
    [HasDefaultValue(0)]
    public int MainStat { get; set; }


    ///// <summary> Типы существ героя. Вычисляемое свойство. </summary>
    //[NotMapped]
    //public IReadOnlyCollection<CreatureType> CreatureTypes => X_Hero_CreatureType?.Select(static x => x.CreatureTypes).ToList() ?? [];

    #region Характеристики
    [Column(TypeName = "jsonb")]
    public Dice? Health { get; set; }

    [Column(TypeName = "jsonb")]
    public Dice? Damage { get; set; }
    #endregion Характеристики

}
