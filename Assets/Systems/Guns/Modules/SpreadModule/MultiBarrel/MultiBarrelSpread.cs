using Systems.Guns.Modules.Shared;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Systems.Guns.Modules.SpreadModule
{
    [CreateAssetMenu(menuName = MenuPath + "MultiBarrel")]
    public sealed class MultiBarrelSpread : SpreadModuleBase
    {
        [SerializeField] private float barrelSpacing = 0.1f;

        public override ShotPoint[] GetShotPoints(Transform muzzle)
        {
            int pelletsPerBarrel = Mathf.Max(1, pelletCount / 2);
            var shots = new ShotPoint[pelletsPerBarrel * 2];

            Vector3 left = muzzle.position - (muzzle.right * (barrelSpacing * 0.5f));
            Vector3 right = muzzle.position + (muzzle.right * (barrelSpacing * 0.5f));

            int i = 0;
            for (int p = 0; p < pelletsPerBarrel; p++)
            {
                shots[i++] = new ShotPoint(left, Spread(muzzle.forward));
                shots[i++] = new ShotPoint(right, Spread(muzzle.forward));
            }
            return shots;
        }

        private Vector3 Spread(Vector3 forward)
        {
            float yaw = Random.Range(-spreadAngle, spreadAngle);
            float pitch = Random.Range(-spreadAngle, spreadAngle);
            return Quaternion.Euler(pitch, yaw, 0f) * forward;
        }
    }
}
