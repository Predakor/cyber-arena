using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    [SerializeField] Inventory _inventory;
    [SerializeField] WeaponInventory _weaponInventory;

    [SerializeField] List<ItemData> invList;


    void Start() {
        invList = _inventory.GetItems();
    }

    void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out ItemContainer itemContainer)) {
            ItemData pickedItem = itemContainer.Pickup();
            if (pickedItem is GunData pickedGunData) {
                _weaponInventory.Pickup(pickedGunData);
            }
            //invList.Add(pickedItem);
        }
    }
}
