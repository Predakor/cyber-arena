using System;
using Systems.Guns.Modules.Shared;
using UnityEngine;

namespace Systems.Guns.Modules.ShootModules
{
    [CreateAssetMenu(menuName = MenuPath + "Semi Auto")]
    public sealed class SemiAutoFireModule : FireRateModuleBase
    {
        public override void Pressed(ShootContext context, Action<ShootContext> next)
        {
            if (!fireRateController.IsReadyToFire)
            {
                return;
            }

            next(context);
            fireRateController.Fired();
        }

        public override void Released(ShootContext context, Action<ShootContext> next) { }
    }
}