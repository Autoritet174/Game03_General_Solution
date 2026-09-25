using General;
using General.DTO.Battlefield;

namespace Server.Battlefield.Abilities;

/// <summary>Определяет проверку условий и применение боевой способности.</summary>
public interface IBattleAbility
{
    EBattlefieldLogAbility id { get; }

    /// <summary>Проверяет условия, самостоятельно выбирает цели и применяет способность.</summary>
    /// <returns>True, если способность применена; при false состояние боя и журнал не изменяются.</returns>
    bool TryUse(SpawnedHero caster, BattleAbilityContext context);
}
