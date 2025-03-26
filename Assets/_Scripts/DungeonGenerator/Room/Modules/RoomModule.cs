using UnityEngine;

[RequireComponent(typeof(Room))]
public abstract class RoomModule : MonoBehaviour, IRoomModule {
    [SerializeField] protected Room _room;
    [SerializeField] protected bool _isPreloaded = false;

    public virtual bool IsPreloaded {
        get => _isPreloaded;
        set => _isPreloaded = value;
    }

    void Awake() {
        if (_room == null) {
            Debug.LogWarning("No room selected add it in template ", this);
            _room = GetComponent<Room>();
        }
    }

    void OnEnable() {
        _room.OnRoomEnter += HandlePlayerEnter;
        _room.OnRoomExit += HandlePlayerExit;
        _room.OnPlayerNearby += HandlePlayerNearby;
        _room.OnPlayerFaraway += HandlePlayerFaraway;
    }

    void OnDisable() {
        _room.OnRoomEnter -= HandlePlayerEnter;
        _room.OnRoomExit -= HandlePlayerExit;
        _room.OnPlayerNearby -= HandlePlayerNearby;
        _room.OnPlayerFaraway -= HandlePlayerFaraway;
    }

    virtual public void HandlePlayerEnter() { }
    virtual public void HandlePlayerExit() { }
    virtual public void HandlePlayerNearby() { }
    virtual public void HandlePlayerFaraway() { }
}
