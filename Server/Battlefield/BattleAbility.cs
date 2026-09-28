using General.DTO.Battlefield;
using General.DTO.Entities.GameData;

namespace Server.BattleField;

/// <summary>Хранит личное состояние способности героя, используя общую запись её базовых параметров.</summary>
public abstract class BattleAbility(Ability definition)
{
    public Ability definition { get; } = definition;
    public EBattlefieldLogAbility id => definition.code;
    public float effectiveness { get; protected set; }
    public int cooldownRemaining { get; private set; }

    public void ResetEffectiveness()
    {
        effectiveness = 0f;
    }

    /// <summary>В начале нового хода уменьшает оставшийся кулдаун, не допуская отрицательных значений.</summary>
    public void ReduceCooldown()
    {
        if (cooldownRemaining > 0)
        {
            cooldownRemaining--;
        }
    }

    /// <summary>После успешного применения устанавливает кулдаун из общих базовых параметров.</summary>
    protected void StartCooldown()
    {
        cooldownRemaining = Math.Max(0, definition.cooldown);
    }

    /// <summary>Обновляет эффективность; недоступная способность получает нулевую эффективность.</summary>
    public abstract void CalcEffectiveness(SpawnedHero caster, BattleAbilityContext context);

    /// <summary>Проверяет условия, выбирает цели и применяет способность, запуская её кулдаун.</summary>
    /// <returns>True, если способность применена; при false состояние боя и журнал не изменяются.</returns>
    public abstract bool UseAbility(SpawnedHero caster, BattleAbilityContext context);
}
