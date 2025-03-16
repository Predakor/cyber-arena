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
    private bool isGuarded;

    public bool IsGuarded { get => isGuarded; set => isGuarded = value; }
}

public enum RoomSize { Small, Medium, Large, Huge, Giant }
public enum RoomType { Normal, Combat, Treasure, Boss, Puzzle, Special }

