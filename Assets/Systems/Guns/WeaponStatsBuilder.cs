using System;
using System.Collections.Generic;
using System.Linq;
using Systems.Guns.Modules.ProjectileModule;
using Systems.Guns.Modules.Shared;
using Systems.Guns.Stats;
using Systems.Weapons.Guns.Modules;

namespace Systems.Guns
{
    public sealed record StatDescriptor(string Name, float Value);

    public sealed class WeaponStatsBuilder
    {
        private readonly Dictionary<StatType, List<StatModifier>> _modifiers = new();

        public float Damage { get; set; }
        public float Speed { get; set; }
        public float Duration { get; set; }
        public float Size { get; set; }
        public short CritChance { get; set; }
        public short CritDamage { get; set; }
        public short EffectRadius { get; set; }
        public short EffectStrength { get; set; }
        public short EffectDuration { get; set; }
        public ShootIgnoreFlag Flags { get; set; }
        public byte Piercing { get; set; } = 1;
        public int AmmoCost { get; set; } = 1;

        public List<StatDescriptor> Extras { get; private set; } = new();

        private static readonly Array enumTypes = Enum.GetValues(typeof(StatType));

        private WeaponStatsBuilder()
        {
            foreach (StatType type in enumTypes)
            {
                _modifiers[type] = new List<StatModifier>();
            }
        }

        public static WeaponStatsBuilder FromProjectileBase(ProjectileModuleBase @base)
        {
            var builder = new WeaponStatsBuilder();

            var context = @base.GetShootContext();
            builder.Damage = context.Damage;
            builder.Speed = context.Speed;
            builder.Size = context.Size;
            builder.Duration = context.Duration;
            builder.AmmoCost = context.AmmoCost;

            return builder.ApplyModuleModifiers(@base);
        }

        public WeaponStatsBuilder ApplyModuleModifiers(IGunModule module)
        {
            module?.Apply(this);
            return this;
        }

        public void AddModifier(StatModifier mod)
        {
            _modifiers[mod.Stat].Add(mod);
        }
        public void AddModifierList(IEnumerable<StatModifier> mods)
        {
            foreach (var mod in mods)
            {
                AddModifier(mod);
            }
        }

        public void AddExtra(string name, float value)
        {
            Extras.Add(new StatDescriptor(name, value));
        }

        public WeaponStats Build()
        {
            return new WeaponStats
            {
                Damage = CalculateStat(StatType.Damage, Damage),
                Speed = CalculateStat(StatType.Speed, Speed),
                Size = CalculateStat(StatType.Size, Size),
                Duration = CalculateStat(StatType.Duration, Duration),
                AmmoCost = (int)CalculateStat(StatType.AmmoCost, AmmoCost),

                CritChance = (short)CalculateStat(StatType.CritChance, CritChance),
                CritDamage = (short)CalculateStat(StatType.CritDamage, CritDamage),
                EffectRadius = (short)CalculateStat(StatType.EffectRadius, EffectRadius),
                EffectStrength = (short)CalculateStat(StatType.EffectStrength, EffectStrength),
                EffectDuration = (short)CalculateStat(StatType.EffectDuration, EffectDuration),

                Piercing = (byte)CalculateStat(StatType.Piercing, Piercing),
                Flags = Flags, // Not mathematically modified

                Custom = Extras.ToArray()
            };
        }

        private float CalculateStat(StatType type, float baseValue)
        {
            float finalValue = baseValue;
            float percentAddSum = 0f;

            var mods = _modifiers[type];

            var flatAdds = mods.Where(x => x.Type == ModifierType.FlatAdd);
            var percentAdds = mods.Where(x => x.Type == ModifierType.PercentAdd);
            var percentMults = mods.Where(x => x.Type == ModifierType.PercentMult);


            // Apply Flat additions first
            foreach (var mod in flatAdds)
            {
                finalValue += mod.Value;
            }

            // Sum up Percent Add (e.g., two +20% modifiers equal +40%)
            foreach (var mod in percentAdds)
            {
                percentAddSum += mod.Value;
            }

            if (percentAddSum != 0)
            {
                finalValue *= 1.0f + percentAddSum;
            }

            // Apply Percent Multipliers (e.g., x1.5)
            foreach (var mod in percentMults)
            {
                finalValue *= mod.Value;
            }

            return finalValue;
        }
    }
}
