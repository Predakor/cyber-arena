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
        protected virtual void Pickup()
        {
            Destroy(gameObject);
        }

        public virtual void SetItem(InventoryItemBase item)
        {
            _item = item;
        }
    }
}
