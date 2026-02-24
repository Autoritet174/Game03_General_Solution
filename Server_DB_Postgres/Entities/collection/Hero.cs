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

    /// <summary> Имя группы. </summary>
    [MaxLength(256)]
    public string? GroupName { get; set; }

    /// <summary> Редкость. </summary>
    [HasDefaultValue(1)]
    public int Level { get; set; }

    /// <summary> Текущий опыт. </summary>
    [HasDefaultValue(0)]
    public long ExperienceNow { get; set; }



    #region Характеристики
    // --------------Характеристики числовые----------------

    /// <summary> Сила. </summary>
    public int Strength { get; set; }

    /// <summary> Ловкость. </summary>
    public int Agility { get; set; }

    /// <summary> Интеллект. </summary>
    public int Intelligence { get; set; }

    /// <summary> Шанс критического урона. </summary>
    public int CritChance { get; set; }

    /// <summary> Сила критического урона. </summary>
    public int CritPower { get; set; }

    /// <summary> Скорость. </summary>
    public int Haste { get; set; }

    /// <summary> Универсальность. </summary>
    public int Versality { get; set; }

    /// <summary> Физическая выносливось. </summary>
    public int EndurancePhysical { get; set; }

    /// <summary> Магическая выносливось. </summary>
    public int EnduranceMagical { get; set; }

    /// <summary> Здоровье. Первые три знака, тысячные доли от единицы видимой игроку. </summary>
    public long Health1000 { get; set; }

    // --------------Характеристики кубика ДНД----------------

    [Column(TypeName = "jsonb")]
    public Dice? Damage { get; set; }
    #endregion Характеристики


    #region Resistances

    /// <summary> Сопротивление физическому урону. Выражается числом которое преобразовывается в проценты. </summary>
    public int ResistDamagePhysical { get; set; }

    /// <summary> Сопротивление магическому урону. Выражается числом которое преобразовывается в проценты. </summary>
    public int ResistDamageMagical { get; set; }

    #endregion Resistances

}
