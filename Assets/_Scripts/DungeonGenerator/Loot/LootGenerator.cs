using UnityEngine;

public class LootGenerator : MonoBehaviour {
    [SerializeField] Loot _lootPedestal;
    [SerializeField] Room _room;

    bool _preloaded = false;

    public void PlaceLoot() {
        Loot loot = Instantiate(_lootPedestal, transform);
        loot.SetItem(LootManager.RequestLoot());
    }

    void OnEnable() {
        _room.OnPlayerNearby += OnPlayerNearby;
    }

    void OnDisable() {
        _room.OnPlayerNearby -= OnPlayerNearby;
    }


    void OnPlayerNearby() {
        if (!_preloaded) {
            PlaceLoot();
            _preloaded = true;
        }
    }
}
