using Helpers.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : Singleton<LootManager> {
    [SerializeField] List<ItemData> _avaiableLoot = new();

    public void Init(List<ItemData> loot) {
        _avaiableLoot = loot;
    }

    public static ItemData RequestLoot() {
        ItemData item = CollectionUtils.RandomElement(Instance._avaiableLoot);
        Debug.Log(item);
        return item;
    }

    public static List<ItemData> RequestLoot(int amount) {
        List<ItemData> list = new();
        for (int i = 0; i < amount; i++) {
            list.Add(CollectionUtils.RandomElement(Instance._avaiableLoot, amount));
        }
        return list;
    }
}
