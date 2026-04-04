using System.Collections.Generic;
using Systems.Guns.HitEffects;
using UnityEngine;

namespace Systems.Guns.Projectiles.Physics
{
    public abstract class ProjectileConfigSO : ScriptableObject, IProjectileConfig
    {
        protected const string menuPath = "Weapons/Projectiles";
        [field: SerializeField] public float Size { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public int AmmoCost { get; private set; }

        [SerializeField] private List<HitEffectSO> _effects;
        public IReadOnlyList<IHitEffect> Effects => _effects.AsReadOnly();
    }
}
