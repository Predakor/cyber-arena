using Assets.Scripts.Utils;
using System.Collections.Generic;
using Systems.Guns.HitEffects;
using Systems.Guns.Interfaces;
using Systems.Guns.Modules.Shared;
using Systems.Guns.Projectiles.Physics;
using UnityEngine;

namespace Systems.Guns.Projectiles
{
    public interface IProjectile : IShootable
    {
        IProjectile Apply(ShootContext context);
    }

    public interface IShootable
    {
        void Shoot();
        void Shoot(Transform origin);
        void Shoot(Vector3 position, Vector3 direction);
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

        protected float _damage;
        protected float _speed;
        protected float _size;

        protected IReadOnlyList<IHitEffect> _effects;

        protected virtual void Awake()
        {
            gameObject.EnsureComponent(out colider);
        }

        public IProjectile Apply(ShootContext context)
        {
            ApplyContext(context);
            return this;
        }

        protected virtual void ApplyContext(ShootContext context)
        {
            _size += context.Size;
            _damage += context.Damage;
            _speed += context.Speed;
        }

        public abstract void Shoot();
        public abstract void Shoot(Transform origin);
        public abstract TProjectile Configure(TConfig config);

        public void Shoot(Vector3 position, Vector3 direction)
        {
            transform.SetPositionAndRotation(position, Quaternion.LookRotation(direction));
            Shoot(transform);
        }
    }
}
