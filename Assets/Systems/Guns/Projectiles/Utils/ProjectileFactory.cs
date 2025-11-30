using System;
using Systems.Guns.Projectiles.Physics;
using Systems.Shared;
using UnityEngine;

namespace Systems.Guns.Projectiles
{
    internal sealed class ProjectileFactory : Singleton<ProjectileFactory>
    {
        [SerializeField]
        TrailOnlyProjectile trailProjectile;

        //TODO Add pooling

        public IProjectile Create(ProjectileConfigSO config)
        {
            return config switch
            {
                TrailProjectileConfiguration c => Instantiate(trailProjectile).Configure(c),
                null => throw new NullReferenceException("Config was not passed"),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
