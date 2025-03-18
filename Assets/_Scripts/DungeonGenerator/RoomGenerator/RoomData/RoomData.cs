using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Generation/Room/Data", fileName = "Room Data")]
public class RoomData : ScriptableObject {
    public RoomStats stats;
    public LevelPrefabs prefabs;
    public RoomLinks links;

    public void LoadPrefabs(LevelPrefabs newPrefabs) => prefabs = newPrefabs;
}

[Serializable]
public struct RoomStats {
    public RoomSize size;
    public RoomType type;
    [Range(4, 12)] public int sides;
    public bool hasTreasure;
    public bool isGuarded;

    public bool IsGuarded => type == RoomType.Guarded;
    public bool HasLoot => type == RoomType.Loot;

    public void SetType(RoomType newType) {
        if (newType != type) {
            type = newType;
        }
    }
}

public enum RoomSize { Small, Medium, Large, Huge, Giant }
public enum RoomType { Normal, Guarded, Loot, Boss, Puzzle, Special }

