using System;
using Systems.Guns.Assets.Systems.Guns.Interfaces;
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
        public ProjectileModuleBase projectileModule;
        public FireRateModuleBase fireRateModule;
        public AmmoModuleBase ammoModule;
        public SpreadModuleBase spreadModule;
    }

    public sealed class Gun : MonoBehaviour, IGun
    {
        [SerializeField] private Transform _muzzle;
        [SerializeField] private Configuration _config;
        [SerializeField] private WeaponStats _stats;

        private ShootPipeline _pipeline;
        private ModuleContainer _modules;


        public event Action<IWeaponStats> StatsChanged;
        public event Action<IWeaponModules> ModulesChanged;

        public IAmmoEvents AmmoEvents => _modules.AmmoModule;
        public IWeaponStats Stats
        {
            private set
            {
                _stats = value as WeaponStats;
                StatsChanged?.Invoke(_stats);
            }
            get => _stats;
        }

        public IWeaponModules Modules => _modules;

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
            _modules = new ModuleContainer
            {
                FireRateModule = _config.fireRateModule,
                ProjectileModule = _config.projectileModule,
                SpreadModule = Instantiate(_config.spreadModule),
                AmmoModule = Instantiate(_config.ammoModule),
            };

            _pipeline = new ShootPipeline(
                _config.fireRateModule,
                _config.spreadModule,
                _modules.AmmoModule,
                new ProjectileSpawnModule()
            );

            var stats = WeaponStatsBuilder
                .FromProjectileBase(_config.projectileModule)
                .ApplyModuleModifiers(_config.fireRateModule)
                .ApplyModuleModifiers(_config.spreadModule)
                .ApplyModuleModifiers(_modules.AmmoModule)
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

    internal sealed class ModuleContainer : IWeaponModules
    {
        public AmmoModuleBase AmmoModule { get; internal set; }
        public FireRateModuleBase FireRateModule { get; internal set; }
        public SpreadModuleBase SpreadModule { get; internal set; }
        public ProjectileModuleBase ProjectileModule { get; internal set; }


        IGunModule IWeaponModules.FireRateModule => FireRateModule;
        IGunModule IWeaponModules.AmmoModule => AmmoModule;
        IGunModule IWeaponModules.SpreadModule => SpreadModule;
        IGunModule IWeaponModules.ProjectileModule => ProjectileModule;
    }
}
