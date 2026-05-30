using Scripts.Player;
using Systems.Shared.Loggers;
using UnityEngine;

[RequireComponent(typeof(Room))]
public abstract class RoomModule<TModule> : MonoBehaviour, IRoomModule
    where TModule : RoomModule<TModule>
{
    [SerializeField] protected Room _room;
    [field: SerializeField] public bool IsPreloaded { get; protected set; }

    protected IGameLogger logger;

    private void Awake()
    {
        logger = GameLogger.GetOrAdd<TModule>(LogGroup.Rooms);
        if (_room == null)
        {
            logger.Warn("No room selected add it in template ", this);
            _room = GetComponent<Room>();
        }
    }

    private void OnEnable()
    {
        _room.OnRoomEnter += HandlePlayerEnter;
        _room.OnRoomExit += HandlePlayerExit;
        _room.OnPlayerNearby += HandlePlayerNearby;
        _room.OnPlayerFaraway += HandlePlayerFaraway;
    }

    private void OnDisable()
    {
        _room.OnRoomEnter -= HandlePlayerEnter;
        _room.OnRoomExit -= HandlePlayerExit;
        _room.OnPlayerNearby -= HandlePlayerNearby;
        _room.OnPlayerFaraway -= HandlePlayerFaraway;
    }

    public virtual void HandlePlayerEnter(Player player) { }
    public virtual void HandlePlayerExit(Player player) { }
    public virtual void HandlePlayerNearby(Player player) { }
    public virtual void HandlePlayerFaraway(Player player) { }
}
