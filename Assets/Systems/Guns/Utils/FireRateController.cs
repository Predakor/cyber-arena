using UnityEngine;

namespace Systems.Guns.Utils {
    public sealed class FireRateController {
        private float _fireDelay;
        private float _lastFiretime = Time.time;

        private FireRateController(float fireDelay) {
            _fireDelay = fireDelay;
        }

        public static FireRateController FromRPM(float roundsPerMinute) =>
            new(1f / (roundsPerMinute / 60));

        public static FireRateController FromRoundsPerSecond(float roundsPerSeconds) =>
            new(roundsPerSeconds);

        public void Fired() => _lastFiretime = Time.time;

        public bool IsReadyToFire => Time.time >= _lastFiretime + _fireDelay;

        public FireRateController UpdateRPM(float roundsPerMinute) {
            _fireDelay = 1f / (roundsPerMinute / 60);
            return this;
        }
    }
}
