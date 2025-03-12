using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Floor Generation/Room Data", fileName = "Room Data")]
public class RoomData : ScriptableObject {
    public RoomStats stats;
    public FloorPrefabs prefabs;
    public RoomLinks links;

    public void LoadPrefabs(FloorPrefabs newPrefabs) => prefabs = newPrefabs;
}

[Serializable]
public struct FloorPrefabs {
    public GameObject[] wallPrefabs;
    public GameObject[] floorPrefabs;
    public GameObject[] doorPrefabs;
}

[Serializable]
public struct RoomStats {
    public RoomSize size;
    public RoomType type;
    [Range(3, 12)] public int sides;
    public bool[] doors;
    public bool hasTreasure;
    public bool isGuarded;
}

public enum RoomSize { Small, Medium, Large, Huge, Giant }
public enum RoomType { Normal, Combat, Treasure, Boss, Puzzle, Special }

