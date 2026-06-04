using Assets.Scripts.Utils;
using Systems.Guns.Modules.Shared;

using UnityEngine;

namespace Systems.Guns.Projectiles.Physics.Rocket
{
    public sealed class RocketProjectile : ProjectileBase<RocketProjectile, RocketProjectileConfig>
    {
        [SerializeField] private ParticleSystem _trusterVfx;
        [SerializeField] private ParticleSystem _explosionVfx;
        [SerializeField] private Rigidbody _rb;

        private float _explosionRadius;

        protected override void Awake()
        {
            base.Awake();
            gameObject.EnsureComponent(out _rb);
        }

        public override RocketProjectile Configure(RocketProjectileConfig config)
        {
            _damage = config.Damage;
            _speed = config.Speed;
            _explosionRadius = config.ExplosionRadius;
            _effects = config.Effects;
            return this;
        }

        protected override void ApplyContext(ShootContext context)
        {
            base.ApplyContext(context);
            _explosionRadius += context.EffectRadius;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            _trusterVfx.Stop();
            _trusterVfx.Clear();
            _rb.linearVelocity = Vector3.zero;
            Destroy(gameObject);
        }

        public override void Shoot()
        {
            _trusterVfx.Play();
            _rb.linearVelocity = transform.forward * _speed;
        }

        public override void Shoot(Transform origin)
        {
            transform.SetPositionAndRotation(origin.position, origin.rotation);
            Shoot();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
    }
}
