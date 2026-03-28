using Systems.Guns.Modules.Shared;
using UnityEngine;

namespace Systems.Guns.Modules.SpreadModule
{
    [CreateAssetMenu(menuName = MenuPath + "Shotgun")]
    public sealed class ShotgunSpread : SpreadModuleBase
    {
        public override ShotPoint[] GetShotPoints(Transform muzzle)
        {
            var shots = new ShotPoint[pelletCount];
            for (int i = 0; i < pelletCount; i++)
            {
                shots[i] = new ShotPoint(muzzle.position, GetPelletDirection(muzzle.forward));
            }
            return shots;
        }

        private Vector3 GetPelletDirection(Vector3 forward)
        {
            // Uniform disk sampling — sqrt prevents clustering at center
            float angle = Random.value * 360f;
            float radius = Mathf.Sqrt(Random.value) * spreadAngle;

            float yaw = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float pitch = radius * Mathf.Sin(angle * Mathf.Deg2Rad);

            return Quaternion.Euler(pitch, yaw, 0f) * forward;
        }
    }
}