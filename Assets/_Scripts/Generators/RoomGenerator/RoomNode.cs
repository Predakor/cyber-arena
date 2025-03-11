using UnityEngine;

public class RoomNode : MonoBehaviour {
    [SerializeField] RoomData roomData;
    [SerializeField] RoomConnections roomConnections;

    public RoomData RoomData {
        get => roomData; set {
            roomData = value;
        }
    }
    public RoomConnections RoomConnections {
        get => roomConnections; set {
            roomConnections = value;
        }
    }
}
