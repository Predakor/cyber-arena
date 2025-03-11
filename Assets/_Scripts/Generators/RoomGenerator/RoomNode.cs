using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode : MonoBehaviour {
    [SerializeField] int depth;
    [SerializeField] Vector3 position;
    [SerializeField] RoomConnection? prevRoom;
    [SerializeField] List<RoomConnection> nextRooms = new();
    [SerializeField] RoomGenerator room = null;


    public int Depth { get => depth; set => depth = value; }
    public Vector3 Position { get => position; set => position = value; }

    public RoomGenerator Room { get => room; }
    public RoomConnection? PrevRoom { get => prevRoom; }

    public void SetPrevRoom(RoomNode roomNode, Vector3 direction) {
        prevRoom = new(direction, roomNode);
    }

    public void AddNextRoom(RoomNode roomNode, Vector3 direction) {
        nextRooms.Add(new(direction, roomNode));
    }

    public Vector3 GetPrevRoomDir() {
        if (PrevRoom.HasValue) {
            return PrevRoom.Value.direction;
        }
        return Position;
    }

    public RoomNode GetPrevNode() {
        if (PrevRoom.HasValue) {
            return PrevRoom.Value.room;
        }
        return null;
    }

}

[Serializable]
public struct RoomConnection {
    public Vector3 direction;
    public RoomNode room;

    public RoomConnection(Vector3 direction, RoomNode room) {
        this.direction = direction;
        this.room = room;
    }
}
