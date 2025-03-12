using UnityEngine;

public class RoomNode : MonoBehaviour {
    [SerializeField] RoomData roomData;
    [SerializeField] RoomLinks roomConnections;

    public RoomData RoomData {
        get => roomData; set {
            roomData = value;
        }
    }
    public RoomLinks RoomConnections {
        get => roomConnections; set {
            roomConnections = value;
        }
    }
}
