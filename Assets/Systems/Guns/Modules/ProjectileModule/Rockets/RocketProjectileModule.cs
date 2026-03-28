using Systems.Guns.Modules.Shared;
using Systems.Guns.Projectiles.Physics.Rocket;
using UnityEngine;

namespace Systems.Guns.Modules.ProjectileModule
{
    public sealed class RocketProjectileModule : ProjectileModuleBase
    {
        [SerializeField][TypedSOSelector] private RocketProjectileConfig _config;

        public override ShootContext GetShootContext() => ContextFrom(_config);
    }
}