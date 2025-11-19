using Assets._Scripts.Utils;
using System.Collections.Generic;
using Systems.Guns.HitEffects;
using Systems.Guns.Interfaces;
using UnityEngine;

namespace Systems.Guns.Projectiles {
    public interface IProjectile : IShootable { };

    public interface IProjectile<TProjectile> : IProjectile
        where TProjectile : IProjectile {
        TProjectile Configure(IConfig<TProjectile> config);
    }

    public interface IShootable {
        void Shoot();
        void Shoot(Transform origin);
    }

    public interface IProjectileConfig : IConfig {
        float Size { get; }
        int Damage { get; }
        float Speed { get; }

        IReadOnlyList<IHitEffect> Effects { get; }
    }

    public abstract class ProjectileBase<TProjectile> : MonoBehaviour, IProjectile<TProjectile>
        where TProjectile : ProjectileBase<TProjectile> {

        [SerializeField]
        protected Collider colider;

        protected virtual void Awake() {
            gameObject.EnsureComponent(out colider);
        }

        public abstract void Shoot();
        public abstract void Shoot(Transform origin);

        public abstract TProjectile Configure(IConfig<TProjectile> config);
    }
}
