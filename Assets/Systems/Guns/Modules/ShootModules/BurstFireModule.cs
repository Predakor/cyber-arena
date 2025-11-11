using System.Collections;
using UnityEngine;

namespace Systems.Guns.Modules.ShootModules {
    public sealed class BurstFireModule : ShootModuleBase {

        [SerializeField] Transform _muzzle;
        [SerializeField] float _interclipTime;
        [SerializeField] byte _clipSize;
        [SerializeField] Projectile _projectile;

        public override void Pressed() {
            StartCoroutine(BurstRoutine(_projectile));
        }

        public override void Released() {

        }

        public void Shoot(Projectile projectile) {
            StartCoroutine(BurstRoutine(projectile));
        }

        private IEnumerator BurstRoutine(Projectile projectile) {
            for (int i = 0; i < _clipSize; i++) {
                var bulletInstance = Object.Instantiate(projectile, _muzzle.position, _muzzle.rotation)
                    .Init(p => Object.Destroy(p.gameObject), 50);

                bulletInstance.OnDamageableHit += d => d.Damage(50);

                bulletInstance.Fire();

                yield return new WaitForSeconds(_interclipTime);
            }
            fireRateController.Fired();
        }
    }
}
