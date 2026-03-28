using System;
using Systems.Guns.Modules.Shared;
using Systems.Weapons.Guns.Modules;
using UnityEngine;


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
}
