using System;
using Systems.Guns.Modules.Shared;
using Systems.Weapons.Guns.Modules;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Systems.Guns.Modules.SpreadModule
{
    public abstract class SpreadModuleBase : ScriptableObject, IGunModule
    {
        protected const string MenuPath = "Weapons/Spread/";

        [SerializeField, Range(1, 32)]
        protected byte pelletCount = 1;

        [SerializeField, Range(0f, 90f)]
        protected float spreadAngle = 0f;

        [SerializeField, Range(0.01f, 1f)]
        protected float damageMultiplier = 1f;

        public byte PelletCount => pelletCount;
        public float DamageMultiplier => damageMultiplier;

        public abstract ShotPoint[] GetShotPoints(Transform muzzle);

        public void Handle(ShootContext context, Action<ShootContext> next)
        {
            context.ShotPoints = GetShotPoints(context.Muzzle);
            context.Damage *= damageMultiplier;
            next(context);
        }
    }


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
