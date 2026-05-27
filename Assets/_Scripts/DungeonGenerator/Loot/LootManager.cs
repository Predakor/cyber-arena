using Helpers.Collections;
using System.Collections.Generic;
using Systems.Inventories.Items;
using Systems.Shared;
using UnityEngine;

public sealed class LootManager : Singleton<LootManager>
{
    [SerializeField] private List<InventoryItemBase> _avaiableLoot = new();
    [field: SerializeField] public ItemContainerBase LootContainer { get; private set; }

    public void Init(List<InventoryItemBase> loot)
    {
        _avaiableLoot = loot;
    }

    public static InventoryItemBase RequestLoot()
    {
        InventoryItemBase item = Instance._avaiableLoot.GetRandom();
        Debug.Log(item);
        return item;
    }

    public static List<InventoryItemBase> RequestLoot(int amount)
    {
        List<InventoryItemBase> list = new();
        for (int i = 0; i < amount; i++)
        {
            list.Add(Instance._avaiableLoot.GetRandom());
        }
        return list;
    }
}
