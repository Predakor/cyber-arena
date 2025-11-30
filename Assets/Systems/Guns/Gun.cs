using System;
using Systems.Channels;
using Systems.Channels.Inputs;
using Systems.Guns.Interfaces;
using Systems.Guns.Modules.ShootModules;
using UnityEngine;

namespace Systems.Guns
{
    public sealed class Gun : MonoBehaviour, IGun
    {
        [SerializeField]
        private InputsChannel _channel;

        [SerializeField]
        private Configuration _config;

        public void Use()
        {
            throw new System.NotImplementedException();
        }

        private void OnShoot(InputEvents.Shoot shootEvent)
        {
            if (shootEvent.IsPressed)
            {
                _config.shootModule.Pressed();
            }
            else
            {
                _config.shootModule.Released();
            }
        }

        private void OnEnable()
        {
            _channel.Subscribe<InputEvents.Shoot>(OnShoot);
        }

        private void OnDisable()
        {
            _channel.Unsubscribe<InputEvents.Shoot>(OnShoot);
        }

        public TWeapon Configure<TWeapon>(IConfig<TWeapon> config) where TWeapon : IWeapon
        {
            throw new NotImplementedException();
        }

        [Serializable]
        public sealed class Configuration : IConfig<Gun>
        {
            public ShootModuleBase shootModule;
            public AmmoModule ammoModule;
            public ProjectileModule projectileModule;
        }
    }
}
