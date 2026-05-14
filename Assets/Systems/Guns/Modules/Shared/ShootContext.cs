using Systems.Guns.Projectiles.Physics;
using UnityEngine;

namespace Systems.Guns.Modules.Shared
{
    [System.Flags]
    public enum ShootIgnoreFlag
    {
        None = 0,
        IgnoreArmor = 1 << 0,
        IgnoreShields = 1 << 1,
        IgnoreResistances = 1 << 2,
    }
    public enum ShootState
    {
        None = 0,
        Shoot = 1,
        Stop = 2,
    }

    public sealed class ShootContext
    {
        public ProjectileConfigSO ProjectileConfig { get; set; }
        public Transform Muzzle { get; set; }
        public ShotPoint[] ShotPoints { get; set; }
        public ShootState State { get; set; }

        public float Damage { get; set; }
        public float Speed { get; set; }
        public float Duration { get; set; }
        public float Size { get; set; }

        public short CritChance { get; set; }
        public short CritDamage { get; set; }

        public short EffectRadius { get; set; }
        public short EffectStrength { get; set; }
        public short EffectDuration { get; set; }

        public ShootIgnoreFlag Flags { get; private set; }

        /// <summary>Number of enemies a single projectile can pierce through.</summary>
        public byte Piercing { get; set; }
        public int AmmoCost { get; set; } = 1;

        public bool IgnoreArmor
        {
            get => (Flags & ShootIgnoreFlag.IgnoreArmor) != 0;
            set => Flags = value ? Flags | ShootIgnoreFlag.IgnoreArmor : Flags & ~ShootIgnoreFlag.IgnoreArmor;
        }
        public bool IgnoreShields
        {
            get => (Flags & ShootIgnoreFlag.IgnoreShields) != 0;
            set => Flags = value ? Flags | ShootIgnoreFlag.IgnoreShields : Flags & ~ShootIgnoreFlag.IgnoreShields;
        }
        public bool IgnoreResistances
        {
            get => (Flags & ShootIgnoreFlag.IgnoreResistances) != 0;
            set => Flags = value ? Flags | ShootIgnoreFlag.IgnoreResistances : Flags & ~ShootIgnoreFlag.IgnoreResistances;
        }

        public ShootContext Clone() => new()
        {
            ProjectileConfig = ProjectileConfig,
            Muzzle = Muzzle,
            ShotPoints = ShotPoints,
            State = State,
            Damage = Damage,
            Speed = Speed,
            Duration = Duration,
            Size = Size,
            AmmoCost = AmmoCost,
            CritChance = CritChance,
            CritDamage = CritDamage,
            EffectRadius = EffectRadius,
            EffectStrength = EffectStrength,
            EffectDuration = EffectDuration,
        };

        public ShootContext ApplyStats(IWeaponStats stats)
        {
            Damage = stats.Damage;
            Speed = stats.Speed;
            Duration = stats.Duration;
            Size = stats.Size;
            AmmoCost = stats.AmmoCost;
            CritChance = stats.CritChance;
            CritDamage = stats.CritDamage;
            EffectRadius = stats.EffectRadius;
            EffectStrength = stats.EffectStrength;
            EffectDuration = stats.EffectDuration;

            return this;
        }

        public ShootContext SetShootState(ShootState newState)
        {
            State = newState;
            return this;
        }

        public ShootContext SetMuzzle(Transform muzzle)
        {
            Muzzle = muzzle;
            return this;
        }
    }
}
