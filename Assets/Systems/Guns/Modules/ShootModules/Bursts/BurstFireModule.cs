using System;
using System.Collections;
using Systems.Guns.Modules.Shared;
using UnityEngine;

namespace Systems.Guns.Modules.ShootModules
{
    [CreateAssetMenu(menuName = MenuPath + "Burst Fire")]
    public sealed class BurstFireModule : FireRateModuleBase
    {
        [SerializeField, Range(0.01f, 5f)] private float _interclipTime;
        [SerializeField, Range(1, 32)] private byte _clipSize;

        public override void Apply(WeaponStatsBuilder stats)
        {
            base.Apply(stats);
            stats.AddExtra("Burst Size", _clipSize);
            stats.AddExtra("Burst Delay", _interclipTime);
        }

        public override void Pressed(ShootContext context, Action<ShootContext> next)
        {
            StartCoroutine(BurstRoutine(context, next));
        }

        public override void Released(ShootContext context, Action<ShootContext> next) { }

        private IEnumerator BurstRoutine(ShootContext context, Action<ShootContext> next)
        {
            for (int i = 0; i < _clipSize; i++)
            {
                next(context.Clone());
                yield return new WaitForSeconds(_interclipTime);
            }
            fireRateController.Fired();
        }
    }
}
