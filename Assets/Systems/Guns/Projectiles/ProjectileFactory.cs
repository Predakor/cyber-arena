using System;
using Systems.Guns.Interfaces;
using Systems.Guns.Projectiles.Physics;
using Systems.Shared;
using UnityEngine;

namespace Systems.Guns.Projectiles {
    internal sealed class ProjectileFactory : Singleton<ProjectileFactory> {
        [SerializeField]
        TrailOnlyProjectile trailProjectile;

        public IProjectile Create(IConfig config) {
            return config switch {
                IConfig<TrailOnlyProjectile> gunConfig => Instantiate(trailProjectile)
                    .Configure(gunConfig),
                //IConfig<Meele> meleeConfig => Instantiate(_meele).Configure(meleeConfig).gameObject,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
