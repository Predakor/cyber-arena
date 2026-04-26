using System;
using Systems.Guns.Modules.Shared;
using UnityEngine;

namespace Systems.Guns.Modules.ShootModules
{
    [CreateAssetMenu(menuName = MenuPath + "Charged")]
    public sealed class ChargedShootModule : FireRateModuleBase
    {
        [SerializeField][Range(0.01f, 5f)] private float _minChargeTime;
        [SerializeField][Range(0.01f, 5f)] private float _maxChargeTime;

        [SerializeField] private float _maxDamageMultiplier = 3f;
        [SerializeField] private float _maxSpeedMultiplier = 2f;

        [SerializeField] private ParticleSystem _chargeEffect;

        private ChargeTimer _chargeTracker;
        private ParticleSystem _particles;

        private bool MinChargeTimeExceeded => _chargeTracker.GetDuration() > _minChargeTime;

        public override FireRateModuleBase Initialize()
        {
            base.Initialize();
            _chargeTracker = new ChargeTimer(_maxChargeTime);
            return this;
        }

        public override void Pressed(ShootContext context, Action<ShootContext> next)
        {
            if (_chargeTracker.State != ChargeState.None)
            {
                return;
            }

            _chargeTracker.Start();
            _particles = Instantiate(_chargeEffect, context.Muzzle);
            var main = _particles.main;
            main.duration = _maxChargeTime / 2;
            _particles.Play();
        }

        public override void Released(ShootContext context, Action<ShootContext> next)
        {
            _chargeTracker.Stop();
            _particles.Clear();
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
