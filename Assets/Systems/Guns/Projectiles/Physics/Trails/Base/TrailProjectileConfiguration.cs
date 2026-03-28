using UnityEngine;

namespace Systems.Guns.Projectiles.Physics
{
    [CreateAssetMenu(menuName = menuPath + "/" + nameof(TrailProjectileConfiguration))]
    public sealed class TrailProjectileConfiguration : ProjectileConfigSO
    {
        [Header("Trail")]
        public float trailTime = 0.3f;
        public float startWidth = 0.1f;
        public float endWidth = 0.02f;
        public Gradient colorGradient;
        public Material material;
    }
}
