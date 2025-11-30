using Assets.Scripts.Utils;
using UnityEngine;

namespace Systems.Guns.Projectiles.Physics
{
    [RequireComponent(typeof(TrailRenderer), typeof(SphereCollider))]
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
            _rigidbody.velocity = Vector3.forward * _config.speed;
        }

        public override void Shoot(Transform origin)
        {
            transform.SetPositionAndRotation(origin.position, origin.rotation);
            _rigidbody.velocity = origin.forward * _config.speed;
        }

        public override TrailOnlyProjectile Configure(TrailProjectileConfiguration config)
        {
            _config = config;
            _trail.startWidth = config.size;
            _trail.endWidth = config.size / 2;
            _trail.time = config.size / 2;
            return this;

        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.LogWarning("Enter");
            HitHandler.Handle(other.gameObject, _config, transform);
            Destroy(gameObject);
        }

    }

    [CreateAssetMenu(menuName = menuPath + "/" + nameof(TrailProjectileConfiguration))]
    public sealed class TrailProjectileConfiguration : ProjectileConfigSO
    {
        public TrailRenderer Trail;
    }
}
