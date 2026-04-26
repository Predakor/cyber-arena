using System;
using System.Collections;
using Systems.Guns.Modules.Shared;
using UnityEngine;

namespace Systems.Guns.Modules.ShootModules.Automatics
{
    [CreateAssetMenu(menuName = MenuPath + "Automatic Fire")]
    public sealed class AutomaticFireModule : FireRateModuleBase
    {
        private Coroutine _fireCoroutine;
        private bool _isHoldingTrigger;

        public override void Pressed(ShootContext context, Action<ShootContext> next)
        {
            if (_isHoldingTrigger)
            {
                return;
            }

            _isHoldingTrigger = true;
            _fireCoroutine = StartCoroutine(FireContinuously(context, next));
        }

        public override void Released(ShootContext context, Action<ShootContext> next)
        {
            if (!_isHoldingTrigger)
            {
                return;
            }

            _isHoldingTrigger = false;
            StopCoroutine(_fireCoroutine);
        }

        private IEnumerator FireContinuously(ShootContext context, Action<ShootContext> next)
        {
            while (_isHoldingTrigger)
            {
                next(context);
                yield return new WaitForSeconds(fireRateController.FireRate);
            }
        }
    }
}
