using Systems.Guns.Projectiles;
using Systems.Guns.Projectiles.Utils;

namespace Systems.Guns.HitEffects
{
    public interface IEffect { }

    public interface IHitEffect : IEffect
    {
        HitFlag Trigger { get; }
        float Duration { get; }
        void Apply(HitInfo target);
        void Clear(HitInfo target);
    }
}
