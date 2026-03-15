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
    /// <summary> Уникальный идентификатор. </summary>
    public Guid Id { get; init; }

    /// <summary> Уникальный идентификатор владельца. </summary>
    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

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
    public BaseHero? BaseHero { get; set; }

    //public bool IsBaseHeroUnique { get; set; }

    /// <summary> Имя группы. </summary>
    [MaxLength(256)]
    public string? GroupName { get; set; }

    /// <summary> Уровень героя. </summary>
    [Default(1)]
    public int Level { get; set; }

    /// <summary> Текущий опыт. </summary>
    [Default(0)]
    public long ExperienceNow { get; set; }



    #region Характеристики
    public long Health_1000 { get; set; }
    public long Strength_1000 { get; set; }
    public long Agility_1000 { get; set; }
    public long Intelligence_1000 { get; set; }
    public long CritChance_1000 { get; set; }
    public long CritMultiplier_1000 { get; set; }
    public long Haste_1000 { get; set; }
    public long Versality_1000 { get; set; }
    public long EndurancePhysical_1000 { get; set; }
    public long EnduranceMagical_1000 { get; set; }
    public long Initiative_1000 { get; set; }
    #endregion Характеристики


    #region Resistances

    ///// <summary> Сопротивление физическому урону. Выражается числом которое преобразовывается в проценты. </summary>
    //public long ResistDamagePhysical1000 { get; set; }

    ///// <summary> Сопротивление магическому урону. Выражается числом которое преобразовывается в проценты. </summary>
    //public long ResistDamageMagical1000 { get; set; }

    #endregion Resistances

}
