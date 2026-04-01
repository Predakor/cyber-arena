using System.Collections;
using Systems.Guns.Modules.AmmoModule.Base;
using UnityEngine;

namespace Systems.Guns.Modules.AmmoModule
{
    public sealed class ReloadableAmmoModule : AmmoModuleBase
    {
        [SerializeField][Range(0, 1000)] private int _reserveAmmo;
        [SerializeField][Range(0, 1000)] private int _maxReserve;

        public int ReserveAmmo => _reserveAmmo;

        protected override void StartReload()
        {
            if (_isReloading || _reserveAmmo <= 0)
            {
                return;
            }
            base.StartReload();
            StartCoroutine(Reload());
        }

        protected override void RefillAmmo()
        {
            var needed = MagazineSize - CurrentAmmo;
            var refill = Mathf.Min(needed, _reserveAmmo);
            _reserveAmmo -= refill;
            CurrentAmmo += refill;
        }

        public override IEnumerator Reload()
        {
            yield return new WaitForSeconds(ReloadSpeed);
            FinishReload();
        }

        public override void DecreaseAmmo(int amount = 1)
        {
            CurrentAmmo -= amount;
        }

        public override void IncreaseAmmo(int amount = 1)
        {
            _reserveAmmo = Mathf.Min(_reserveAmmo + amount, _maxReserve);
        }
    }
}