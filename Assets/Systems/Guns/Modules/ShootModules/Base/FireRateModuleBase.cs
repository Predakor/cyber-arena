using System;
using System.Collections;
using System.Collections.Generic;
using Systems.Guns.Modules.Shared;
using Systems.Guns.Stats;
using Systems.Guns.Utils;
using Systems.Shared;
using Systems.Weapons.Guns.Modules;
using UnityEngine;

namespace Systems.Guns.Modules.ShootModules
{
    public abstract class FireRateModuleBase : ScriptableObject, IGunModule
    {
        protected const string MenuPath = "Weapons/FireRate/";

        protected FireRateController fireRateController;

        [SerializeField]
        protected List<StatModifier> statModifiers = new()
        {
            StatModifier.Flat(StatType.FireRate, 120)
        };

        [field: SerializeField] public virtual string Name { get; protected set; } = "Base Fire Rate Module";

        public virtual FireRateModuleBase Initialize()
        {
            fireRateController = FireRateController.FromRPM(statModifiers[0].Value);
            return this;
        }

        public virtual void Handle(ShootContext context, Action<ShootContext> next)
        {
            switch (context.State)
            {
                case ShootState.Shoot:
                    Pressed(context, next);
                    return;
                case ShootState.Stop:
                    Released(context, next);
                    return;
                default:
                    return;
            }
        }

        public virtual void Apply(WeaponStatsBuilder stats)
        {
            stats.AddModifierList(statModifiers);
        }

        public abstract void Pressed(ShootContext context, Action<ShootContext> next);
        public abstract void Released(ShootContext context, Action<ShootContext> next);

        protected Coroutine StartCoroutine(IEnumerator routine) => CoroutineRunner.Run(routine);
        protected void StopCoroutine(Coroutine coroutine) => CoroutineRunner.Stop(coroutine);
    }
}
