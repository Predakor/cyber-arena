using Systems.Guns.Modules.Shared;
using Systems.Guns.Projectiles.Physics;
using UnityEngine;

namespace Systems.Guns.Modules.ProjectileModule
{

    [CreateAssetMenu(menuName = menuPath + "/" + nameof(ProjectileModuleBase))]
    public class ProjectileModuleBase : ScriptableObject
    {
        protected const string menuPath = "Weapons/Projectiles";

        [SerializeField] protected ProjectileConfigSO _config;
        [SerializeField] protected GameObject _model;

        [Header("Pool")]
        [SerializeField] protected ushort _poolSize = 10;
        [SerializeField] protected ushort _poolMaxSize = 200;

        public virtual ShootContext GetShootContext() => new()
        {
            Size = _config.Size,
            Damage = _config.Damage,
            Speed = _config.Speed,
            Duration = _config.Duration,
            ProjectileConfig = _config,
        };
    }
}
