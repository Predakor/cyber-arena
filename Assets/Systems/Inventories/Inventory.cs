using System.Collections.Generic;
using System.Linq;
using Systems.Inventories.Items;
using UnityEngine;

namespace Systems.Inventories
{
    [CreateAssetMenu(menuName = MenuPath)]
    public class Inventory : ScriptableObject
    {
        protected const string MenuPath = "Items/Inventory";

        [SerializeField] private bool EnableLog = false;
        [SerializeField] private List<InventoryItemBase> items = new();

        public IReadOnlyList<InventoryItemBase> GetItems() => items.AsReadOnly();

        public void SaveItems(List<InventoryItemBase> newItems)
        {
            items = newItems;
        }
        public void LoadItems(IEnumerable<InventoryItemBase> newItems)
        {
            items = newItems.ToList();
        }

        public void AddItem(InventoryItemBase item)
        {
            items.Add(item);
            Log($"Added {item.Name} to the inventory.");
        }

        public void RemoveItem(InventoryItemBase item)
        {
            items.Remove(item);
            Log($"Removed {item.Name} from the inventory.");
        }

        private void Log(string message)
        {
            if (EnableLog)
            {
                Debug.Log(message);
            }
        }
    }

}