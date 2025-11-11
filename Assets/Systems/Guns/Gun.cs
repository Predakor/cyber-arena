using System;
using Systems.Channels;
using Systems.Channels.Inputs;
using Systems.Guns.Modules.ShootModules;
using UnityEngine;

namespace Systems.Guns {
    public sealed class Gun : MonoBehaviour, IGun, IWeapon {

        [SerializeField] private InputsChannel _channel;
        [SerializeField] private GunConfiguration _config;
        public void Use() {
            throw new System.NotImplementedException();
        }

        private void OnEnable() {
            _channel.Subscribe<InputEvents.Shoot>(OnShoot);
        }

        private void OnDisable() {
            _channel.Unsubscribe<InputEvents.Shoot>(OnShoot);
        }

        private void OnShoot(InputEvents.Shoot evt) {
            _config.shootModule.Pressed();
        }

    }

    [Serializable]
    public class GunConfiguration {
        [SerializeField] public ShootModuleBase shootModule;
        [SerializeField] public AmmoModule ammoModule;
        [SerializeField] public ProjectileModule projectileModule;
    }
}
