using Systems.Inventories.Items;

public sealed class LootModule : RoomModule
{
    private ItemContainerBase _container;

    public void PlaceLoot()
    {
        _container = Instantiate(LootManager.Instance.LootContainer, transform);
        _container.SetItem(LootManager.RequestLoot());
    }

    public override void HandlePlayerNearby()
    {
        if (!_isPreloaded)
        {
            PlaceLoot();
            _isPreloaded = true;
        }
    }
}
