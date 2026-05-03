using Systems.Channels;
using Systems.Inventories.Items;
using UnityEngine;

namespace Scripts.Inventories
{
    public static class InventoryEvents
    {
        public sealed record ItemAdded(InventoryItemBase Item);
        public sealed record ItemRemoved(InventoryItemBase Item);

    }

    [CreateAssetMenu(fileName = "InventoryChannel", menuName = MenuName + "InventoryChannel")]

    public sealed class InventoryChannel : EventChannelBase
    {
        public void RaiseItemAdded(InventoryItemBase item) => Raise(new InventoryEvents.ItemAdded(item));

        public void RaiseItemRemoved(InventoryItemBase item) => Raise(new InventoryEvents.ItemRemoved(item));
    }
}
