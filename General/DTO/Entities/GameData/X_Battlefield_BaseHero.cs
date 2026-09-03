namespace General.DTO.Entities.GameData;

public class X_Battlefield_BaseHero
{
    public int id { get; set; }
    public EBattleFiled battlefieldId { get; set; }
    public Battlefield battlefield { get; set; } = null!;
    public int baseHeroId { get; set; }
    public BaseHero baseHero { get; set; } = null!;

    /// <summary> Гарантированное появление на поле боя. </summary>
    public bool guarantSpawn { get; set; }

    /// <summary> Вероятность появления на поле боя. Чем выше число тем выше шанс. Игнорируется при GuarantSpawn=true. </summary>
    public int probabilitySpawn { get; set; }

    /// <summary> При респауне может иметь ранг. </summary>
    public bool possibleRank { get; set; }
    public int count { get; set; } = 1;

    public X_Battlefield_BaseHero Copy()
    {
        return (X_Battlefield_BaseHero)MemberwiseClone();
    }
}
