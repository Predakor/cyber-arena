using Assets.Scripts.Utils;
using Systems.Guns.Modules.Shared;
using Systems.Guns.Projectiles.Utils;
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
            _damage = config.damage;
            _speed = config.speed;
            _explosionRadius = config.ExplosionRadius;
            _effects = config.Effects;
            return this;
        }

        protected override void ApplyContext(ShootContext context)
        {
            _damage = (int)context.Damage;
            _speed = context.Speed;
            _explosionRadius = context.EffectRadius;
        }

        public override void Shoot()
        {
            _trusterVfx.Play();
            _rb.velocity = transform.forward * _speed;
        }

        public override void Shoot(Transform origin)
        {
            transform.SetPositionAndRotation(origin.position, origin.rotation);
            Shoot();
        }

        private void OnTriggerEnter(Collider other)
        {
            _trusterVfx.Stop();
            _rb.velocity = Vector3.zero;
            HitHandler.Handle(other.gameObject, _damage, _effects, transform);
            Destroy(gameObject, 2f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
    }

    [CreateAssetMenu(menuName = menuPath + "/" + nameof(RocketProjectileConfig))]
    public class RocketProjectileConfig : ProjectileConfigSO
    {
        [SerializeField] private float explosionRadius = 1.0f;
        public float ExplosionRadius => explosionRadius;
    }
}
