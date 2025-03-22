using System;
using UnityEngine;

[Serializable]
public struct RoomStats {
    public RoomSize size;
    public RoomType type;
    [Range(4, 12)] public int sides;

    const int _tileSize = 20;

    public readonly bool HasLoot => type == RoomType.Loot;
    public readonly bool HasEnemies => type == RoomType.Guarded;

    public void SetType(RoomType newType) {
        if (newType != type) {
            type = newType;
        }
    }


    public readonly int GetRoomSizeNumber() => (int)size + 1;
    public readonly int GetRoomRadius() => (GetRoomSizeNumber() * _tileSize) / 2;
    public readonly float GetRoomWorldSize() => GetRoomSizeNumber() * _tileSize;
}

public enum RoomSize { Small, Medium, Large, Huge, Giant }
public enum RoomType { Normal, Guarded, Loot, Boss, Puzzle, Special }

