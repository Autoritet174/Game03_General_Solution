using General.DTO;
using Server_DB_Postgres.Entities.GameData;
using Server_DB_Postgres.Entities.Users;
using Server_DB_Postgres.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Server_DB_Postgres.Attributes;

namespace Server_DB_Postgres.Entities.Collection;

/// <summary> Герой в коллекции пользователя. </summary>
[Table("Heroes", Schema = nameof(Collection))]
public class Hero : IVersion, ICreatedAt, IUpdatedAt
{
    /// <summary> Уникальный идентификатор. </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary> Уникальный идентификатор владельца. </summary>
    public Guid UserId { get; set; }
    /// <summary> Сущность <see cref="Users.User"/>. </summary>
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
    public int Level { get; set; } = 1;

    /// <summary> Текущий опыт. </summary>
    [HasDefaultValue(0)]
    public long ExperienceNow { get; set; } = 0;

    /// <summary> Редкость. </summary>
    [HasDefaultValue(1)]
    public int Rarity { get; set; } = 1;



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

    #region Equipment
    /// <summary> Экипировка, слот 1. </summary>
    public Guid? Equipment1Id { get; set; }
    /// <summary> Экипировка, слот 1. </summary>
    [ForeignKey(nameof(Equipment1Id))]
    public Equipment? Equipment1 { get; set; }

    /// <summary> Экипировка, слот 2. </summary>
    public Guid? Equipment2Id { get; set; }
    /// <summary> Экипировка, слот 2. </summary>
    [ForeignKey(nameof(Equipment2Id))]
    public Equipment? Equipment2 { get; set; }

    /// <summary> Экипировка, слот 3. </summary>
    public Guid? Equipment3Id { get; set; }
    /// <summary> Экипировка, слот 3. </summary>
    [ForeignKey(nameof(Equipment3Id))]
    public Equipment? Equipment3 { get; set; }

    /// <summary> Экипировка, слот 4. </summary>
    public Guid? Equipment4Id { get; set; }
    /// <summary> Экипировка, слот 4. </summary>
    [ForeignKey(nameof(Equipment4Id))]
    public Equipment? Equipment4 { get; set; }

    /// <summary> Экипировка, слот 5. </summary>
    public Guid? Equipment5Id { get; set; }
    /// <summary> Экипировка, слот 5. </summary>
    [ForeignKey(nameof(Equipment5Id))]
    public Equipment? Equipment5 { get; set; }

    /// <summary> Экипировка, слот 6. </summary>
    public Guid? Equipment6Id { get; set; }
    /// <summary> Экипировка, слот 6. </summary>
    [ForeignKey(nameof(Equipment6Id))]
    public Equipment? Equipment6 { get; set; }

    /// <summary> Экипировка, слот 7. </summary>
    public Guid? Equipment7Id { get; set; }
    /// <summary> Экипировка, слот 7. </summary>
    [ForeignKey(nameof(Equipment7Id))]
    public Equipment? Equipment7 { get; set; }

    /// <summary> Экипировка, слот 8. </summary>
    public Guid? Equipment8Id { get; set; }
    /// <summary> Экипировка, слот 8. </summary>
    [ForeignKey(nameof(Equipment8Id))]
    public Equipment? Equipment8 { get; set; }

    /// <summary> Экипировка, слот 9. </summary>
    public Guid? Equipment9Id { get; set; }
    /// <summary> Экипировка, слот 9. </summary>
    [ForeignKey(nameof(Equipment9Id))]
    public Equipment? Equipment9 { get; set; }

    /// <summary> Экипировка, слот 10. </summary>
    public Guid? Equipment10Id { get; set; }
    /// <summary> Экипировка, слот 10. </summary>
    [ForeignKey(nameof(Equipment10Id))]
    public Equipment? Equipment10 { get; set; }

    /// <summary> Экипировка, слот 11. </summary>
    public Guid? Equipment11Id { get; set; }
    /// <summary> Экипировка, слот 11. </summary>
    [ForeignKey(nameof(Equipment11Id))]
    public Equipment? Equipment11 { get; set; }

    /// <summary> Экипировка, слот 12. </summary>
    public Guid? Equipment12Id { get; set; }
    /// <summary> Экипировка, слот 12. </summary>
    [ForeignKey(nameof(Equipment12Id))]
    public Equipment? Equipment12 { get; set; }

    ///// <summary> Экипировка, слот 13. </summary>
    //public Guid? Equipment13Id { get; set; }
    ///// <summary> Экипировка, слот 13. </summary>
    //[ForeignKey(nameof(Equipment13Id))]
    //public Equipment? Equipment13 { get; set; }

    ///// <summary> Экипировка, слот 14. </summary>
    //public Guid? Equipment14Id { get; set; }
    ///// <summary> Экипировка, слот 14. </summary>
    //[ForeignKey(nameof(Equipment14Id))]
    //public Equipment? Equipment14 { get; set; }

    ///// <summary> Экипировка, слот 15. </summary>
    //public Guid? Equipment15Id { get; set; }
    ///// <summary> Экипировка, слот 15. </summary>
    //[ForeignKey(nameof(Equipment15Id))]
    //public Equipment? Equipment15 { get; set; }

    ///// <summary> Экипировка, слот 16. </summary>
    //public Guid? Equipment16Id { get; set; }
    ///// <summary> Экипировка, слот 16. </summary>
    //[ForeignKey(nameof(Equipment16Id))]
    //public Equipment? Equipment16 { get; set; }

    ///// <summary> Экипировка, слот 17. </summary>
    //public Guid? Equipment17Id { get; set; }
    ///// <summary> Экипировка, слот 17. </summary>
    //[ForeignKey(nameof(Equipment17Id))]
    //public Equipment? Equipment17 { get; set; }

    ///// <summary> Экипировка, слот 18. </summary>
    //public Guid? Equipment18Id { get; set; }
    ///// <summary> Экипировка, слот 18. </summary>
    //[ForeignKey(nameof(Equipment18Id))]
    //public Equipment? Equipment18 { get; set; }

    ///// <summary> Экипировка, слот 19. </summary>
    //public Guid? Equipment19Id { get; set; }
    ///// <summary> Экипировка, слот 19. </summary>
    //[ForeignKey(nameof(Equipment19Id))]
    //public Equipment? Equipment19 { get; set; }

    ///// <summary> Экипировка, слот 20. </summary>
    //public Guid? Equipment20Id { get; set; }
    ///// <summary> Экипировка, слот 20. </summary>
    //[ForeignKey(nameof(Equipment20Id))]
    //public Equipment? Equipment20 { get; set; }

    ///// <summary> Экипировка, слот 21. </summary>
    //public Guid? Equipment21Id { get; set; }
    ///// <summary> Экипировка, слот 21. </summary>
    //[ForeignKey(nameof(Equipment21Id))]
    //public Equipment? Equipment21 { get; set; }

    ///// <summary> Экипировка, слот 22. </summary>
    //public Guid? Equipment22Id { get; set; }
    ///// <summary> Экипировка, слот 22. </summary>
    //[ForeignKey(nameof(Equipment22Id))]
    //public Equipment? Equipment22 { get; set; }

    ///// <summary> Экипировка, слот 23. </summary>
    //public Guid? Equipment23Id { get; set; }
    ///// <summary> Экипировка, слот 23. </summary>
    //[ForeignKey(nameof(Equipment23Id))]
    //public Equipment? Equipment23 { get; set; }

    ///// <summary> Экипировка, слот 24. </summary>
    //public Guid? Equipment24Id { get; set; }
    ///// <summary> Экипировка, слот 24. </summary>
    //[ForeignKey(nameof(Equipment24Id))]
    //public Equipment? Equipment24 { get; set; }
    #endregion Equipment
}
