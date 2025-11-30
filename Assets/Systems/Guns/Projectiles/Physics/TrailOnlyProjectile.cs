using Assets._Scripts.Utils;
using UnityEngine;

namespace Systems.Guns.Projectiles.Physics
{
    [RequireComponent(typeof(TrailRenderer), typeof(Rigidbody), typeof(SphereCollider))]
    public sealed class TrailOnlyProjectile : ProjectileBase<TrailOnlyProjectile, TrailProjectileConfiguration>
    {
        [SerializeField] TrailProjectileConfiguration _config;
        [SerializeField] TrailRenderer _trail;
        [SerializeField] Rigidbody _rigidbody;

        protected override void Awake()
        {
            base.Awake();
            gameObject.EnsureComponent(out _trail).EnsureComponent(out _rigidbody);
        }

        public override void Shoot()
        {
            _rigidbody.velocity = Vector3.forward * _config.Speed;
        }

        public override void Shoot(Transform origin)
        {
            transform.SetPositionAndRotation(origin.position, origin.rotation);
            _rigidbody.velocity = origin.forward * _config.Speed;
        }

        public override TrailOnlyProjectile Configure(TrailProjectileConfiguration config)
        {
            _config = config;
            _trail.startWidth = config.Size;
            _trail.endWidth = config.Size / 2;
            _trail.time = config.Size / 2;
            return this;

        }
    }

    [CreateAssetMenu(menuName = menuPath + "/" + nameof(TrailProjectileConfiguration))]
    public sealed class TrailProjectileConfiguration : ProjectileConfigSO
    {
        public TrailRenderer Trail;
    }
}
