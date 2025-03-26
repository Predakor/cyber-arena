using System;
using UnityEngine;

public class Loot : MonoBehaviour {
    [SerializeField] ItemData _item;
    [SerializeField] bool _isLocked = false;

    [SerializeField] ItemContainer _container;

    public void SetItem(ItemData item) {
        _item = item;
        _container.SetItem(item);
    }

    public bool IsLocked {
        get => _isLocked; private set {
            _container.GetComponent<Collider>().enabled = !value;
            _isLocked = value;
        }
    }
    public bool Lock() => IsLocked = true;
    public bool Unlock() => IsLocked = false;

    public void PreviewItem() {
        throw new NotImplementedException();
    }

    private void LockedItemNotify() {
        throw new NotImplementedException();
    }

}
