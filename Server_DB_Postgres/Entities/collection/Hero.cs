using Server_DB_Postgres.Attributes;
using Server_DB_Postgres.Entities.GameData;
using Server_DB_Postgres.Entities.Users;
using Server_DB_Postgres.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_DB_Postgres.Entities.Collection;

/// <summary> Герой в коллекции пользователя. </summary>
[Table(nameof(DbContextGame.Heroes), Schema = nameof(Collection))]
public class Hero : IVersion, ICreatedAt, IUpdatedAt
{
    public Guid Id { get; init; }

    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    /// <summary> <inheritdoc/> </summary>
    public long Version { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary> <inheritdoc/> </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary> Идентификатор базовой версии героя. </summary>
    public int BaseHeroId { get; set; }
    /// <summary> Сущность <see cref="GameData.BaseHero"/>. </summary>
    [ForeignKey(nameof(BaseHeroId))]
    public BaseHero BaseHero { get; set; } = null!;

    /// <summary> Имя группы. </summary>
    [MaxLength(256)]
    public string? GroupName { get; set; }

    /// <summary> Уровень героя. </summary>
    [Default(1)]
    public int Level { get; set; }

    /// <summary> Текущий опыт. </summary>
    [Default(0)]
    public float Experience { get; set; }



    #region Характеристики
    public float Health { get; set; }
    public float Strength { get; set; }
    public float Agility { get; set; }
    public float Intelligence { get; set; }
    public float CritChance { get; set; }
    public float CritMultiplier { get; set; }
    public float Haste { get; set; }
    public float Versality { get; set; }
    public float EndurancePhysical { get; set; }
    public float EnduranceMagical { get; set; }
    public float Initiative { get; set; }
    #endregion Характеристики


    #region Resistances

    ///// <summary> Сопротивление физическому урону. Выражается числом которое преобразовывается в проценты. </summary>
    //public long ResistDamagePhysical { get; set; }

    ///// <summary> Сопротивление магическому урону. Выражается числом которое преобразовывается в проценты. </summary>
    //public long ResistDamageMagical { get; set; }

    #endregion Resistances

}
