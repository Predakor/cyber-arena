using System.Collections.Generic;
using Systems.Guns.Interfaces;
using UnityEngine;

namespace Systems.Guns.Utils {
    public interface IWeaponManager {
        public IWeapon CurrentWeapon { get; }

        public void Equip(int index);
        public void Equip(IWeapon weapon);
        public void Pickup<TWeapon>(IConfig<TWeapon> config)
            where TWeapon : MonoBehaviour, IWeapon;
    }

    public sealed class WeaponManager : MonoBehaviour, IWeaponManager {
        const byte MaxInventorySize = 3;

        private readonly List<IWeapon> _weapons = new(3);

        public IWeapon CurrentWeapon { get; private set; }

        public void Equip(IWeapon weapon) => EquipWeapon(_weapons.Find(w => w == weapon));

        public void Equip(int index) => EquipWeapon(_weapons[index]);

        public void Pickup<TWeapon>(IConfig<TWeapon> config)
            where TWeapon : MonoBehaviour, IWeapon {
            if (IsInventoryFull) {
                return;
            }

            var weapon = WeaponFactory.Instance.Create(config);
            _weapons.Add(weapon);
        }

        private bool IsInventoryFull => _weapons.Count >= MaxInventorySize;

        private void EquipWeapon(IWeapon weapon) {
            if (weapon == null) {
                return;
            }

            SetActive(CurrentWeapon, false);
            SetActive(weapon, true);

            CurrentWeapon = weapon;
        }

        private void SetActive(IWeapon weapon, bool active) {
            if (weapon is MonoBehaviour mb) {
                mb.gameObject.SetActive(active);
            }
        }
    }
}
