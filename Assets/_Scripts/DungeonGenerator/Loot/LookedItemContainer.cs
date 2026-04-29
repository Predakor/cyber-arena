using Systems.Inventories.Items;
using UnityEngine;

public class LookedItemContainer : ItemContainerBase
{
    [SerializeField] private bool _isLocked = false;

    public bool IsLocked
    {
        get => _isLocked; private set
        {
            _isLocked = value;
        }
    }


    public bool Lock() => IsLocked = true;
    public bool Unlock() => IsLocked = false;

    protected override void Pickup()
    {
        if (IsLocked)
        {
            return;
        }

        base.Pickup();
    }
}
