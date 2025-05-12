using System;

[Serializable]
public class PlayerEffect
{
    public PlayerEffectType type;
    public int ManaCost;
    public float Duration;
    public float Value;

    public PlayerEffect(PlayerEffectType type, float value, int manaCost = 0, float duration = 0)
    {
        this.type = type;
        Value = value;
        ManaCost = manaCost;
        Duration = duration;
    }
}

public enum PlayerEffectType
{
    DamageBoost,
    Healing,
    Invincibility,
    ManaRegeneration,
    RaiseMaxHp,
    RaiseMaxMana,
    RaiseDamage,
    RaiseGatheredMana,
    RaiseGatheredGold
}


