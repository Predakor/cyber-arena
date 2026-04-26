using Systems.Guns.Modules.Shared;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Systems.Guns.Modules.SpreadModule
{
    [CreateAssetMenu(menuName = MenuPath + "Random Cone")]
    public sealed class RandomConeSpread : SpreadModuleBase
    {
        public override ShotPoint[] GetShotPoints(Transform muzzle)
        {
            var shots = new ShotPoint[pelletCount];
            for (int i = 0; i < pelletCount; i++)
            {
                float yaw = Random.Range(-spreadAngle, spreadAngle);
                float pitch = Random.Range(-spreadAngle, spreadAngle);
                Vector3 dir = Quaternion.Euler(pitch, yaw, 0f) * muzzle.forward;
                shots[i] = new ShotPoint(muzzle.position, dir);
            }
            return shots;
        }
    }
}
