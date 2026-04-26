using System;
using System.Collections;
using System.Collections.Generic;
using Systems.Guns.Modules.Shared;
using Systems.Guns.Stats;
using Systems.Shared;
using Systems.Weapons.Guns.Modules;
using UnityEngine;

namespace Systems.Guns.Modules.AmmoModule.Base
{
    public abstract class AmmoModuleBase : ScriptableObject, IGunModule, IAmmoEvents
    {
        protected const string MenuPath = "Weapons/Ammo/";

        [SerializeField]
        protected List<StatModifier> statModifiers = new()
        {
            StatModifier.Flat(StatType.MagazineSize, 30),
            StatModifier.Flat(StatType.ReloadSpeed, 1.5f)
        };

        protected bool _isReloading = false;
        private int _currentAmmo;

        public event Action<int, int> OnAmmoChange;
        public event Action<float> OnReloadStart;
        public event Action<float, bool> OnReloadEnd;

        [field: SerializeField] public virtual string Name { get; protected set; } = "Base Ammo Module";

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

        // Pull from modifiers instead of standalone fields
        public virtual int MagazineSize => (int)statModifiers.Find(m => m.Stat == StatType.MagazineSize).Value;
        public virtual float ReloadSpeed => statModifiers.Find(m => m.Stat == StatType.ReloadSpeed).Value;

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
            OnReloadStart?.Invoke(ReloadSpeed);
        }

        protected virtual void RefillAmmo()
        {
            CurrentAmmo = MagazineSize;
        }

        protected virtual void FinishReload()
        {
            _isReloading = false;
            RefillAmmo();
            OnReloadEnd?.Invoke(ReloadSpeed, false);
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
            stats.AddModifierList(statModifiers);
        }
    }
}