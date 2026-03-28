using Systems.Guns.Modules.Shared;
using Systems.Guns.Projectiles.Physics;
using UnityEngine;

namespace Systems.Guns.Modules.ProjectileModule
{
    public sealed class BulletProjectileModule : ProjectileModuleBase
    {
        [SerializeField] private TrailProjectileConfiguration _config;

        public override ShootContext GetShootContext() => ContextFrom(_config);
    }
}