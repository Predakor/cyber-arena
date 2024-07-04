using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {


    public List<Item> items;




    public void AddItem(Item item) {
        if (items.Contains(item)) {
            return;
        }
        items.Add(item);
    }
    public void RemoveItem(Item item) {
        if (!items.Contains(item)) {
            return;
        }
        items.Remove(item);
    }

    public bool ContainsItem(Item item) {
        return items.Contains(item);
    }


    void Start() {

    }
}
