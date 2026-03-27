using System.Collections.Generic;
using Systems.Guns.HitEffect;
using Systems.Guns.HitEffects;
using UnityEngine;

namespace Systems.Guns.Projectiles.Utils
{
    public static class HitHandler
    {
        public static void Handle(GameObject target, IProjectileConfig config, Transform projectile)
        {
            Handle(target, config.Damage, config.Effects, projectile);
        }

        public static void Handle(GameObject target, float damage, IReadOnlyList<IHitEffect> effects, Transform projectile)
        {
            var hitPoint = target.TryGetComponent<Collider>(out var col)
                ? col.ClosestPoint(projectile.position)
                : target.transform.position;

            var hitInfo = new HitInfo()
            {
                Target = target,
                Normal = (target.transform.position - projectile.position).normalized,
                Point = hitPoint,
            };

            var flags = HitFlag.Impact;
            if (target.TryGetComponent<IDamageable>(out var health))
            {
                health.Damage((int)damage);
                flags |= HitFlag.Damageable;
            }
            else
            {
                flags |= HitFlag.Surface;
            }

            foreach (var effect in effects)
            {
                if ((flags & effect.Trigger) != 0)
                {
                    EffectRunner.Instance.StartEffect(effect, hitInfo);
                }
            }
        }
    }

    [System.Flags]
    public enum HitFlag
    {
        None = 0,
        Impact = 1 << 0,
        Damageable = 1 << 1,
        Surface = 1 << 2,
        Critical = 1 << 3,
        Weakpoint = 1 << 4,
    }
}
