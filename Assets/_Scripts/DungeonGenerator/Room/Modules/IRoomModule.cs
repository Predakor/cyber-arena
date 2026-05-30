using Scripts.Player;

public interface IRoomModule
{
    void HandlePlayerEnter(Player player);
    void HandlePlayerExit(Player player);
    void HandlePlayerNearby(Player player);
    void HandlePlayerFaraway(Player player);

    bool IsPreloaded { get; }
}
