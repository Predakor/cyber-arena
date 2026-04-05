using System;
using Systems.Guns.Interfaces;
using Systems.Guns.Modules;
using Systems.Guns.Modules.AmmoModule.Base;
using Systems.Guns.Modules.ProjectileModule;
using Systems.Guns.Modules.Shared;
using Systems.Guns.Modules.ShootModules;
using Systems.Guns.Modules.SpreadModule;
using Systems.Weapons.Guns.Modules;
using UnityEngine;

namespace Systems.Guns
{
    [Serializable]
    public sealed class Configuration : IConfig<Gun>
    {
        public FireRateModuleBase fireRateModule;
        public AmmoModuleBase ammoModule;
        public ProjectileModuleBase projectileModule;
        public SpreadModuleBase spreadModule;
    }

    public sealed class Gun : MonoBehaviour, IGun
    {
        [SerializeField] private Transform _muzzle;
        [SerializeField] private Configuration _config;
        [SerializeField] private WeaponStats _stats;

        private ShootPipeline _pipeline;
        private AmmoModuleBase _ammoModule;

        public event Action<IWeaponStats> StatsChanged;

        public IAmmoEvents AmmoEvents => _ammoModule;
        public IWeaponStats Stats
        {
            private set
            {
                _stats = value as WeaponStats;
                StatsChanged?.Invoke(_stats);
            }
            get => _stats;
        }

        private void Awake()
        {
            Configure();
        }

        public void Use(bool isPressed) => ShootHandler(isPressed ? ShootState.Shoot : ShootState.Stop);

        public TWeapon Configure<TWeapon>(IConfig<TWeapon> config) where TWeapon : IWeapon
        {
            if (config is not Configuration)
            {
                Debug.LogError($"Incorrect configuration type passed Expected: {typeof(Configuration)} but got: {config.GetType()}");
            }

            _config = config as Configuration;

            Configure();

            return (TWeapon)(IWeapon)this;
        }

        private void Configure()
        {
            _ammoModule = Instantiate(_config.ammoModule);

            _pipeline = new ShootPipeline(
                _config.fireRateModule,
                _config.spreadModule,
                _ammoModule,
                new ProjectileSpawnModule()
            );

            var stats = WeaponStatsBuilder
                .FromProjectileBase(_config.projectileModule)
                .ApplyModuleModifiers(_config.fireRateModule)
                .ApplyModuleModifiers(_config.spreadModule)
                .ApplyModuleModifiers(_ammoModule)
                .Build();

            Stats = stats;

        }

        private void ShootHandler(ShootState state)
        {
            var context = _config.projectileModule.GetShootContext();
            context.State = state;
            context.Muzzle = _muzzle;
            _pipeline.Execute(context);
        }
    }
}
