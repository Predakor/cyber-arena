using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RoomGenerator/Room Data", fileName = "Room Data")]
public class RoomData : ScriptableObject {
    public RoomStats stats;
    public RoomPrefabs prefabs;
}

[Serializable]
public struct RoomPrefabs {
    public GameObject[] wallPrefabs;
    public GameObject[] floorPrefabs;
    public GameObject[] doorPrefabs;
}

[Serializable]
public struct RoomStats {
    public RoomSize size;
    public RoomType type;
    [Range(3, 12)] public int sides;
    public bool hasTreasure;
    public bool isGuarded;
}

public enum RoomSize { Small, Medium, Large, Huge, Giant }
public enum RoomType { Normal, Combat, Treasure, Boss, Puzzle, Special }

