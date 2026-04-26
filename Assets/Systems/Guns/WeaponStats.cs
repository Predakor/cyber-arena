using System;
using System.Collections.Generic;
using Systems.Guns.Modules.Shared;

namespace Systems.Guns
{
    public interface IWeaponStats
    {
        int AmmoCost { get; }
        short CritChance { get; }
        short CritDamage { get; }
        float Damage { get; }
        float Duration { get; }
        short EffectDuration { get; }
        short EffectRadius { get; }
        short EffectStrength { get; }
        ShootIgnoreFlag Flags { get; }
        byte Piercing { get; }
        float Size { get; }
        float Speed { get; }

        IReadOnlyList<StatDescriptor> Custom { get; }
    }

    [Serializable]
    public sealed record WeaponStats : IWeaponStats
    {
        public float Damage { get; init; }
        public float Speed { get; init; }
        public float Duration { get; init; }
        public float Size { get; init; }

        public short CritChance { get; init; }
        public short CritDamage { get; init; }

        public short EffectRadius { get; init; }
        public short EffectStrength { get; init; }
        public short EffectDuration { get; init; }

        public ShootIgnoreFlag Flags { get; init; }

        public byte Piercing { get; init; } = 1;
        public int AmmoCost { get; init; } = 1;

        public IReadOnlyList<StatDescriptor> Custom { get; init; } = new List<StatDescriptor>();
    }
}
