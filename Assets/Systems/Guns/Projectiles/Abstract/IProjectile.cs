using Assets._Scripts.Utils;
using System.Collections.Generic;
using Systems.Guns.HitEffects;
using Systems.Guns.Interfaces;
using Systems.Guns.Projectiles.Physics;
using UnityEngine;

namespace Systems.Guns.Projectiles
{
    public interface IProjectile : IShootable { };

    public interface IShootable
    {
        void Shoot();
        void Shoot(Transform origin);
    }

    public interface IProjectileConfig : IConfig
    {
        float Size { get; }
        int Damage { get; }
        float Speed { get; }

        IReadOnlyList<IHitEffect> Effects { get; }
    }

    public abstract class ProjectileBase<TProjectile, TConfig> : MonoBehaviour, IProjectile
        where TProjectile : ProjectileBase<TProjectile, TConfig>
        where TConfig : ProjectileConfigSO
    {
        [SerializeField] protected Collider colider;
        protected virtual void Awake()
        {
            gameObject.EnsureComponent(out colider);
        }

        public abstract void Shoot();
        public abstract void Shoot(Transform origin);
        public abstract TProjectile Configure(TConfig config);
    }
}
