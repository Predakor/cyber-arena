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

        [TypedDerivedSOSelector] public SpreadModuleBase spreadModule;
    }

    public sealed class Gun : MonoBehaviour, IGun
    {
        [SerializeField] private Transform _muzzle;
        [SerializeField] private Configuration _config;

        private ShootPipeline _pipeline;

        private void Awake()
        {
            _pipeline = new ShootPipeline(
                _config.fireRateModule,
                _config.spreadModule,
                _config.ammoModule,
                new ProjectileSpawnModule()
            );
        }

        public IAmmoEvents AmmoEvents => _config.ammoModule;

        public void Use(bool isPressed) => ShootHandler(isPressed ? ShootState.Shoot : ShootState.Stop);

        public TWeapon Configure<TWeapon>(IConfig<TWeapon> config) where TWeapon : IWeapon
        {
            throw new System.NotImplementedException();
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
