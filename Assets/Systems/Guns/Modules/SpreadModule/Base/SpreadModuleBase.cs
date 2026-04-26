using System;
using System.Collections.Generic;
using Systems.Guns.Modules.Shared;
using Systems.Guns.Stats;
using Systems.Weapons.Guns.Modules;
using UnityEngine;

namespace Systems.Guns.Modules.SpreadModule
{
    public abstract class SpreadModuleBase : ScriptableObject, IGunModule
    {
        protected const string MenuPath = "Weapons/Spread/";

        [SerializeField]
        protected List<StatModifier> statModifiers = new()
        {
            StatModifier.Flat(StatType.Spread, 0.15f)
        };

        [SerializeField, Range(1, 32)] protected byte pelletCount = 1;
        [SerializeField, Range(0f, 90f)] protected float spreadAngle = 0f;
        [SerializeField, Range(0.01f, 1f)] protected float damageMultiplier = 1f;

        [field: SerializeField] public virtual string Name { get; protected set; } = "Base Spread Module";

        public abstract ShotPoint[] GetShotPoints(Transform muzzle);

        public void Handle(ShootContext context, Action<ShootContext> next)
        {
            context.ShotPoints = GetShotPoints(context.Muzzle);
            next(context);
        }

        public void Apply(WeaponStatsBuilder stats)
        {
            stats.AddModifierList(statModifiers);
        }
    }
}
