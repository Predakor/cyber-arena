using System;
using Systems.Guns.HitEffects;
using Systems.Guns.Projectiles.Physics;
using Systems.Guns.Projectiles.Physics.Rocket;
using Systems.Shared;
using UnityEngine;

namespace Systems.Guns.Projectiles
{
    public sealed class ProjectileFactory : Singleton<ProjectileFactory>
    {
        [SerializeField] private TrailOnlyProjectile trailProjectile;
        [SerializeField] private RocketProjectile _rocketProjectile;

        private static Action<HitInfo> _hitHandler;
        //TODO Add pooling

        public static void Configure(Action<HitInfo> hitHandler)
        {
            if (hitHandler is null)
            {
                return;
            }

            _hitHandler = hitHandler;
        }

        public IProjectile Create(ProjectileConfigSO config)
        {
            IProjectile projectile = config switch
            {
                TrailProjectileConfiguration c => Instantiate(trailProjectile).Configure(c),
                RocketProjectileConfig c => Instantiate(_rocketProjectile).Configure(c),
                null => throw new NullReferenceException("Config was not passed"),
                _ => throw new NotImplementedException(nameof(config) + " Has no method in factory")
            };

            projectile?.OnHit(_hitHandler);

            return projectile;
        }
    }
}
