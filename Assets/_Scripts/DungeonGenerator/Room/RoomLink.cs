using System;
using UnityEngine;

[Serializable]
public class RoomLink<T> {
    public Vector3 direction;
    public float distance;
    [SerializeField] public T room;

    public RoomLink(T room, float distance, Vector3 direction) {
        this.direction = direction;
        this.distance = distance;
        this.room = room;
    }

    public RoomLink(T room) {
        this.room = room;
    }
}
