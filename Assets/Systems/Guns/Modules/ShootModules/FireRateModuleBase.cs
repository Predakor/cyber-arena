using System;
using Systems.Guns.Modules.Shared;
using Systems.Guns.Utils;
using Systems.Weapons.Guns.Modules;
using UnityEngine;

namespace Systems.Guns.Modules.ShootModules
{

    public abstract class FireRateModuleBase : MonoBehaviour, IGunModule
    {
        protected FireRateController fireRateController;

        [SerializeField] protected short roundPerMinute;


        protected virtual void Awake()
        {
            fireRateController = FireRateController.FromRPM(roundPerMinute);
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

        public abstract void Pressed(ShootContext context, Action<ShootContext> next);
        public abstract void Released(ShootContext context, Action<ShootContext> next);

    }
}
