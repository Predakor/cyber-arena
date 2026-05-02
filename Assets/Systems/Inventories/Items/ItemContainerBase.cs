using UnityEngine;

namespace Systems.Inventories.Items
{
    [RequireComponent(typeof(Collider))]
    public class ItemContainerBase : MonoBehaviour
    {
        [SerializeField] protected InventoryItemBase _item;
        [SerializeField] protected Collider _collider;

        private void Start()
        {
            //this will fail for abstract or generic components
            if (!gameObject.TryGetComponent(out _collider))
            {
                _collider = gameObject.AddComponent<SphereCollider>();
            }

            _collider.isTrigger = true;
        }
        public virtual InventoryItemBase Pickup()
        {
            var item = _item;
            _item = null;
            Destroy(gameObject, 0.2f);
            return item;
        }

        public virtual void SetItem(InventoryItemBase item)
        {
            _item = item;
        }

    }
}
