using Systems.Guns.Projectiles;
using Systems.Guns.Projectiles.Physics;
using UnityEngine;

namespace Systems.Guns.Modules.ShootModules {
    public class ChargedShootModule : ShootModuleBase {
        [SerializeField]
        private float _minChargeTime;

        [SerializeField]
        private float _maxChargeTime;

        [SerializeField]
        private Projectile _projectile;

        [SerializeField]
        private Transform _muzzle;

        [SerializeField]
        private ProjectileConfigurationSO _projectileConfig;

        [SerializeField]
        private ParticleSystem _chargeEffect;

        private ChargeTimer _chargeTracker;


        private ParticleSystem _particles;
        private bool MinChargeTimeExceeded => _chargeTracker.GetDuration() > _minChargeTime;

        protected override void Awake() {
            base.Awake();
            _chargeTracker = new ChargeTimer();
        }

        public override void Pressed() {
            if (_chargeTracker.State == ChargeState.None) {
                _chargeTracker.Start();
                _particles = Instantiate(_chargeEffect, _muzzle);
                _particles.Play();
            }
        }

        public override void Released() {
            _chargeTracker.Stop();
            _particles.Stop();


            if (!MinChargeTimeExceeded) {
                _chargeTracker.Reset();
                return;
            }


            var chargeTime = _chargeTracker.GetDuration();

            var config = CreateProjectileConfig(chargeTime);

            var projectile = ProjectileFactory.Instance.Create(config);
            projectile.Shoot(_muzzle);

            fireRateController.Fired();

            _chargeTracker.Reset();
        }

        private ProjectileConfigurationSO CreateProjectileConfig(float chargeTime) {
            var config = Instantiate(_projectileConfig);
            config.Size *= (2 * chargeTime);
            config.Speed += (2 * chargeTime);
            config.Damage = Mathf.RoundToInt(config.Damage * chargeTime);
            return config;
        }
    }
}


