using System.Collections.Generic;
using Systems.Guns.HitEffects;
using UnityEngine;

namespace Systems.Guns.Projectiles.Physics
{
    public abstract class ProjectileConfigSO : ScriptableObject, IProjectileConfig
    {
        protected const string menuPath = "Weapons/Projectiles";
        public float size;
        public int damage;
        public float speed;
        public float duration;
        public List<HitEffectSO> effects;

        public float Size => size;

        public int Damage => damage;

        public float Speed => speed;

        public float Duration => duration;

        public IReadOnlyList<IHitEffect> Effects => effects.AsReadOnly();
    }
}
