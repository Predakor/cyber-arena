using Assets.Scripts.Utils;
using System;
using Systems.Channels;
using Systems.Shared;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[Obsolete("Use New Weapon Manager in systems")]
public class WeaponManager : Singleton<WeaponManager> {
    [SerializeField]
    Gun _currentWeapon;

    [SerializeField]
    WeaponInventory _inventory;

    [SerializeField]
    Animator _animator;

    [SerializeField]
    InputsChannel _inputChannel;

    [SerializeField]
    GunState _weaponState;

    [SerializeField]
    Transform _weaponTransform;

    [Header("Options")]
    [SerializeField]
    bool _autoEquipNewWeapon = false;

    [SerializeField]
    float _timeToIdle = 0;

    [Header("Events")]
    public UnityEvent<Gun> OnWeaponPickup;
    public UnityEvent<Gun, Gun> OnWeaponChange; // 1: new weapon, 2: old weapon or null
    public UnityEvent<Gun> OnWeaponEquipped;

    public Gun CurrentWeapon {
        get => _currentWeapon;
        private set {
            if (_currentWeapon == value) {
                return;
            }

            OnWeaponChange?.Invoke(value, _currentWeapon);
            _inventory.DequipWeapon(_currentWeapon);
            _currentWeapon = value;

            if (_currentWeapon == null) {
                return;
            }

            EquipWeapon(_currentWeapon.gameObject);

            OnWeaponEquipped?.Invoke(_currentWeapon);
        }
    }

    internal GunState CurrentWeaponState {
        get => _weaponState;
        private set {
            if (_weaponState == value) {
                return;
            }
            _weaponState = value;
            if (_animator != null) {
                _animator.SetBool(_weaponState.ToString(), false);
                _animator.SetBool(value.ToString(), true);
            }
        }
    }

    public void SwapWeapon(InputAction.CallbackContext context) {
        if (!context.performed) {
            return;
        }

        if (context.control is KeyControl key) {
            int index = key.keyCode - Key.Digit1;
            EquipWeapon(index);
        }
    }

    public void EquipWeapon(int index) {
        GameObject weapon = _inventory.EquipWeapon(index);
        if (weapon != null) {
            EquipWeapon(weapon);
        }
    }

    public void EquipWeapon(GameObject weapon) {
        weapon.transform.SetParent(_weaponTransform);
        weapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        weapon.SetActive(true);
        CurrentWeapon = weapon.GetComponent<Gun>();
    }

    public void ReloadCurrentWeapon() {
        if (CurrentWeapon is Gun rangeWeapon) {
            rangeWeapon.Reload();
        }
    }

    protected override void Awake() {
        base.Awake();
        gameObject.EnsureComponent(out _animator).EnsureComponent(out _inventory);
    }

    void Start() {
        //_playerInputHandler = PlayerInputHandler.Instance;

        if (_inventory.IsEmpty) {
            CurrentWeapon = null;
            return;
        }

        EquipWeapon(0);
    }

    private void OnEnable() {
        _inventory.OnWeaponPickup += WeaponPickup;
    }

    private void OnDisable() {
        _inventory.OnWeaponPickup -= WeaponPickup;
    }

    private void WeaponPickup(GameObject gameObject) {
        var weapon = gameObject.GetComponent<Gun>();
        OnWeaponPickup?.Invoke(weapon);
        if (_autoEquipNewWeapon) {
            _inventory.EquipWeapon(weapon);
        }
    }
}
