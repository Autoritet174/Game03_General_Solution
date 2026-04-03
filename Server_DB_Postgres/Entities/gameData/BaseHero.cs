using General;
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
    public ERarity Rarity { get; set; }

    /// <summary> Уникальный для одного аккаунта. </summary>
    [Default(false)]
    public bool IsUnique { get; set; }

    /// <summary> Основной стат который повышает урон. Сила(1) или Ловкость(2) или Интеллект(3). </summary>
    public EMainStat MainStat { get; set; }


    ///// <summary> Типы существ героя. Вычисляемое свойство. </summary>
    //[NotMapped]
    //public IReadOnlyCollection<CreatureType> CreatureTypes => X_Hero_CreatureType?.Select(static x => x.CreatureTypes).ToList() ?? [];

    #region Характеристики
    [Jsonb] public Dice Health { get; set; } = null!;
    [Jsonb] public Dice Damage { get; set; } = null!;
    [Jsonb] public Dice Strength { get; set; } = null!;
    [Jsonb] public Dice Agility { get; set; } = null!;
    [Jsonb] public Dice Intelligence { get; set; } = null!;
    [Jsonb] public Dice CritChance { get; set; } = null!;
    [Jsonb] public Dice CritMultiplier { get; set; } = null!;
    [Jsonb] public Dice Haste { get; set; } = null!;
    [Jsonb] public Dice Versality { get; set; } = null!;
    [Jsonb] public Dice EndurancePhysical { get; set; } = null!;
    [Jsonb] public Dice EnduranceMagical { get; set; } = null!;
    [Jsonb] public Dice Initiative { get; set; } = null!;
    #endregion Характеристики

}
