using UnityEngine;
using UnityEngine.UIElements;

public class AmmoTracker : MonoBehaviour {
    [SerializeField] private RangeWeapon trackedWeapon;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private UIDocument uIDocument;

    private IntegerField ammoField;

    void Start() {
        trackedWeapon = weaponManager.CurrentWeapon as RangeWeapon;

        var root = uIDocument.rootVisualElement;
        ammoField = root.Q<IntegerField>("ammo-field");

        if (trackedWeapon != null) {
            ammoField.SetValueWithoutNotify((int)trackedWeapon.CurrentAmmo); // Initial UI update
        }
    }

    void Update() {
        trackedWeapon = weaponManager.CurrentWeapon as RangeWeapon;

        if (trackedWeapon != null) {
            ammoField.SetValueWithoutNotify((int)trackedWeapon.CurrentAmmo);
        }
    }
}
