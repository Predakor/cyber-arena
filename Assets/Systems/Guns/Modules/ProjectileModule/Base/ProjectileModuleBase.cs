using Systems.Guns.Modules.Shared;
using Systems.Guns.Projectiles.Physics;
using UnityEngine;

namespace Systems.Guns.Modules.ProjectileModule
{
    public abstract class ProjectileModuleBase : MonoBehaviour
    {
        [Header("Pool")]
        [SerializeField] protected ushort _poolSize = 10;
        [SerializeField] protected ushort _poolMaxSize = 200;

        public abstract ShootContext GetShootContext();

        protected static ShootContext ContextFrom(ProjectileConfigSO config) => new()
        {
            Size = config.Size,
            Damage = config.Damage,
            Speed = config.Speed,
            Duration = config.Duration,
            ProjectileConfig = config,
        };
    }
}
