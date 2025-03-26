using System;
using UnityEngine;

[Serializable]
public class RoomStats {
    [SerializeField] RoomSize size;
    [SerializeField] RoomType type;
    [Range(4, 12)] public int sides;

    const int _tileSize = 20;

    public bool HasLoot => Type == RoomType.Loot;
    public bool HasEnemies => Type == RoomType.Guarded;
    public RoomSize Size { get => size; set => size = value; }
    public RoomType Type {
        get => type;
        set {
            if (type == value) return;
            type = value;
        }
    }


    public void SetType(RoomType newType) {
        if (newType != Type) {
            Type = newType;
        }
    }

    public int GetRoomSizeNumber() => (int)Size + 1;
    public int GetRoomRadius() => (GetRoomSizeNumber() * _tileSize) / 2;
    public float GetRoomWorldSize() => GetRoomSizeNumber() * _tileSize;
}

public enum RoomSize { Small, Medium, Large, Huge, Giant }
public enum RoomType { Normal, Guarded, Loot, Boss, Puzzle, Special }

