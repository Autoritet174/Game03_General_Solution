using General.DTO;
using Server_DB_Postgres.Entities.GameData;
using Server_DB_Postgres.Entities.Users;
using Server_DB_Postgres.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Server_DB_Postgres.Attributes;

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

    public bool IsBaseHeroUnique { get; set; }

    /// <summary> Имя группы. </summary>
    [MaxLength(256)]
    public string? GroupName { get; set; }

    /// <summary> Уровень героя. </summary>
    [HasDefaultValue(1)]
    public int Level { get; set; }

    /// <summary> Текущий опыт. </summary>
    [HasDefaultValue(0)]
    public long ExperienceNow { get; set; }



    #region Характеристики
    // --------------Характеристики числовые----------------

    /// <summary> Здоровье. Первые три знака, тысячные доли от единицы видимой игроку. </summary>
    public long Health1000 { get; set; }

    /// <summary> Сила. </summary>
    public long Strength1000 { get; set; }

    /// <summary> Ловкость. </summary>
    public long Agility1000 { get; set; }

    /// <summary> Интеллект. </summary>
    public long Intelligence1000 { get; set; }

    /// <summary> Шанс критического урона. </summary>
    public long CritChance1000 { get; set; }

    /// <summary> Сила критического урона. </summary>
    public long CritPower1000 { get; set; }

    /// <summary> Скорость. </summary>
    public long Haste1000 { get; set; }

    /// <summary> Универсальность. </summary>
    public long Versality1000 { get; set; }

    /// <summary> Физическая выносливось. </summary>
    public long EndurancePhysical1000 { get; set; }

    /// <summary> Магическая выносливось. </summary>
    public long EnduranceMagical1000 { get; set; }

    [Column(TypeName = "jsonb")]
    public long Initiative1000 { get; set; }

    // --------------Характеристики кубика ДНД----------------
    #endregion Характеристики


    #region Resistances

    ///// <summary> Сопротивление физическому урону. Выражается числом которое преобразовывается в проценты. </summary>
    //public long ResistDamagePhysical1000 { get; set; }

    ///// <summary> Сопротивление магическому урону. Выражается числом которое преобразовывается в проценты. </summary>
    //public long ResistDamageMagical1000 { get; set; }

    #endregion Resistances

}
