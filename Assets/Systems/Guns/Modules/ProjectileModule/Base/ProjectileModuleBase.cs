using System;
using System.Collections.Generic;
using Systems.Guns.Modules.Shared;
using Systems.Guns.Projectiles.Physics;
using Systems.Guns.Stats;
using Systems.Weapons.Guns.Modules;
using UnityEngine;

namespace Systems.Guns.Modules.ProjectileModule
{
    public abstract class ProjectileModuleBase : MonoBehaviour, IGunModule
    {
        [Header("Pool")]
        [SerializeField] protected ushort _poolSize = 10;
        [SerializeField] protected ushort _poolMaxSize = 200;

        [SerializeField]
        protected List<StatModifier> statModifiers = new()
        {

        };

        public virtual string Name { get; protected set; } = "Projectile Module";

        public abstract ShootContext GetShootContext();

        public void Apply(WeaponStatsBuilder stats)
        {
            var context = GetShootContext();

            stats.AddModifier(StatModifier.Flat(StatType.Damage, context.Damage));
            stats.AddModifier(StatModifier.Flat(StatType.Speed, context.Speed));
            stats.AddModifier(StatModifier.Flat(StatType.Size, context.Size));
            stats.AddModifier(StatModifier.Flat(StatType.Duration, context.Duration));
            stats.AddModifier(StatModifier.Flat(StatType.AmmoCost, context.AmmoCost));

            stats.AddModifierList(statModifiers);
        }

        protected static ShootContext ContextFrom(ProjectileConfigSO config) => new()
        {
            Size = config.Size,
            Damage = config.Damage,
            Speed = config.Speed,
            AmmoCost = config.AmmoCost,
            Duration = config.Duration,
            ProjectileConfig = config,
        };

        public void Handle(ShootContext context, Action<ShootContext> next)
        {
            next(context);
        }
    }
}
