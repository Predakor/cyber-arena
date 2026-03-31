using UnityEngine;

namespace Systems.Guns.HitEffects.Damage
{
    [CreateAssetMenu(menuName = MenuName + "StunEffect")]
    public sealed class StunEffect : HitEffectSO
    {
        public override void Apply(HitInfo hit)
        {
            if (!hit.Target.TryGetComponent<BaseMovement>(out var movement))
            {
                return;
            }

            movement.AllowMovement(false);
        }

        public override void Clear(HitInfo hit)
        {
            if (hit.Target == null)
            {
                return;
            }

            if (!hit.Target.TryGetComponent<BaseMovement>(out var movement))
            {
                return;
            }

            movement.AllowMovement(true);
        }
    }
}
