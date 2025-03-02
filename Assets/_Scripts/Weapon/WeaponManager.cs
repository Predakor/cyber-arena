using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class WeaponManager : MonoBehaviour {

    [SerializeField] Weapon _currentWeapon;
    [SerializeField] WeaponInventory _inventory;
    [SerializeField] Animator _animator;
    [SerializeField] PlayerInputHandler _playerInputHandler;
    [SerializeField] WeaponState _weaponState;
    [SerializeField] Transform _weaponTransform;

    [Header("Options")]
    [SerializeField] bool _autoEquipNewWeapon = false;
    [SerializeField] float _timeToIdle = 0;

    public static WeaponManager Instance;

    [Header("Events")]
    public UnityEvent<Weapon> OnWeaponPickup;
    public UnityEvent<Weapon, Weapon> OnWeaponChange; // 1: new weapon, 2: old weapon or null
    public UnityEvent<Weapon> OnWeaponEquipped;

    public Weapon CurrentWeapon {
        get => _currentWeapon;
        private set {
            if (_currentWeapon != value) {
                OnWeaponChange?.Invoke(value, _currentWeapon);
                _inventory.DequipWeapon(_currentWeapon);
                _currentWeapon = value;

                if (_currentWeapon != null) {
                    _currentWeapon.transform.SetParent(_weaponTransform);
                    _currentWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                    _currentWeapon.gameObject.SetActive(true);
                    OnWeaponEquipped?.Invoke(_currentWeapon);
                }
            }
        }
    }

    internal WeaponState CurrentWeaponState {
        get => _weaponState;
        private set {
            if (_weaponState != value) {
                _weaponState = value;
                if (_animator != null) {
                    _animator.SetBool(_weaponState.ToString(), false);
                    _animator.SetBool(value.ToString(), true);
                }
            }
        }
    }

    public void SwapWeapon(InputAction.CallbackContext context) {
        if (!context.performed) return;

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
        CurrentWeapon = weapon.GetComponent<Weapon>();
    }
    public void ReloadCurrentWeapon() {
        if (CurrentWeapon is RangeWeapon rangeWeapon) {
            rangeWeapon.StartReload();
        }
    }

    void Awake() {
        if (Instance == null) Instance = this;
        if (!_animator) _animator = GetComponent<Animator>();
        if (!_inventory) _inventory = GetComponent<WeaponInventory>();
    }

    void Start() {
        _playerInputHandler = PlayerInputHandler.Instance;
        _inventory.OnWeaponPickup += WeaponPickup;

        if (_inventory.IsEmpty) {
            CurrentWeapon = null;
            return;
        }

        EquipWeapon(0);
    }

    void Update() {
        if (_inventory.IsEmpty || CurrentWeapon == null) return;

        if (_playerInputHandler.ShootInput == 1) {
            CurrentWeaponState = WeaponState.Shooting;
            _timeToIdle = Time.time;
            CurrentWeapon.Fire();
        }
        else {
            if (Time.time - _timeToIdle > 2) {
                CurrentWeaponState = WeaponState.Idle;
            }
        }
    }
    void WeaponPickup(GameObject gameObject) {
        OnWeaponPickup?.Invoke(gameObject.GetComponent<Weapon>());
        if (_autoEquipNewWeapon) {
            _inventory.EquipWeapon(gameObject.GetComponent<Weapon>());
        }
    }
}

enum WeaponState {
    Idle,
    Aiming,
    Shooting,
    Reloading,
}
