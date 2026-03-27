using System;
using Systems.Guns.Modules.Shared;

namespace Systems.Guns.Modules.ShootModules
{
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