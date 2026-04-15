using System;
using System.Collections;
using Systems.Guns.Modules.Shared;
using Systems.Shared;
using Systems.Weapons.Guns.Modules;
using UnityEngine;

namespace Systems.Guns.Modules.AmmoModule.Base
{
    public abstract class AmmoModuleBase : ScriptableObject, IGunModule, IAmmoEvents
    {
        protected const string MenuPath = "Weapons/Ammo/";

        [SerializeField][Range(0, 1000)] private int _magazineSize;
        [SerializeField][Range(0, 1000)] private int _currentAmmo;
        [SerializeField][Range(0, 32)] private float _reloadSpeed;

        protected bool _isReloading = false;

        public event Action<int, int> OnAmmoChange;
        public event Action<float> OnReloadStart;
        public event Action<float, bool> OnReloadEnd;

        public virtual string Name => "Base Ammo Module";

        public virtual int CurrentAmmo
        {
            get => _currentAmmo;
            protected set
            {
                if (_currentAmmo != value)
                {
                    _currentAmmo = value;
                    OnAmmoChange?.Invoke(value, MagazineSize);
                }
            }
        }

        public virtual int MagazineSize
        {
            get => _magazineSize;
            protected set { _magazineSize = value; }
        }

        public virtual float ReloadSpeed
        {
            get => _reloadSpeed;
            protected set { _reloadSpeed = value; }
        }

        protected Coroutine StartCoroutine(IEnumerator routine) => CoroutineRunner.Run(routine);
        protected void StopCoroutine(Coroutine coroutine) => CoroutineRunner.Stop(coroutine);

        public abstract IEnumerator Reload();
        public abstract void DecreaseAmmo(int amount = 1);
        public abstract void IncreaseAmmo(int amount = 1);

        protected virtual void StartReload()
        {
            if (_isReloading)
            {
                return;
            }
            _isReloading = true;
            OnReloadStart?.Invoke(_reloadSpeed);
        }

        protected virtual void RefillAmmo()
        {
            CurrentAmmo = MagazineSize;
        }

        protected virtual void FinishReload()
        {
            _isReloading = false;
            RefillAmmo();
            OnReloadEnd?.Invoke(_reloadSpeed, false);
        }

        protected virtual void OnEnable()
        {
            _isReloading = false;
        }

        public void Handle(ShootContext context, Action<ShootContext> next)
        {
            if (_isReloading)
            {
                return;
            }

            if (context.AmmoCost > CurrentAmmo)
            {
                StartReload();
                return;
            }

            DecreaseAmmo(context.AmmoCost);
            next(context);
        }

        public virtual void Apply(WeaponStatsBuilder stats)
        {
            stats.AddExtra("Magazine", MagazineSize);
            stats.AddExtra("Reload", ReloadSpeed);
        }
    }
}