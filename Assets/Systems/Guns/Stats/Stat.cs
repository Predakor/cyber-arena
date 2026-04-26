using System;

namespace Systems.Guns.Stats
{
    public enum StatType
    {
        Damage,
        Speed,
        Size,
        Duration,
        AmmoCost,
        Piercing,
        CritChance,
        CritDamage,
        EffectRadius,
        FireRate,
        MagazineSize,
        ReloadSpeed,
        EffectStrength,
        EffectDuration,
        Spread
    }

    public enum ModifierType
    {
        FlatAdd,       // e.g., +5 Damage
        PercentAdd,    // e.g., +50% Damage (Added together before multiplying)
        PercentMult    // e.g., x1.5 Damage (Multiplied sequentially)
    }

    [Serializable]
    public struct StatModifier
    {
        public StatType Stat;
        public ModifierType Type;
        public float Value;

        public static StatModifier Flat(StatType stat, float value) => new(stat, ModifierType.FlatAdd, value);
        public static StatModifier PercentAdd(StatType stat, float value) => new(stat, ModifierType.PercentAdd, value);
        public static StatModifier PercentMult(StatType stat, float value) => new(stat, ModifierType.PercentMult, value);

        private StatModifier(StatType stat, ModifierType type, float value)
        {
            Stat = stat;
            Type = type;
            Value = value;
        }
    }

}
