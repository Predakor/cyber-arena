using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Generation/Data/Floor Data", fileName = "Floor Data")]
public class FloorData : ScriptableObject {

    public BaseFloorStats _baseStats; [Space]
    public PrefabPool _floorPool; [Space]
    public FloorModifiers _floorModifiers; [Space]
    public LootRoomSettings _lootRoomSettings; [Space]
    public GuardedRoomSettings _guardedRoomSettings; [Space]
    public PlacerData _roomPlacerData;

    public FloorGenerationData GetFloorData() {
        return new FloorGenerationData {
            baseStats = _baseStats,
            floorPool = _floorPool,
            floorModifiers = _floorModifiers,
            lootRoomSettings = _lootRoomSettings,
            guardedRoomSettings = _guardedRoomSettings,
            roomPlacerData = _roomPlacerData
        };
    }

    void OnValidate() {
        //_lootRoomSettings.Validate();
        //_guardedRoomSettings.Validate();
        //_roomConnectionSettings.Validate();
    }
}

[Serializable]
public struct FloorGenerationData {
    public BaseFloorStats baseStats;
    public PrefabPool floorPool;
    public FloorModifiers floorModifiers;
    public LootRoomSettings lootRoomSettings;
    public GuardedRoomSettings guardedRoomSettings;
    public PlacerData roomPlacerData;
}

[Serializable]
public struct BaseFloorStats {
    [Tooltip("Name of the dungeon floor")]
    [SerializeField] public string name;

    [Tooltip("Base difficulty level")]
    [SerializeField] public int difficulty;

    [Tooltip("Base loot quality")]
    [SerializeField] public int lootQuality;
}

[Serializable]
public struct PrefabPool {
    [Tooltip("Prefabs for floor rooms")]
    [SerializeField] public LevelPrefabs levelPrefabs;

    [Tooltip("List of enemy prefabs that can appear on this floor")]
    [SerializeField] public EnemyPoolData enemiesPool;

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