using Systems.Guns.Projectiles;
using UnityEngine;

namespace Systems.Guns.HitEffects
{
    public abstract class HitEffectSO : ScriptableObject, IHitEffect
    {
        protected const string MenuName = "Weapons/Effects/";
        [field: SerializeField]
        public float Duration { get; protected set; } = 1f;

        [SerializeField]
        protected HitFlag trigger = HitFlag.None;

        public HitFlag Trigger => trigger;

        public abstract void Apply(HitInfo hit);
        public abstract void Clear(HitInfo hit);
    }
}
