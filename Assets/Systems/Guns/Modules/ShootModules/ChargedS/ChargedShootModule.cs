using System;
using Systems.Guns.Modules.Shared;
using Systems.Shared.Runners;
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
        [SerializeField] private ParticleSystem _releaseEffect;


        private ChargeTimer _chargeTracker;
        private VfxEntity _chargeParticles;

        private readonly VfxEffectOptions effectOptions = new() { StickToParent = true };

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
            _chargeParticles = VfxRunner.CreateEffect(_chargeEffect, context.Muzzle, effectOptions);
        }

        public override void Released(ShootContext context, Action<ShootContext> next)
        {
            _chargeTracker.Stop();
            _chargeEffect.Stop();

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
            _chargeParticles.Stop();
            VfxRunner.CreateEffect(_releaseEffect, context.Muzzle, effectOptions);
            _chargeTracker?.Reset();
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
