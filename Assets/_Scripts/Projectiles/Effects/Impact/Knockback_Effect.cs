using Systems.Guns.Projectiles;
using UnityEngine;

namespace Systems.Guns.HitEffects.Impact
{
    [CreateAssetMenu(menuName = MenuName + nameof(Knockback_Effect))]
    public sealed class Knockback_Effect : HitEffectSO
    {
        private const int StrengthMultiplier = 10;

        [Range(0.1f, 10f)]
        public float Strength = 1f;

        public override void Apply(HitInfo hit)
        {
            var target = hit.Target;

            if (!target.TryGetComponent(out Rigidbody rb))
            {
                return;
            }

            target.EneableMovement(false);
            Vector3 direction = hit.Point - target.transform.position;
            direction.y = 0f;
            rb.AddForce(StrengthMultiplier * Strength * direction.normalized, ForceMode.Impulse);
        }

        public override void Clear(HitInfo hit)
        {
            hit.Target.EneableMovement(true);
        }
    }

    static class Extentions
    {
        public static void EneableMovement(this GameObject target, bool active)
        {
            if (target.TryGetComponent<BaseMovement>(out var movement))
            {
                movement.enabled = active;
            }
        }
    }
}
