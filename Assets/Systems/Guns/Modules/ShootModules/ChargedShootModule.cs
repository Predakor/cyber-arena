using UnityEngine;

namespace Systems.Guns.Modules.ShootModules {
    public class ChargedShootModule : ShootModuleBase {

        private float _minChargeTime;
        private float _maxChargeTime;

        private ChargeTimer _chargeTracker;

        private bool MinChargeTimeExceeded => _chargeTracker.GetDuration() < _minChargeTime;

        protected override void Awake() {
            base.Awake();
            _chargeTracker = new ChargeTimer();

        }

        public override void Pressed() {
            if (_chargeTracker.State == ChargeState.None) {
                _chargeTracker.Start();
            }
        }

        public override void Released() {
            if (MinChargeTimeExceeded) {
                //do some logic
                //include boostable stats by some modifier related to chargeTime
                // config.FinalDamage *= (ChargeTimeMultiplier * ChargeDuration) 
                fireRateController.Fired();
            }

            _chargeTracker.Reset();
        }
    }
}

internal class ChargeTimer {
    private float _chargeStartTime;
    private float _chargeEndTime;
    private float _maxChargeTime;

    private static float Now => Time.time;
    public ChargeState State { get; private set; } = ChargeState.None;

    public ChargeTimer Start() {
        _chargeStartTime = Now;
        State = ChargeState.Charging;
        return this;
    }

    public ChargeTimer Stop() {
        _chargeEndTime = Now;
        State = ChargeState.Paused;
        return this;
    }

    public ChargeTimer Reset() {
        _chargeEndTime = default;
        _chargeStartTime = default;
        State = ChargeState.None;
        return this;
    }


    public float GetDuration() {
        return _chargeEndTime != 0
            ? _chargeEndTime - _chargeStartTime
            : Now - _chargeStartTime;
    }

    public float GetMaxChargePercentile() {
        if (_maxChargeTime <= 0f) {
            return 1f;
        }

        float chargeTime = GetDuration();
        return Mathf.Clamp01(chargeTime / _maxChargeTime);
    }
}

internal enum ChargeState {
    None,
    Charging,
    Paused, //add actual support
    Full
}