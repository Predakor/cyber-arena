using UnityEngine;

public class RoomNode : Node {

    [SerializeField] RoomData roomData;
    [SerializeField] RoomLinks roomConnections;
    [SerializeField] RoomGenerator room;

    public RoomGenerator Room { get => room; set => room = value; }

    public RoomData RoomData {
        get => roomData;
        set => roomData = value;
    }

    public RoomLinks RoomConnections {
        get => roomConnections;
        set => roomConnections = value;
    }
}
