using Systems.Guns.Projectiles;
using UnityEngine;

namespace Systems.Guns.HitEffects.Damage.Explosion
{

    [CreateAssetMenu(menuName = MenuName + nameof(ExplosionEffectSo))]
    public sealed class ExplosionEffectSo : HitEffectSO
    {

        [SerializeField] float explosionRadius = 1.0f;
        [SerializeField] float damageFallow = 1.0f;
        [SerializeField] ParticleSystem _explosionVfx;


        public override void Apply(HitInfo hit)
        {
            Instantiate(_explosionVfx, hit.Target.transform).Play();
        }

        public override void Clear(HitInfo hit)
        {

            //kill self or return to pool
        }

    }

}
