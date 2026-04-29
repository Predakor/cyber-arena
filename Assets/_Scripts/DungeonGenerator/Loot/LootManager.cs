using Helpers.Collections;
using System.Collections.Generic;
using Systems.Inventories.Items;
using Systems.Shared;
using UnityEngine;

public class LootManager : Singleton<LootManager>
{
    [SerializeField] private List<InventoryItemBase> _avaiableLoot = new();

    public void Init(List<InventoryItemBase> loot)
    {
        _avaiableLoot = loot;
    }

    public static InventoryItemBase RequestLoot()
    {
        InventoryItemBase item = CollectionUtils.RandomElement(Instance._avaiableLoot);
        Debug.Log(item);
        return item;
    }

    public static List<InventoryItemBase> RequestLoot(int amount)
    {
        List<InventoryItemBase> list = new();
        for (int i = 0; i < amount; i++)
        {
            list.Add(CollectionUtils.RandomElement(Instance._avaiableLoot, amount));
        }
        return list;
    }
}
