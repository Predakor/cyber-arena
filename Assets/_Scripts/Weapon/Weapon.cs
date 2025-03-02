using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    #region stats
    [Header("Weapon stats")]


    [Header("Weapon events")]
    Event OnEquip;

    [SerializeField] Transform handTransform;

    #endregion

    public virtual void LoadStats(GunData gunData) {

    }

    [ContextMenu("Fire")]
    public virtual void Fire() {

    }
}
