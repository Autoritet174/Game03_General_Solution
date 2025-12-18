using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres.Entities.GameData;

/// <summary> Базовая версия героя. </summary>
[Table("BaseHeroes", Schema = nameof(GameData))]
[Index(nameof(Name), IsUnique = true)]
public class BaseHero
{

    /// <summary> Уникальный идентификатор. </summary>
    public int Id { get; set; }

    /// <summary> Наименование на английском языке. </summary>
    [MaxLength(255)]
    public required string Name { get; set; }

    /// <summary> Уровень редкости. </summary>
    public int Rarity { get; set; } = 1;

    /// <summary> Уникальный для одного аккаунта. </summary>
    [HasDefaultValue(false)]
    public bool IsUnique { get; set; } = false;


    /// <summary> Здоровье. Формат DND кубиков, 2d2. </summary>
    [MaxLength(255)]
    public required string Health { get; set; }

    /// <summary> Урон. Формат DND кубиков, 2d2. </summary>
    [MaxLength(255)]
    public required string Damage { get; set; }

    /// <summary> Основной стат который повышает урон. Сила(1) или Ловкость(2) или Интеллект(3). </summary>
    [HasDefaultValue(0)]
    public int MainStat { get; set; } = 0;

    /// <summary> Навигационное свойство к CreatureTypes. </summary>
    public ICollection<X_Hero_CreatureType> X_Hero_CreatureType { get; set; } = [];

    ///// <summary>// Типы существ героя. Вычисляемое свойство. </summary>
    //[NotMapped]
    //public IReadOnlyCollection<CreatureType> CreatureTypes => X_Hero_CreatureType?.Select(static x => x.CreatureTypes).ToList() ?? [];
}
