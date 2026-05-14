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
    public enum GunState
    {
        None = 0,
        Idle = 1,
        Aiming = 2,
        Shooting = 3,
        Reloading = 4,
    }

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

        public ICurrentGunState CurrentState => new InitialState(_modules.AmmoModule.CurrentAmmo, GunState.Idle);

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
            var fireRateModule = Instantiate(_config.fireRateModule).Initialize();
            var spreadModule = Instantiate(_config.spreadModule);
            var ammoModule = Instantiate(_config.ammoModule);

            _modules = new ModuleContainer
            {
                ProjectileModule = _config.projectileModule,
                FireRateModule = fireRateModule,
                SpreadModule = spreadModule,
                AmmoModule = ammoModule,
            };

            _pipeline = new ShootPipeline(
                fireRateModule,
                spreadModule,
                ammoModule,
                new ProjectileSpawnModule()
            );

            var stats = WeaponStatsBuilder
                .FromProjectileBase(_config.projectileModule)
                .ApplyModuleModifiers(fireRateModule)
                .ApplyModuleModifiers(spreadModule)
                .ApplyModuleModifiers(ammoModule)
                .Build();

            Stats = stats;
            ModulesChanged?.Invoke(_modules);
        }

        private void ShootHandler(ShootState state)
        {
            var context = _config.projectileModule
                .GetShootContext()
                .SetShootState(state)
                .SetMuzzle(_muzzle)
                .ApplyStats(Stats);

            _pipeline.Execute(context);
        }
    }

    internal sealed record ModuleContainer : IWeaponModules
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

    internal sealed record InitialState(int CurrentAmmo, GunState State) : ICurrentGunState;
}
