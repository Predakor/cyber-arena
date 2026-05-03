using Assets.Scripts.Utils;
using Scripts.Inventories;
using Systems.Inventories;
using Systems.Inventories.Items;
using UnityEngine;

namespace Scripts.Player
{

    [RequireComponent(typeof(Collider))]
    public sealed class InventoryManager : MonoBehaviour
    {
        [SerializeField] private Collider _pickupCollider;
        [SerializeField] private Inventory _inventory;
        [SerializeField] private InventoryChannel _channel;

        private void Awake()
        {
            gameObject.EnsureComponent(out _pickupCollider);
        }

        private void Start()
        {
            _pickupCollider.isTrigger = true;
        }


        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Collided with {other.name}");
            if (other.TryGetComponent(out ItemContainerBase itemContainer))
            {
                var item = itemContainer.Pickup();
                _inventory.AddItem(item);
                _channel.RaiseItemAdded(item);
            }
        }
    }
}
