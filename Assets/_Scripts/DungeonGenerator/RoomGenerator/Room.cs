using System;
using UnityEngine;



public class Room : MonoBehaviour {

    [SerializeField] RoomNode _roomNode;

    public event Action OnRoomEnter;
    public event Action OnRoomExit;
    public event Action OnPlayerNearby;

    public void Init(RoomNode roomNode) {
        _roomNode = roomNode;
        float roomSize = roomNode.Data.GetRoomWorldSize() * 0.8f;
        GetComponent<BoxCollider>().size = new(roomSize, 6, roomSize);
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }
        OnEnter();
    }

    void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Player")) {
            return;
        }
        OnExit();
    }

    void OnEnter() {
        OnRoomEnter?.Invoke();
        _roomNode.GetNeighbours().ForEach((__neighbour) => {
            RoomNode neighbour = __neighbour as RoomNode;
            if (neighbour != null) {
                neighbour.GetComponentInChildren<Room>().OnPlayerNearby?.Invoke();
            }
        });
    }

    void OnExit() {
        OnRoomExit?.Invoke();
    }
}
