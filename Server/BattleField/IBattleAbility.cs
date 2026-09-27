using General.DTO.Battlefield;

namespace Server.BattleField;

/// <summary>Определяет проверку условий и применение боевой способности.</summary>
public interface IBattleAbility
{
    float effectiveness { get;} 
    EBattlefieldLogAbility id { get; }
    void ResetEffectiveness();

    /// <summary>Проверяет условия, самостоятельно выбирает цели и применяет способность.</summary>
    /// <returns>True, если способность применена; при false состояние боя и журнал не изменяются.</returns>
    void CalcEffectiveness(SpawnedHero caster, BattleAbilityContext context);

    bool UseAbility(SpawnedHero caster, BattleAbilityContext context);

}
