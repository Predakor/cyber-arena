using System.Collections.Generic;
using Systems.Guns.HitEffects;
using Systems.Guns.Interfaces;
using UnityEngine;

namespace Systems.Guns.Projectiles.Physics
{
    public abstract class ProjectileConfigSO : ScriptableObject, IConfig
    {
        protected const string menuPath = "Weapons/Projectiles";
        public float Size;
        public int Damage;
        public float Speed;
        public List<HitEffectSO> Effects;
    }
}
