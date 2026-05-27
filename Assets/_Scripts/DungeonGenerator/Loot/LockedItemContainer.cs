using Systems.Inventories.Items;
using UnityEngine;

public class LookedItemContainer : ItemContainerBase
{
    [SerializeField] private bool _isLocked = false;

    public bool IsLocked
    {
        get => _isLocked;
        private set
        {
            _isLocked = value;
        }
    }


    public bool Lock() => IsLocked = true;
    public bool Unlock() => IsLocked = false;

    public override InventoryItemBase Pickup()
    {
        if (IsLocked)
        {
            return null;
        }

        return base.Pickup();
    }
}
