using System.Collections;
using Systems.Guns.Projectiles;
using Systems.Guns.Projectiles.Physics;
using UnityEngine;

namespace Systems.Guns.Modules.ShootModules
{
    public sealed class BurstFireModule : ShootModuleBase
    {
        [SerializeField]
        Transform _muzzle;

        [SerializeField, Range(0.01f, 5f)]
        float _interclipTime;

        [SerializeField, Range(1, 255)]
        byte _clipSize;

        [SerializeReference]
        ProjectileConfigSO _config;

        public override void Pressed()
        {
            StartCoroutine(BurstRoutine(_config));
        }

        public override void Released() { }

        private IEnumerator BurstRoutine(ProjectileConfigSO config)
        {
            for (int i = 0; i < _clipSize; i++)
            {
                ProjectileFactory.Instance.Create(config).Shoot(_muzzle);
                yield return new WaitForSeconds(_interclipTime);
            }
            fireRateController.Fired();
        }
    }
}
