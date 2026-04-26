using UnityEngine;

namespace Systems.Guns.HitEffects.Damage.Explosion
{

    [CreateAssetMenu(menuName = MenuName + nameof(ExplosionEffectSo))]
    public sealed class ExplosionEffectSo : HitEffectSO
    {

        [SerializeField] private float explosionRadius = 1.0f;
        [SerializeField] private float damageFallow = 1.0f;
        [SerializeField] private ParticleSystem _explosionVfx;


        public override void Apply(HitInfo hit)
        {
            var instance = Instantiate(_explosionVfx, hit.Target.transform);
            var main = instance.main;
            main.stopAction = ParticleSystemStopAction.Destroy;
            instance.Play();
        }

        public override void Clear(HitInfo hit)
        {
            //kill self or return to pool
        }

    }

}
