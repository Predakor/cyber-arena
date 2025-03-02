using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponInventory : MonoBehaviour {
    const int MAX_WEAPONS = 3;

    [SerializeField] Inventory _scriptableObjectIntentory;
    [SerializeField] List<ItemData> _scriptableInventoryCopy;
    [SerializeField] List<GameObject> _inventory;

    public event Action OnInventoryFull;
    public event Action<GameObject> OnWeaponPickup;
    public event Action OnFirstItemPickup;

    #region getters
    public bool IsEmpty { get; private set; }
    public bool IsFull { get; private set; }

    #endregion
    #region helpers
    GameObject FindWeapon(Weapon weapon) {
        return _inventory.Find(_weapon => weapon == _weapon.GetComponent<Weapon>());
    }

    void CheckFlags() {
        IsEmpty = _inventory.Count == 0;
        IsFull = _inventory.Count == MAX_WEAPONS;
    }
    void AddItem(GameObject gameObject) {
        _inventory.Add(gameObject);

        CheckFlags();

        if (IsFull) {
            OnInventoryFull?.Invoke();
            OnWeaponPickup(gameObject);
        }
    }

    #endregion

    //function to give spawned weapon when demanded
    //equip makes the weapon active weapon 
    public GameObject EquipWeapon(Weapon weapon) {
        GameObject selectedWeapon = FindWeapon(weapon);
        selectedWeapon.SetActive(true);
        return selectedWeapon;
    }

    public GameObject EquipWeapon(int index) {
        if (index > _inventory.Count) return null;
        index = Mathf.Clamp(index, 0, _inventory.Count);
        return _inventory[index];
    }

    public void DequipWeapon(int index) {
        if (index > _inventory.Count) return;
        index = Mathf.Clamp(index, 0, _inventory.Count);
        _inventory[index].SetActive(false);
    }

    public void DequipWeapon(Weapon weapon) {
        GameObject selectedWeapon = FindWeapon(weapon);
        if (selectedWeapon) {
            selectedWeapon.SetActive(false);
        }
    }

    //some method to pickup gun in item form
    public void Pickup(GunData data) {
        if (!IsFull) _scriptableInventoryCopy.Add(data);
        _inventory.Add(CreateWeapon(data));

        OnInventoryFull?.Invoke();
    }
    //pickup onnly ads to intentory
    public void Pickup(Weapon weapon) {
        if (!IsFull) AddItem(weapon.gameObject);
        OnInventoryFull?.Invoke();
    }

    //Create weapon from item data
    GameObject CreateWeapon(GunData data) {
        GameObject InstantiateWeapon(GameObject weapon) {
            GameObject instantiedWeapon = Instantiate(weapon, transform.position, transform.rotation);
            instantiedWeapon.GetComponent<Weapon>().LoadStats(data);
            instantiedWeapon.transform.SetParent(transform, true);
            instantiedWeapon.SetActive(false);
            return instantiedWeapon;
        }

        return InstantiateWeapon(data.prefab);
    }


    //save and loading probb with scriptable objects
    void Awake() {
        //===========TODO=======================
        //some magical method to load fromm disk 
        CheckFlags();

        _scriptableInventoryCopy = _scriptableObjectIntentory.GetItems();

        //load all from scriptable objects
        if (IsEmpty && _scriptableInventoryCopy.Count > 0) {
            foreach (GunData data in _scriptableInventoryCopy.Cast<GunData>()) {
                GameObject weapon = CreateWeapon(data);
                AddItem(weapon);
            }
        }
    }

}
