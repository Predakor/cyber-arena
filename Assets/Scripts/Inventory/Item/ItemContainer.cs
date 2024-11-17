using UnityEngine;

public class ItemContainer : MonoBehaviour {
    public ItemData itemData;

    public ItemData TakeItem() {
        Destroy(gameObject);
        return itemData;
    }
}
