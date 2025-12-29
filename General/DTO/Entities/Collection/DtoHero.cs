using General.DTO.Entities.GameData;
using System;

namespace General.DTO.Entities.Collection;

/// <summary>
/// Герой из коллекции игрока.
/// </summary>
public class DtoHero()
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public int BaseHeroId { get; set; }
    public DtoBaseHero? DtoBaseHero { get; set; } = null;
    public string? GroupName { get; set; }
    public int Rarity { get; set; }
    public int Level { get; set; }
    public long ExperienceNow { get; set; }


    #region Характеристики
    // --------------Характеристики числовые----------------
    public int Strength { get; set; }
    public int Agility { get; set; }
    public int Intelligence { get; set; }
    public int CritChance { get; set; }
    public int CritPower { get; set; }
    public int Haste { get; set; }
    public int Versality { get; set; }
    public int EndurancePhysical { get; set; }
    public int EnduranceMagical { get; set; }
    public long Health1000 { get; set; }

    // --------------Характеристики кубика ДНД----------------
    public Dice? Damage { get; set; }
    #endregion Характеристики

    #region Resistances

    /// <summary> Сопротивление физическому урону. Выражается числом которое преобразовывается в проценты. </summary>
    public int ResistDamagePhysical { get; set; }

    /// <summary> Сопротивление магическому урону. Выражается числом которое преобразовывается в проценты. </summary>
    public int ResistDamageMagical { get; set; }

    #endregion Resistances

    #region Equipment
    public Guid? Equipment1Id { get; set; }
    public DtoEquipment? DtoEquipment1 { get; set; } = null;

    public Guid? Equipment2Id { get; set; }
    public DtoEquipment? DtoEquipment2 { get; set; } = null;

    public Guid? Equipment3Id { get; set; }
    public DtoEquipment? DtoEquipment3 { get; set; } = null;

    public Guid? Equipment4Id { get; set; }
    public DtoEquipment? DtoEquipment4 { get; set; } = null;

    public Guid? Equipment5Id { get; set; }
    public DtoEquipment? DtoEquipment5 { get; set; } = null;

    public Guid? Equipment6Id { get; set; }
    public DtoEquipment? DtoEquipment6 { get; set; } = null;

    public Guid? Equipment7Id { get; set; }
    public DtoEquipment? DtoEquipment7 { get; set; } = null;

    public Guid? Equipment8Id { get; set; }
    public DtoEquipment? DtoEquipment8 { get; set; } = null;

    public Guid? Equipment9Id { get; set; }
    public DtoEquipment? DtoEquipment9 { get; set; } = null;

    public Guid? Equipment10Id { get; set; }
    public DtoEquipment? DtoEquipment10 { get; set; } = null;

    public Guid? Equipment11Id { get; set; }
    public DtoEquipment? DtoEquipment11 { get; set; } = null;

    public Guid? Equipment12Id { get; set; }
    public DtoEquipment? DtoEquipment12 { get; set; } = null;

    //public Guid? Equipment13Id { get; set; }
    //public DtoEquipment? DtoEquipment13 { get; set; } = null;

    //public Guid? Equipment14Id { get; set; }
    //public DtoEquipment? DtoEquipment14 { get; set; } = null;

    //public Guid? Equipment15Id { get; set; }
    //public DtoEquipment? DtoEquipment15 { get; set; } = null;

    //public Guid? Equipment16Id { get; set; }
    //public DtoEquipment? DtoEquipment16 { get; set; } = null;

    //public Guid? Equipment17Id { get; set; }
    //public DtoEquipment? DtoEquipment17 { get; set; } = null;

    //public Guid? Equipment18Id { get; set; }
    //public DtoEquipment? DtoEquipment18 { get; set; } = null;

    //public Guid? Equipment19Id { get; set; }
    //public DtoEquipment? DtoEquipment19 { get; set; } = null;

    //public Guid? Equipment20Id { get; set; }
    //public DtoEquipment? DtoEquipment20 { get; set; } = null;

    //public Guid? Equipment21Id { get; set; }
    //public DtoEquipment? DtoEquipment21 { get; set; } = null;

    //public Guid? Equipment22Id { get; set; }
    //public DtoEquipment? DtoEquipment22 { get; set; } = null;

    //public Guid? Equipment23Id { get; set; }
    //public DtoEquipment? DtoEquipment23 { get; set; } = null;

    //public Guid? Equipment24Id { get; set; }
    //public DtoEquipment? DtoEquipment24 { get; set; } = null;
    #endregion Equipment
}
