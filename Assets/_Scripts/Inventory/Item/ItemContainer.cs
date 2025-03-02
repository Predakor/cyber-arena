using UnityEngine;

public class ItemContainer : MonoBehaviour {
    public ItemData itemData;
    [SerializeField] bool showItem = true;
    [SerializeField] GameObject prefab;
    [SerializeField] Transform previewLocation;

    private void Start() {
        if (prefab == null) {
            prefab = itemData.prefab;
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
        if (!prefab || !showItem) { return; }

        Transform _transform = previewLocation;

        Instantiate(prefab, _transform.position, _transform.rotation, _transform);

    }

    public ItemData TakeItem() {
        Destroy(gameObject);
        return itemData;
    }
}
