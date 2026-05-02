using Assets.Scripts.Utils;
using System.Collections.Generic;
using System.Linq;
using Systems.Inventories;
using Systems.Inventories.Items;
using UnityEngine;

namespace Scripts.Player
{

    [RequireComponent(typeof(Collider))]
    public sealed class InventoryManager : MonoBehaviour
    {
        [SerializeField] private Collider _pickupCollider;
        [SerializeField] private Inventory inventory;

        [SerializeField] private List<InventoryItemBase> _items = new();

        private void Awake()
        {
            gameObject.EnsureComponent(out _pickupCollider);
        }

        private void Start()
        {
            _pickupCollider.isTrigger = true;
            _items = inventory
                .GetItems()
                .ToList();
        }


        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Collided with {other.name}");
            if (other.TryGetComponent(out ItemContainerBase itemContainer))
            {
                var item = itemContainer.Pickup();
                _items.Add(item);
            }
        }
    }
}
