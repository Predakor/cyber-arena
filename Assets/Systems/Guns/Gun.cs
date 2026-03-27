using System;
using Systems.Channels;
using Systems.Channels.Inputs;
using Systems.Guns.Interfaces;
using Systems.Guns.Modules;
using Systems.Guns.Modules.ProjectileModule;
using Systems.Guns.Modules.Shared;
using Systems.Guns.Modules.ShootModules;
using Systems.Guns.Modules.SpreadModule;
using UnityEngine;

namespace Systems.Guns
{
    [Serializable]
    public sealed class Configuration : IConfig<Gun>
    {
        public FireRateModuleBase fireRateModule;
        public AmmoModule ammoModule;
        public SpreadModuleBase spreadModule;
        public ProjectileModuleBase projectileModule;
    }

    public sealed class Gun : MonoBehaviour, IGun
    {
        [SerializeField] private InputsChannel _channel;
        [SerializeField] private Transform _muzzle;
        [SerializeField] private Configuration _config;

        private ShootPipeline _pipeline;

        private void Awake()
        {
            _pipeline = new ShootPipeline(
                _config.fireRateModule,
                _config.ammoModule,
                _config.spreadModule,
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

        private void OnEnable()
        {
            _channel.Subscribe<InputEvents.Shoot>(InputHandler);
        }

        private void OnDisable()
        {
            _channel.Unsubscribe<InputEvents.Shoot>(InputHandler);
        }

        public TWeapon Configure<TWeapon>(IConfig<TWeapon> config) where TWeapon : IWeapon
        {
            throw new NotImplementedException();
        }
    }

}
