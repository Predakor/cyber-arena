using System;
using UnityEngine;

public class Room : MonoBehaviour, IRoomModule {
    [SerializeField] RoomNode _roomNode;

    public bool IsPreloaded { get; set; }
    public RoomNode Node => _roomNode;

    public event Action OnRoomEnter;
    public event Action OnRoomExit;
    public event Action OnPlayerNearby;
    public event Action OnPlayerFaraway;

    public void Init(RoomNode roomNode) {
        _roomNode = roomNode;
        float roomSize = roomNode.Data.GetRoomWorldSize() * 0.8f;
        GetComponent<BoxCollider>().size = new(roomSize, 6, roomSize);
    }

    void OnTriggerEnter(Collider other) {
        if (IsPlayer(other)) {
            HandlePlayerEnter();
        }
    }

    void OnTriggerExit(Collider other) {
        if (IsPlayer(other)) {
            HandlePlayerExit();
        }
    }

    public void HandlePlayerEnter() {
        OnRoomEnter?.Invoke();
        _roomNode.GetNeighbours().ForEach((__neighbour) => {
            RoomNode neighbour = __neighbour as RoomNode;
            if (neighbour != null) {
                neighbour.GetComponentInChildren<Room>().HandlePlayerNearby();
            }
        });
    }

    public void HandlePlayerExit() {
        OnRoomExit?.Invoke();

        //wait for new room and unload all nearby nodes
        //that are not in new room nearby nodes
    }

    public void HandlePlayerNearby() {
        Debug.Log("Player Nearby", this);
        OnPlayerNearby?.Invoke();
    }

    public void HandlePlayerFaraway() {
        return;
        //not implemented
        OnPlayerFaraway?.Invoke();
    }


    bool IsPlayer(Collider other) => other.CompareTag("Player");
}
