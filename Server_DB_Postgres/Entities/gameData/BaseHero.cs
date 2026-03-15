using General.DTO;
using Microsoft.EntityFrameworkCore;
using Server_DB_Postgres.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Базовая версия героя. </summary>
[Table(nameof(DbContextGame.BaseHeroes), Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class BaseHero
{
    public int Id { get; init; }

    /// <summary> Наименование на английском языке. </summary>
    [MaxLength(256)]
    public required string Name { get; set; }

    /// <summary> Уровень редкости. </summary>
    public int Rarity { get; set; }

    /// <summary> Уникальный для одного аккаунта. </summary>
    [Default(false)]
    public bool IsUnique { get; set; }

    /// <summary> Основной стат который повышает урон. Сила(1) или Ловкость(2) или Интеллект(3). </summary>
    [Default(0)]
    public int MainStat { get; set; }


    ///// <summary> Типы существ героя. Вычисляемое свойство. </summary>
    //[NotMapped]
    //public IReadOnlyCollection<CreatureType> CreatureTypes => X_Hero_CreatureType?.Select(static x => x.CreatureTypes).ToList() ?? [];

    #region Характеристики
    [Jsonb] public Dice Health_1000 { get; set; } = null!;
    [Jsonb] public Dice Damage_1000 { get; set; } = null!;
    [Jsonb] public Dice Strength_1000 { get; set; } = null!;
    [Jsonb] public Dice Agility_1000 { get; set; } = null!;
    [Jsonb] public Dice Intelligence_1000 { get; set; } = null!;
    [Jsonb] public Dice CritChance_1000 { get; set; } = null!;
    [Jsonb] public Dice CritMultiplier_1000 { get; set; } = null!;
    [Jsonb] public Dice Haste_1000 { get; set; } = null!;
    [Jsonb] public Dice Versality_1000 { get; set; } = null!;
    [Jsonb] public Dice EndurancePhysical_1000 { get; set; } = null!;
    [Jsonb] public Dice EnduranceMagical_1000 { get; set; } = null!;
    [Jsonb] public Dice Initiative_1000 { get; set; } = null!;
    #endregion Характеристики

}
