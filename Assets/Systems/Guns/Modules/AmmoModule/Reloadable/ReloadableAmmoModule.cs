using System.Collections;
using Systems.Guns.Modules.AmmoModule.Base;
using UnityEngine;

namespace Systems.Guns.Modules.AmmoModule
{
    [CreateAssetMenu(menuName = MenuPath + nameof(ReloadableAmmoModule))]
    public sealed class ReloadableAmmoModule : AmmoModuleBase
    {
        [SerializeField] private bool _unlimitedAmmo = true;
        [SerializeField][Range(0, 1000)] private int _reserveAmmo;
        [SerializeField][Range(0, 1000)] private int _maxReserve;

        public int ReserveAmmo => _reserveAmmo;

        protected override void StartReload()
        {
            if (_isReloading)
            {
                return;
            }

            if (!_unlimitedAmmo && _reserveAmmo <= 0)
            {
                //shoult emmit some event or something 
                Debug.Log("Ammo reserve is empty", this);
                return;
            }

            base.StartReload();
            StartCoroutine(Reload());
        }

        protected override void RefillAmmo()
        {
            var needed = MagazineSize - CurrentAmmo;
            var refill = !_unlimitedAmmo
                ? Mathf.Min(needed, _reserveAmmo)
                : needed;
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

        public override void Apply(WeaponStatsBuilder stats)
        {
            base.Apply(stats);
            stats.AddExtra("Reserve", _maxReserve);
        }
    }
}