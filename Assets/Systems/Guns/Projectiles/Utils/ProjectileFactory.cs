using System;
using Systems.Guns.Projectiles.Physics;
using Systems.Guns.Projectiles.Physics.Rocket;
using Systems.Shared;
using UnityEngine;

namespace Systems.Guns.Projectiles
{
    internal sealed class ProjectileFactory : Singleton<ProjectileFactory>
    {
        [SerializeField] TrailOnlyProjectile trailProjectile;
        [SerializeField] RocketProjectile _rocketProjectile;
        //TODO Add pooling

        public IProjectile Create(ProjectileConfigSO config)
        {
            return config switch
            {
                TrailProjectileConfiguration c => Instantiate(trailProjectile).Configure(c),
                RocketProjectileConfig c => Instantiate(_rocketProjectile).Configure(c),
                null => throw new NullReferenceException("Config was not passed"),
                _ => throw new NotImplementedException(nameof(config) + " Has no method in factory")
            };
        }
    }
}
