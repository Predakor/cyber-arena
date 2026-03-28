using Assets.Scripts.Utils;
using Systems.Guns.Modules.Shared;
using Systems.Guns.Projectiles.Utils;
using UnityEngine;

namespace Systems.Guns.Projectiles.Physics
{
    [RequireComponent(typeof(TrailRenderer), typeof(SphereCollider))]
    public sealed class TrailOnlyProjectile : ProjectileBase<TrailOnlyProjectile, TrailProjectileConfiguration>
    {
        [SerializeField] private TrailRenderer _trail;
        [SerializeField] private Rigidbody _rigidbody;

        protected override void Awake()
        {
            base.Awake();
            gameObject.EnsureComponent(out _trail).EnsureComponent(out _rigidbody);
        }

        public override TrailOnlyProjectile Configure(TrailProjectileConfiguration config)
        {
            _size = config.Size;
            _damage = config.damage;
            _speed = config.speed;
            _effects = config.Effects;

            _trail.time = config.trailTime;
            _trail.startWidth = config.startWidth;
            _trail.endWidth = config.endWidth;
            _trail.colorGradient = config.colorGradient;
            return this;
        }

        protected override void ApplyContext(ShootContext context)
        {
            _trail.startWidth += Mathf.Max(0.01f, context.Size * 2);
            _trail.endWidth += Mathf.Max(0.01f, context.Size) / 2;
            _trail.time = Mathf.Max(01f, _trail.time - context.Speed);
        }

        public override void Shoot()
        {
            Debug.Log($"[TrailProjectile] Shoot | damage={_damage} speed={_speed} size={_size}");
            _rigidbody.velocity = transform.forward * _speed;
        }

        public override void Shoot(Transform origin)
        {
            Debug.Log($"[TrailProjectile] Shoot | damage={_damage} speed={_speed} size={_size} pos={origin.position} dir={origin.forward}");
            transform.SetPositionAndRotation(origin.position, origin.rotation);
            _rigidbody.velocity = origin.forward * _speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            HitHandler.Handle(other.gameObject, (int)_damage, _effects, transform);
            Destroy(gameObject);
        }
    }
}
