using UnityEngine;
using UnityEngine.UIElements;

public class AmmoTracker : MonoBehaviour {
    UIDocument _uIDocument;
    IntegerField _ammoField;

    int _ammo = 0;
    public int Ammo {
        get => _ammo; private set {
            _ammo = value;
            _ammoField.value = _ammo;
        }
    }


    void OnWeaponChanged(Weapon weapon, Weapon oldWeapon) {
        if (weapon is RangeWeapon rangeWeapon) {
            Ammo = rangeWeapon.CurrentAmmo;
            rangeWeapon.onAmmoChange.AddListener(OnAmmunitionChange);
        }
        if (oldWeapon && oldWeapon is RangeWeapon oldRangeWeapon) {
            oldRangeWeapon.onAmmoChange.RemoveListener(OnAmmunitionChange);
        }
    }

    void OnAmmunitionChange(int ammo) { Ammo = ammo; }


    void Awake() {
        _uIDocument = FindObjectOfType<UIDocument>();
    }

    void Start() {
        WeaponManager _weaponManager = WeaponManager.Instance;
        _weaponManager.OnWeaponChange.AddListener(OnWeaponChanged);

        Weapon currentWeapon = _weaponManager.CurrentWeapon;
        if (currentWeapon != null) {
            if (currentWeapon is RangeWeapon rangeWeapon) {
                Ammo = rangeWeapon.CurrentAmmo;
                return;
            }
        }
        Ammo = 0;
    }

    void OnEnable() {
        var root = _uIDocument.rootVisualElement;
        _ammoField = root.Q<IntegerField>("ammo-field");
    }
}
