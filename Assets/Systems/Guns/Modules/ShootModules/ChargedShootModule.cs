using Systems.Guns.Projectiles;
using Systems.Guns.Projectiles.Physics;
using UnityEngine;

namespace Systems.Guns.Modules.ShootModules
{
    public class ChargedShootModule : ShootModuleBase
    {
        [SerializeField]
        private float _minChargeTime;

        [SerializeField]
        private float _maxChargeTime;

        [SerializeField]
        private Projectile _projectile;

        [SerializeField]
        private Transform _muzzle;

        [SerializeField]
        private ProjectileConfigSO _projectileConfig;

        [SerializeField]
        private ParticleSystem _chargeEffect;

        private ChargeTimer _chargeTracker;

        private ParticleSystem _particles;
        private bool MinChargeTimeExceeded => _chargeTracker.GetDuration() > _minChargeTime;

        protected override void Awake()
        {
            base.Awake();
            _chargeTracker = new ChargeTimer();
        }

        public override void Pressed()
        {
            if (_chargeTracker.State != ChargeState.None)
            {
                return;
            }

            _chargeTracker.Start();
            _particles = Instantiate(_chargeEffect, _muzzle);
            _particles.Play();
        }

        public override void Released()
        {
            _chargeTracker.Stop();
            _particles.Stop();

            if (!MinChargeTimeExceeded)
            {
                _chargeTracker.Reset();
                return;
            }

            var chargeTime = _chargeTracker.GetDuration();

            var config = CreateProjectileConfig(chargeTime);

            ProjectileFactory.Instance.Create(config).Shoot(_muzzle);

            fireRateController.Fired();

            _chargeTracker.Reset();
        }

        private ProjectileConfigSO CreateProjectileConfig(float chargeTime)
        {
            var config = Instantiate(_projectileConfig);
            config.size *= (2 * chargeTime);
            config.speed += (2 * chargeTime);
            config.damage = Mathf.RoundToInt(config.damage * chargeTime);
            return config;
        }
    }
}
