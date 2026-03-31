using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private List<ItemData> invList;

    private void Start()
    {
        invList = _inventory.GetItems();
    }

    private void OnTriggerEnter(Collider other)
    {
    }
}
