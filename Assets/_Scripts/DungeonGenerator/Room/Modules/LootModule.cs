using UnityEngine;

public class LootModule : RoomModule {
    [SerializeField] Loot _lootPedestal;

    public void PlaceLoot() {
        Loot loot = Instantiate(_lootPedestal, transform);
        loot.SetItem(LootManager.RequestLoot());
    }

    public override void HandlePlayerNearby() {
        if (!_isPreloaded) {
            PlaceLoot();
            _isPreloaded = true;
        }
    }
}
