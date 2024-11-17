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
            if (pickedItem is GunData) {
                Debug.Log(pickedItem);
                pickedItem.model.GetComponent<Weapon>().PickUp(pickedItem as GunData);
            }
            inventory.AddItem(pickedItem);
            Debug.Log($"Picked up: {pickedItem.itemName}");
        }
    }
}
