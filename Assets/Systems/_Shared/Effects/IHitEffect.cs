
using UnityEngine;

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


    public readonly struct HitInfo
    {
        public GameObject Target { get; init; }
        public Vector3 Point { get; init; }
        public Vector3 Normal { get; init; }
    };


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
