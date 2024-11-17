using UnityEngine;

public abstract class ItemInstance {
    public string ItemName { get; private set; }
    public Sprite Icon { get; private set; }
    public string Description { get; private set; }
    public GameObject Model { get; private set; }
    public int Rarity { get; private set; }

    public ItemInstance(ItemData itemData) {
        ItemName = itemData.itemName;
        Icon = itemData.icon;
        Description = itemData.description;
        Model = itemData.model;
        Rarity = itemData.rarity;
    }
}
