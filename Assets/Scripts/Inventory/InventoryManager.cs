using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    [SerializeField] Inventory inventory;
    [SerializeField] List<ItemData> invList;


    void Start() {
        invList = inventory.GetItems();
    }

    void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out ItemContainer itemContainer)) {
            ItemData pickedItem = itemContainer.TakeItem();
            if (pickedItem is GunData pickedGunData) {
                Weapon weapon = pickedItem.model.GetComponent<Weapon>();
                weapon.PickUp(pickedGunData);
            }
            invList.Add(pickedItem);
        }
    }
}
