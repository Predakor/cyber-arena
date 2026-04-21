using System;
using Systems.Guns.Modules.Shared;
using Systems.Guns.Projectiles.Physics;
using Systems.Weapons.Guns.Modules;
using UnityEngine;

namespace Systems.Guns.Modules.ProjectileModule
{
    public abstract class ProjectileModuleBase : MonoBehaviour, IGunModule
    {
        [Header("Pool")]
        [SerializeField] protected ushort _poolSize = 10;
        [SerializeField] protected ushort _poolMaxSize = 200;

        public virtual string Name { get; protected set; } = "Projectile Module";

        public abstract ShootContext GetShootContext();

        public void Apply(WeaponStatsBuilder stats)
        {
            var context = GetShootContext();
            stats.Damage = context.Damage;
            stats.Speed = context.Speed;
            stats.Size = context.Size;
            stats.Duration = context.Duration;
            stats.AmmoCost = context.AmmoCost;
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
