namespace General.DTO.Entities.GameData;

public class X_Battlefield_BaseHero
{
    public int Id { get; set; }
    public EBattleFiled BattlefieldId { get; set; }
    public Battlefield Battlefield { get; set; } = null!;
    public int BaseHeroId { get; set; }
    public BaseHero BaseHero { get; set; } = null!;

    /// <summary> Гарантированное появление на поле боя. </summary>
    public bool GuarantSpawn { get; set; }

    /// <summary> Вероятность появления на поле боя. Чем выше число тем выше шанс. Игнорируется при GuarantSpawn=true. </summary>
    public int ProbabilitySpawn { get; set; }

    /// <summary> При респауне может иметь ранг. </summary>
    public bool PossibleRank { get; set; }
    public int Count { get; set; } = 1;

    public X_Battlefield_BaseHero Copy()
    {
        return (X_Battlefield_BaseHero)MemberwiseClone();
    }
}
