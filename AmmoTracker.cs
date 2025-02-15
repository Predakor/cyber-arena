using TMPro;
using UnityEngine;

public class AmmoTracker : MonoBehaviour {

    [SerializeField] Weapon trackedWeapon;
    [SerializeField] TextMeshProUGUI ammoCountDisplay;
    [SerializeField] WeaponManager weaponManager;
    public static AmmoTracker instance;


    void Awake() {
        if (instance == null) {
            instance = this;
        }
        if (weaponManager == null) {
            weaponManager = FindObjectOfType<WeaponManager>();
        }
    }

    void Start() {
        Weapon _weapon = weaponManager.CurrentWeapon;
        ChangeTrackedWeapon(_weapon);

    }

    public void ChangeTrackedWeapon(Weapon _weapon) {
        if (_weapon == null) {
            return;
        }

        if (_weapon is RangeWeapon _rangeWeapon) {
            ammoCountDisplay.text = $"{_rangeWeapon.CurrentAmmo}";
        }

        if (_weapon is MeleeWeapon _meleWeapon) {
            ammoCountDisplay.text = "/";
        }

    }
}
