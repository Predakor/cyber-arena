using System;
using Systems.Channels;
using Systems.Channels.Inputs;
using Systems.Channels.Weapons;
using Systems.Guns.Interfaces;
using Systems.Guns.Modules;
using Systems.Guns.Modules.AmmoModule.Base;
using Systems.Guns.Modules.ProjectileModule;
using Systems.Guns.Modules.Shared;
using Systems.Guns.Modules.ShootModules;
using Systems.Guns.Modules.SpreadModule;
using UnityEngine;

namespace Systems.Guns
{
    public sealed class DerivedSoSelectorAttribute : PropertyAttribute
    {
        public readonly Type BaseType;
        public DerivedSoSelectorAttribute(Type baseType)
        {
            BaseType = baseType;
        }
    }
    public sealed class TypedSOSelectorAttribute : PropertyAttribute { };


    [Serializable]
    public sealed class Configuration : IConfig<Gun>
    {
        public FireRateModuleBase fireRateModule;
        public AmmoModuleBase ammoModule;
        public ProjectileModuleBase projectileModule;

        [DerivedSoSelector(typeof(SpreadModuleBase))] public SpreadModuleBase spreadModule;
    }

    public sealed class Gun : MonoBehaviour, IGun
    {
        [SerializeField] private InputsChannel _channel;
        [SerializeField] private Transform _muzzle;
        [SerializeField] private Configuration _config;
        [SerializeField] private WeaponChannel _weaponChannel;

        private ShootPipeline _pipeline;

        [Obsolete]
        private void Awake()
        {
            _pipeline = new ShootPipeline(
                _config.fireRateModule,
                _config.spreadModule,
                _config.ammoModule,
                new ProjectileSpawnModule()
            );
        }

        public void Use() => ShootHandler(ShootState.Shoot);
        public void ShootHandler(ShootState state)
        {
            var context = _config.projectileModule.GetShootContext();
            context.State = state;
            context.Muzzle = _muzzle;
            _pipeline.Execute(context);
        }

        private void InputHandler(InputEvents.Shoot shootEvent)
        {
            var state = shootEvent.IsPressed ? ShootState.Shoot : ShootState.Stop;
            ShootHandler(state);
        }

        [Obsolete]
        private void OnEnable()
        {
            _channel.Subscribe<InputEvents.Shoot>(InputHandler);
            _config.ammoModule.OnAmmoChange += _weaponChannel.RaiseAmmoChanged;
            _config.ammoModule.OnReloadStart += _weaponChannel.RaiseReloadStarted;
            _config.ammoModule.OnReloadEnd += _weaponChannel.RaiseReloadFinished;
        }

        [Obsolete]
        private void OnDisable()
        {
            _channel.Unsubscribe<InputEvents.Shoot>(InputHandler);
            _config.ammoModule.OnAmmoChange -= _weaponChannel.RaiseAmmoChanged;
            _config.ammoModule.OnReloadStart -= _weaponChannel.RaiseReloadStarted;
            _config.ammoModule.OnReloadEnd -= _weaponChannel.RaiseReloadFinished;
        }

        public TWeapon Configure<TWeapon>(IConfig<TWeapon> config) where TWeapon : IWeapon
        {
            throw new NotImplementedException();
        }
    }

}
