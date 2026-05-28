using Systems.Inventories.Items;
using Systems.Shared.Channels;
using UnityEngine;

namespace Scripts.Inventories
{
    [ChannelEvents(typeof(InventoryChannel))]
    public static class InventoryEvents
    {
        public sealed record ItemAdded(InventoryItemBase Item);
        public sealed record ItemRemoved(InventoryItemBase Item);

    }

    [CreateAssetMenu(fileName = "InventoryChannel", menuName = MenuName + "InventoryChannel")]

    public sealed class InventoryChannel : EventChannelBase<InventoryChannel>
    {
        [ContextMenu("Sync Event Log Rules")]
        private void PopulateEventRules() => _logger.SyncEventLogRules();

        public void RaiseItemAdded(InventoryItemBase item) => Raise(new InventoryEvents.ItemAdded(item));
        public void RaiseItemRemoved(InventoryItemBase item) => Raise(new InventoryEvents.ItemRemoved(item));

    }
}
