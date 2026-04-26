using UnityEngine;

namespace Systems.Guns.Utils
{
    public sealed class FireRateController
    {
        public float FireRate { get; private set; }
        private float _lastFiretime = Time.time;

        private FireRateController(float fireDelay)
        {
            FireRate = fireDelay;
        }

        public static FireRateController FromRPM(float roundsPerMinute) => new(1f / (roundsPerMinute / 60));

        public static FireRateController FromRoundsPerSecond(float roundsPerSeconds) => new(roundsPerSeconds);

        public void Fired() => _lastFiretime = Time.time;

        public bool IsReadyToFire => Time.time >= _lastFiretime + FireRate;

        public FireRateController UpdateRPM(float roundsPerMinute)
        {
            FireRate = 1f / (roundsPerMinute / 60);
            return this;
        }
    }
}
