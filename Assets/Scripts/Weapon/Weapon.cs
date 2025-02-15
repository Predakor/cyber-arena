using UnityEngine;

public class Weapon : MonoBehaviour {

    #region stats
    [Header("Weapon stats")]


    [Header("Weapon events")]
    Event OnEquip;

    [SerializeField] Transform handTransform;

    #endregion

    protected virtual void LoadStats(GunData gunData) {

    }
    public virtual void PickUp(GunData gunData) {
        WeaponManager.instance.PickupNewWeapon(gameObject);
        LoadStats(gunData);
    }

    [ContextMenu("Fire")]
    public virtual void Fire() {

    }
}
