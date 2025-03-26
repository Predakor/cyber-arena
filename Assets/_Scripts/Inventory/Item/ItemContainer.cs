using UnityEngine;

public class ItemContainer : MonoBehaviour {
    [SerializeField] ItemData _itemData;
    [SerializeField] GameObject _prefab;
    [SerializeField] Transform _previewLocation;
    [SerializeField] bool _showItem = true;

    public ItemData ItemData { get => _itemData; }
    public ItemData Pickup() {
        Destroy(gameObject);
        return ItemData;
    }

    public void SetItem(ItemData item) {
        _itemData = item;
        _prefab = item.prefab;
    }

    void Start() {
        if (_prefab == null) {
            _prefab = ItemData.prefab;
        }

        if (_previewLocation == null) {
            _previewLocation = transform;
        }
        if (_showItem) {
            DisplayItem();
        }
    }

    [ContextMenu("show item")]
    void DisplayItem() {
        if (!_prefab || !_showItem) { return; }

        Transform _transform = _previewLocation;

        Instantiate(_prefab, _transform.position, _transform.rotation, _transform);

    }


}
