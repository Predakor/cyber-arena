using System;
using Systems.Guns.Interfaces;
using Systems.Shared;
using UnityEngine;

namespace Systems.Guns.Utils {
    public class WeaponFactory : Singleton<WeaponFactory> {
        [SerializeField]
        private Gun _gun;

        public TWeapon Create<TWeapon>(IConfig<TWeapon> config)
            where TWeapon : IWeapon {

            return config switch {
                IConfig<Gun> gunConfig => (TWeapon)(object)Instantiate(_gun).Configure(gunConfig),
                //IConfig<Meele> meleeConfig => Instantiate(_meele).Configure(meleeConfig).gameObject,
                _ => throw new NotImplementedException(),
            };

        }
    }
}
