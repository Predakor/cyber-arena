using Assets.Scripts.Utils;
using UnityEngine;

namespace Systems.Guns.Projectiles.Physics.Rocket
{
    public sealed class RocketProjectile : ProjectileBase<RocketProjectile, RocketProjectileConfig>
    {
        [SerializeField] ParticleSystem _trusterVfx;
        [SerializeField] ParticleSystem _explosionVfx;

        [SerializeField] Rigidbody _rb;
        [SerializeField] RocketProjectileConfig _config;


        protected override void Awake()
        {
            base.Awake();
            gameObject.EnsureComponent(out _rb);
        }

        public override RocketProjectile Configure(RocketProjectileConfig config)
        {
            _config = config;
            return this;
        }

        public override void Shoot()
        {
            _trusterVfx.Play();
            _rb.velocity = transform.forward * _config.speed;

        }

        public override void Shoot(Transform origin)
        {
            transform.SetPositionAndRotation(origin.position, origin.rotation);
            Shoot();
        }

        private void OnTriggerEnter(Collider other)
        {
            //_explosionVfx.Play();
            _trusterVfx.Stop();
            _rb.velocity = Vector3.zero;

            HitHandler.Handle(other.gameObject, _config, transform);
            Destroy(gameObject, 2f);
        }

        private void OnDrawGizmosSelected()
        {
            if (_config != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, _config.ExplosionRadius);
            }
        }
    }

    [CreateAssetMenu(menuName = menuPath + "/" + nameof(RocketProjectileConfig))]
    public class RocketProjectileConfig : ProjectileConfigSO
    {
        [SerializeField] float explosionRadius = 1.0f;

        public float ExplosionRadius { get => explosionRadius; }


    }
}
