public interface IRoomModule {
    void HandlePlayerEnter();
    void HandlePlayerExit();
    void HandlePlayerNearby();
    void HandlePlayerFaraway();

    bool IsPreloaded { get; set; }
}
