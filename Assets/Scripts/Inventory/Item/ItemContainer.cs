using UnityEngine;

public class ItemContainer : MonoBehaviour {
    public ItemData itemData;
    [SerializeField] bool showItem = true;
    [SerializeField] GameObject itemModel;
    [SerializeField] Transform previewLocation;

    private void Start() {
        if (itemModel == null) {
            itemModel = itemData.model;
        }

        if (previewLocation == null) {
            previewLocation = transform;
        }
        if (showItem) {
            DisplayItem();
        }
    }

    [ContextMenu("show item")]
    void DisplayItem() {
        if (!itemModel || !showItem) { return; }

        Transform _transform = previewLocation;

        Instantiate(itemModel, _transform.position, _transform.rotation, _transform);

    }

    public ItemData TakeItem() {
        Destroy(gameObject);
        return itemData;
    }
}
