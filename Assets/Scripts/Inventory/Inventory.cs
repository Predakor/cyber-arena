using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject {

    [SerializeField] List<ItemData> items = new();

    public List<ItemData> GetItems() { return items; }

    public void AddItem(ItemData item) {
        items.Add(item);
    }

    public void RemoveItem(ItemData item) {
        items.Remove(item);
    }
}
