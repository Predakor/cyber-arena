using Systems.Guns.Projectiles;
using UnityEngine;

namespace Systems.Guns.HitEffects {
    public abstract class HitEffectSO : ScriptableObject, IHitEffect {
        protected const string MenuName = "Weapons/Effects/";
        public float Duration { get; protected set; } = 1f;
        public HitFlag Trigger { get; } = HitFlag.None;

        public abstract void Apply(HitInfo hit);
        public abstract void Clear(HitInfo target);
    }
}
