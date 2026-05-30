using Scripts.Player;
using Systems.Inventories.Items;

public sealed class LootModule : RoomModule<LootModule>
{
    private ItemContainerBase _container;

    public void PlaceLoot()
    {
        _container = Instantiate(LootManager.Instance.LootContainer, transform);
        _container.SetItem(LootManager.RequestLoot());
    }

    public override void HandlePlayerNearby(Player player)
    {
        if (!IsPreloaded)
        {
            PlaceLoot();
            IsPreloaded = true;
        }
    }
}
