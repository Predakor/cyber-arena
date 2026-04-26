using UnityEngine;

namespace Systems.Guns.Projectiles.Physics.Rocket
{
    [CreateAssetMenu(menuName = menuPath + "/" + nameof(RocketProjectileConfig))]
    public class RocketProjectileConfig : ProjectileConfigSO
    {
        [SerializeField] private float explosionRadius = 1.0f;
        public float ExplosionRadius => explosionRadius;
    }
}
