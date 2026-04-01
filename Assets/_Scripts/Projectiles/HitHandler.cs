using Systems.Guns.HitEffect;
using Systems.Guns.HitEffects;

namespace Scripts.Projectiles
{
    public static class HitHandler
    {
        public static void Handle(HitInfo info)
        {
            var flags = HitFlag.Impact;

            if (info.Target.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.Damage((int)info.Damage);
                flags |= HitFlag.Damageable;
            }
            else
            {
                flags |= HitFlag.Surface;
            }

            foreach (var effect in info.Effects)
            {
                if ((flags & effect.Trigger) != 0)
                {
                    EffectRunner.Instance.StartEffect(effect, info);
                }
            }
        }
    }
}
