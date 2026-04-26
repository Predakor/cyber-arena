using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private List<ItemData> invList;

    private void Start()
    {
        if (_inventory == null)
        {
            Debug.LogError("Inventory is null", this);
            return;
        }
        invList = _inventory.GetItems();
    }

}
