using System.Collections.Generic;
using System.Linq;
using Systems.Inventories.Items;
using Systems.Shared.Loggers;
using UnityEngine;

namespace Systems.Inventories
{
    [CreateAssetMenu(menuName = MenuPath)]
    public class Inventory : ScriptableObject
    {
        protected const string MenuPath = "Items/Inventory";

        [SerializeField] private List<InventoryItemBase> items = new();

        [Header("Initial State (editor only)")]
        [Tooltip("This list is only used to set the initial state of the inventory. It will not be saved or loaded at runtime.")]
        [SerializeField] private List<InventoryItemBase> initialItems = new();

        private IGameLogger _logger;

        private void OnEnable()
        {
            _logger ??= GameLogger.GetOrAdd<Inventory>();
            items = initialItems.ToList();
        }

        public IReadOnlyList<InventoryItemBase> GetItems() => items.AsReadOnly();
        public IReadOnlyList<TType> GetItemsOfType<TType>()
            where TType : ScriptableObject => items
                .Where(i => i.Payload is TType)
                .Select(i => i.Payload as TType)
                .ToList()
                .AsReadOnly();

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
            _logger.Debug($"Added {item.Name} to the inventory.", this);
        }

        public void RemoveItem(InventoryItemBase item)
        {
            items.Remove(item);
            _logger.Debug($"Removed {item.Name} from the inventory.", this);
        }
    }
}