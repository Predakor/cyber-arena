using UnityEngine;

public class LootModule : RoomModule {
    [SerializeField] LookedItemContainer _lootPedestal;

    public void PlaceLoot() {
        LookedItemContainer loot = Instantiate(_lootPedestal, transform);
        loot.SetItem(LootManager.RequestLoot());
    }

    public override void HandlePlayerNearby() {
        if (!_isPreloaded) {
            PlaceLoot();
            _isPreloaded = true;
        }
    }
}
