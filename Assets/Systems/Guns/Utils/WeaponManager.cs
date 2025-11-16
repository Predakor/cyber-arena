using System.Collections.Generic;
using Systems.Channels;
using Systems.Channels.Inputs;
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

        readonly List<IWeapon> _weapons = new(3);

        [SerializeField]
        List<GameObject> _gWeapons = new(3);

        [SerializeField]
        Inventory _scriptableObjectIntentory;

        [SerializeField]
        InputsChannel _channel;

        [SerializeField]
        Transform _transform;

        public IWeapon CurrentWeapon { get; private set; }

        private void Start() {
            //_weapons.AddRange(_scriptableObjectIntentory.GetItems());
        }

        public void Equip(IWeapon weapon) => EquipWeapon(_weapons.Find(w => w == weapon));

        public void Equip(int index) {
            var gameObject = _gWeapons[index];
            if (gameObject.TryGetComponent<Gun>(out var gun)) {
                EquipWeapon(gun);
                return;
            }

            Debug.Log("weapon not  found");
        }

        public void Equip(InputEvents.SelectWeapon @event) {
            Equip(@event.WeaponNumber);
        }

        public void Pickup<TWeapon>(IConfig<TWeapon> config)
            where TWeapon : MonoBehaviour, IWeapon {
            if (IsInventoryFull) {
                return;
            }

            var weapon = WeaponFactory.Instance.Create(config);
            _weapons.Add(weapon);
        }

        bool IsInventoryFull => _weapons.Count >= MaxInventorySize;

        void EquipWeapon(IWeapon weapon) {
            if (weapon == null) {
                return;
            }

            SetActive(CurrentWeapon, false);
            SetActive(weapon, true);

            CurrentWeapon = weapon;
        }

        void SetActive(IWeapon weapon, bool active) {
            if (weapon is MonoBehaviour mb) {
                mb.gameObject.SetActive(active);
                mb.gameObject.transform.SetParent(transform);
                mb.gameObject.transform.SetPositionAndRotation(
                    transform.position,
                    gameObject.transform.rotation
                );
            }
        }

        void OnEnable() => _channel.Subscribe<InputEvents.SelectWeapon>(Equip);

        void OnDisable() => _channel.Unsubscribe<InputEvents.SelectWeapon>(Equip);
    }
}
