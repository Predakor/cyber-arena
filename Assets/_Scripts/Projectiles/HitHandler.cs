using System.Collections.Generic;
using Systems.Guns.HitEffect;
using Systems.Guns.HitEffects;
using Systems.Guns.Projectiles;

namespace Scripts.Projectiles
{
    public static class HitHandler
    {
        public static void Register(IProjectile projectile, IProjectileConfig config)
        {
            projectile.OnHit += hit => Handle(hit, config.Damage, config.Effects);
        }

        private static void Handle(HitInfo hit, float damage, IReadOnlyList<IHitEffect> effects)
        {
            var flags = HitFlag.Impact;

            if (hit.Target.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.Damage((int)damage);
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
                    EffectRunner.Instance.StartEffect(effect, hit);
                }
            }
        }
    }
}
