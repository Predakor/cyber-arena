using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory")]
public class Inventory : ScriptableObject {
    [SerializeField] List<ItemData> items = new();

    public List<ItemData> GetItems() => new(items);

    public void SaveItems(List<ItemData> newItems) {
        items = newItems;
    }

    public void AddItem(ItemData item) {
        items.Add(item);
        Debug.Log($"Added {item.itemName} to the inventory.");
    }

    public void RemoveItem(ItemData item) {
        items.Remove(item);
        Debug.Log($"Removed {item.itemName} from the inventory.");
    }
}
