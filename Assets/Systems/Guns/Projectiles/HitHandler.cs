using Systems.Guns.HitEffect;
using Systems.Guns.HitEffects;
using UnityEngine;

namespace Systems.Guns.Projectiles {
    public static class HitHandler {
        public static void Handle(GameObject target, IProjectileConfig config) {
            var flags = HitFlag.Impact;
            var hitInfo = new HitInfo() {
                Target = target,
                Normal = target.transform.position,
                Point = target.transform.position,
            };

            if (target.TryGetComponent<IDamageable>(out var health)) {
                health.Damage(config.Damage);
            }

            flags |= health != null ? HitFlag.Damageable : HitFlag.Surface;
            foreach (IHitEffect effect in config.Effects) {
                if ((flags & effect.Trigger) != 0) {
                    EffectRunner.Instance.StartEffect(effect, hitInfo);
                }
            }
        }
    }

    [System.Flags]
    public enum HitFlag {
        None = 0,
        Impact = 1 << 0,
        Damageable = 1 << 1,
        Surface = 1 << 2,
        Critical = 1 << 3,
        Weakpoint = 1 << 4,
    }
}
