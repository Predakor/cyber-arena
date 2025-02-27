using UnityEngine;

public class ItemData : ScriptableObject {
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;
    public GameObject prefab;
    public int rarity;
}
