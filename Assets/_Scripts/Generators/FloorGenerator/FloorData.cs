using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Floor Generation/Floor Data", fileName = "Floor Data")]
public class FloorData : ScriptableObject {

    public BaseFloorStats _baseStats; [Space]
    public PrefabPool _floorPool; [Space]
    public FloorModifiers _floorModifiers; [Space]
    public LootRoomSettings _lootRoomSettings; [Space]
    public GuardedRoomSettings _guardedRoomSettings; [Space]
    public RoomConnectionSettings _roomConnectionSettings;

    public FloorGenerationData GetFloorData() {
        return new FloorGenerationData {
            baseStats = _baseStats,
            floorPool = _floorPool,
            floorModifiers = _floorModifiers,
            lootRoomSettings = _lootRoomSettings,
            guardedRoomSettings = _guardedRoomSettings,
            roomConnectionSettings = _roomConnectionSettings
        };
    }

    void OnValidate() {
        _lootRoomSettings.Validate();
        _guardedRoomSettings.Validate();
        _roomConnectionSettings.Validate();
    }
}

[Serializable]
public struct FloorGenerationData {
    public BaseFloorStats baseStats;
    public PrefabPool floorPool;
    public FloorModifiers floorModifiers;
    public LootRoomSettings lootRoomSettings;
    public GuardedRoomSettings guardedRoomSettings;
    public RoomConnectionSettings roomConnectionSettings;
}

[Serializable]
public struct BaseFloorStats {
    [Tooltip("Name of the dungeon floor")]
    [SerializeField] public string name;

    [Tooltip("Total number of rooms that can be generated on this floor")]
    [SerializeField] public int numberOfRooms;

    [Tooltip("Base difficulty level")]
    [SerializeField] public int difficulty;

    [Tooltip("Base loot quality")]
    [SerializeField] public int lootQuality;
}

[Serializable]
public struct PrefabPool {
    [Tooltip("Prefabs for floor rooms")]
    [SerializeField] public RoomPrefabs floorPrefabs;

    [Tooltip("List of enemy prefabs that can appear on this floor")]
    [SerializeField] public List<GameObject> enemiesPool;

    [Tooltip("Unique rooms that can appear")]
    [SerializeField] public List<GameObject> uniqueRoomPool;

}

[Serializable]
public struct FloorModifiers {
    [Tooltip("Modifies the base difficulty of the floor")]
    [SerializeField] public float difficultyModifier;

    [Tooltip("Modifies loot spawn rates")]
    [SerializeField] public float lootModifier;

    [Tooltip("Modifies the chance of guarded rooms appearing")]
    [SerializeField] public float guardModifier;
}


[Serializable]
public struct LootRoomSettings {
    [Tooltip("Chance for a loot room to spawn")]
    [SerializeField, Range(0f, 100f)] public float lootRoomChance;

    [Tooltip("Guaranteed loot rooms on the floor")]
    [SerializeField] public int guaranteedLootRooms;

    [Tooltip("Maximum number of loot rooms")]
    [SerializeField] public int maxLootRooms;

    public void Validate() {
        maxLootRooms = Mathf.Max(guaranteedLootRooms, maxLootRooms);
        lootRoomChance = Mathf.Clamp(lootRoomChance, 0, 100);
    }
}

[Serializable]
public struct GuardedRoomSettings {
    [Tooltip("Chance for a guarded room to spawn")]
    [SerializeField, Range(0f, 100f)] public float guardedRoomChance;

    [Tooltip("Guaranteed number of guarded rooms")]
    [SerializeField] public int guaranteedGuardedRooms;

    [Tooltip("Maximum number of guarded rooms")]
    [SerializeField] public int maxGuardedRooms;

    public void Validate() {
        maxGuardedRooms = Mathf.Max(maxGuardedRooms, guaranteedGuardedRooms);
        guardedRoomChance = Mathf.Clamp(guardedRoomChance, 0, 100);
    }
}

[Serializable]
public struct RoomConnectionSettings {
    [Tooltip("Chance of a door spawning in a room")]
    [SerializeField, Range(0f, 10f)] public float doorChance;

    [Tooltip("Minimum spacing between rooms")]
    [SerializeField, Range(5f, 50f)] public float roomSpacing;

    [Tooltip("Maximum corridor length")]
    [SerializeField, Range(10f, 50f)] public float maxCorridorLength;

    [Tooltip("Minimum corridor width")]
    [SerializeField, Range(0f, 10f)] public float minCorridorWidth;

    public void Validate() {
        maxCorridorLength = Mathf.Clamp(maxCorridorLength, 10f, 50f);
        minCorridorWidth = Mathf.Clamp(minCorridorWidth, 0f, maxCorridorLength);

        roomSpacing = Mathf.Clamp(roomSpacing, 5f, 50f);
        doorChance = Mathf.Clamp(doorChance, 0f, 10f);
    }
}