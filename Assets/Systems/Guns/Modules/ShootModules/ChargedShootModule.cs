using System;
using Systems.Guns.Modules.Shared;
using UnityEngine;

namespace Systems.Guns.Modules.ShootModules
{
    public sealed class ChargedShootModule : FireRateModuleBase
    {
        [SerializeField] private float _minChargeTime;
        [SerializeField] private float _maxChargeTime;

        [SerializeField] private float _maxDamageMultiplier = 3f;
        [SerializeField] private float _maxSpeedMultiplier = 2f;

        [SerializeField] private Transform _muzzle;        // used only for VFX attachment
        [SerializeField] private ParticleSystem _chargeEffect;

        private ChargeTimer _chargeTracker;
        private ParticleSystem _particles;

        private bool MinChargeTimeExceeded => _chargeTracker.GetDuration() > _minChargeTime;

        protected override void Awake()
        {
            base.Awake();
            _chargeTracker = new ChargeTimer(_maxChargeTime);
        }

        public override void Pressed(ShootContext context, Action<ShootContext> next)
        {
            if (_chargeTracker.State != ChargeState.None)
            {
                return;
            }

            _chargeTracker.Start();
            _particles = Instantiate(_chargeEffect, _muzzle);
            _particles.Play();
        }

        public override void Released(ShootContext context, Action<ShootContext> next)
        {
            _chargeTracker.Stop();
            _particles.Stop();

            if (!MinChargeTimeExceeded)
            {
                _chargeTracker.Reset();
                return;
            }

            float chargePercent = _chargeTracker.GetMaxChargePercentile();


            context.Damage = Mathf.Lerp(context.Damage, context.Damage * _maxDamageMultiplier, chargePercent);
            context.Speed = Mathf.Lerp(context.Speed, context.Speed * _maxSpeedMultiplier, chargePercent);
            context.Size = Mathf.Lerp(context.Size, context.Size * 10f, chargePercent);
            next(context);

            fireRateController.Fired();
            _chargeTracker.Reset();
        }

        public override void Apply(WeaponStatsBuilder stats)
        {
            float minPercent = _minChargeTime / _maxChargeTime;
            float minDamage = Mathf.Lerp(stats.Damage, stats.Damage * _maxDamageMultiplier, minPercent);
            float maxDamage = stats.Damage * _maxDamageMultiplier;

            stats.Damage = minDamage;

            stats.AddExtra("Max Charge Damage", maxDamage);
            stats.AddExtra("Min Charge Damage", minDamage);

            stats.AddExtra("Max Charge Time", _maxChargeTime);
            stats.AddExtra("Min Charge Time", _minChargeTime);
        }
    }
}
