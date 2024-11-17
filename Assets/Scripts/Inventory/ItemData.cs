using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject {

    string itemName;
    Sprite icon;
    [TextArea] string description;
    int rarity;

}
