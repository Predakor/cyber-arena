using System.Collections.Generic;
using Systems.Guns.Modules.ProjectileModule;
using Systems.Guns.Modules.Shared;
using Systems.Weapons.Guns.Modules;

namespace Systems.Guns
{
    public sealed record StatDescriptor(string Name, float Value);

    public sealed class WeaponStatsBuilder
    {
        private WeaponStatsBuilder() { }
        public static WeaponStatsBuilder FromProjectileBase(ProjectileModuleBase @base)
        {
            return new WeaponStatsBuilder().ApplyModuleModifiers(@base as IGunModule);
        }
        public WeaponStatsBuilder ApplyModuleModifiers(IGunModule module)
        {
            module.Apply(this);
            return this;
        }

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
        public void AddExtra(string name, float value) => Extras.Add(new StatDescriptor(name, value));

        public WeaponStats Build() => new()
        {
            Damage = Damage,
            Speed = Speed,
            Duration = Duration,
            Size = Size,
            CritChance = CritChance,
            CritDamage = CritDamage,
            EffectRadius = EffectRadius,
            EffectStrength = EffectStrength,
            EffectDuration = EffectDuration,
            Flags = Flags,
            Piercing = Piercing,
            AmmoCost = AmmoCost,
            Custom = Extras.ToArray()
        };

    }
}
