using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomLinks : MonoBehaviour {
    [SerializeField] int depth;
    [SerializeField] Vector3 position;
    [SerializeField] RoomConnection prevRoom;
    [SerializeField] List<RoomConnection> nextRooms = new();
    [SerializeField] RoomGenerator room = null;


    public int Depth { get => depth; set => depth = value; }
    public Vector3 Position { get => position; set => position = value; }

    public RoomGenerator Room { get => room; }
    public RoomConnection? PrevRoom { get => prevRoom; }

    public void SetPrevRoom(RoomLinks roomNode, Vector3 direction) {
        prevRoom = new(direction, roomNode);
    }

    public void AddNextRoom(RoomLinks roomNode, Vector3 direction) {
        nextRooms.Add(new(direction, roomNode));
    }

    public RoomConnection? GetPrevRoom() {
        if (PrevRoom.HasValue) {
            return PrevRoom.Value;
        }
        return null;
    }

    public RoomLinks GetPrevNode() {
        if (PrevRoom.HasValue) {
            return PrevRoom.Value.room;
        }
        return null;
    }

    public List<Vector3> GetConnectedDirections() {
        List<Vector3> directions = new();

        if (prevRoom.direction != null) {
            directions.Add(prevRoom.direction);
        }
        if (nextRooms.Count > 0) {
            foreach (var nextRoom in nextRooms) {
                directions.Add(nextRoom.direction);
            }
        }
        return directions;
    }

}

[Serializable]
public struct RoomConnection {
    public Vector3 direction;
    public RoomLinks room;

    public RoomConnection(Vector3 direction, RoomLinks room) {
        this.direction = direction;
        this.room = room;
    }
}
