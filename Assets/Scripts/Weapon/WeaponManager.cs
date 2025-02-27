using UnityEngine;
using UnityEngine.Events;


enum WeaponState {
    Idle,
    Aiming,
    Shooting,
    Reloading,
}

public class WeaponManager : MonoBehaviour {

    [SerializeField] Weapon _currentWeapon;
    [SerializeField] WeaponInventory _inventory;

    [SerializeField] Animator _animator;
    [SerializeField] PlayerInputHandler _playerInputHandler;

    [SerializeField] WeaponState _weaponState;
    [SerializeField] Transform _weaponTransform;

    [SerializeField] bool _autoEquipNewWeapon = false;


    [Header("Options")]

    float _lastShotTime = 0;

    public static WeaponManager Instance;

    [Header("Events")]
    public UnityEvent<Weapon> OnWeaponPickup;
    public UnityEvent<Weapon, Weapon> OnWeaponChange;//1 new weapon 2 old weapon or null
    public UnityEvent<Weapon> OnWeaponEquiped;

    void WeaponPickup(GameObject gameObject) {
        if (_autoEquipNewWeapon) {
            _inventory.EquipWeapon(gameObject.GetComponent<Weapon>());
        }
    }

    public Weapon CurrentWeapon {
        get => _currentWeapon; private set {
            OnWeaponChange?.Invoke(value, _currentWeapon);
            _inventory.DequipWeapon(_currentWeapon);
            _currentWeapon = value;
        }
    }

    internal WeaponState CurrentWeaponState {
        get => _weaponState; private set {
            if (_weaponState == value) { return; }

            _weaponState = value;
            if (_animator != null) {
                _animator.SetBool(_weaponState.ToString(), false);
                _animator.SetBool(value.ToString(), true);
            }
        }
    }

    void Awake() {
        if (Instance == null) { Instance = this; }
        if (!_animator) { _animator = GetComponent<Animator>(); }
        if (!_inventory) { _inventory = GetComponent<WeaponInventory>(); }
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
        if (_inventory.IsEmpty) { return; }

        //!!!!change it to new unity input system since its already in use!!!!
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipWeapon(2);

        if (_playerInputHandler.ShootInput == 1) {
            CurrentWeaponState = WeaponState.Shooting;
            _lastShotTime = Time.time;
            CurrentWeapon.Fire();
        }
        else {
            if (Time.time - _lastShotTime > 2) {
                CurrentWeaponState = WeaponState.Idle;
            }
        }
    }

    public void EquipWeapon(int index) {
        GameObject weapon = _inventory.EquipWeapon(index);
        if (weapon != null) {
            weapon.transform.parent = _weaponTransform;
            weapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            weapon.SetActive(true);

            CurrentWeapon = weapon.GetComponent<Weapon>();
        }

        //=========TODO========== 
        // add handling for weapons that can be used in
        //1 hand only
        //need 2 hands
        //1 is equiped in each hand
    }
}
