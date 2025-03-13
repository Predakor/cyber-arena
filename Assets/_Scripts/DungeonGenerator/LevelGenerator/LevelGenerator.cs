using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(RoomPlacer), typeof(CorridorPlacer))]
public class LevelGenerator : MonoBehaviour {

    #region variables
    [Header("Dependencies")]
    [SerializeField] RoomPlacer _roomPlacer;
    [SerializeField] CorridorPlacer _corridorPlacer;

    [Header("Prefabs/templates")]
    [SerializeField] RoomData _roomDataTemplate;
    [SerializeField] FloorData _floorDataTemplate;
    [SerializeField] GameObject _corridorTemplate;

    [Header("Override base template")]
    [SerializeField] BaseFloorStats _baseStats;
    [SerializeField] PrefabPool _floorPool;
    [SerializeField] FloorModifiers _floorModifiers;
    [SerializeField] LootRoomSettings _lootRoomSettings;
    [SerializeField] GuardedRoomSettings _guardedRoomSettings;
    [SerializeField] RoomConnectionSettings _roomConnectionSettings;

    [Header("Generated rooms")]
    [SerializeField] List<RoomGenerator> _generatedRooms;
    [SerializeField] List<RoomNode> _generatedNodes; //TODO implement nodes 
    [SerializeField] List<CorridorGenerator> _generatedCorridors;

    #endregion

#if UNITY_EDITOR

    [ContextMenu("Generate Floor")]
    public void CreateFloor() {
        KillAllChildren();
        ApplyModifiers();
        GenerateFloor();
    }
    [ContextMenu("Load Floor Data")]
    public void LoadData() {
        LoadData(_floorDataTemplate);
    }
    [ContextMenu("Spawn all generated rooms")]
    public void GenerateSpawnedRooms() {
        if (_generatedRooms.Count == 0) {
            Debug.LogError("no rooms to generate, generate them first");
            return;
        }
        foreach (RoomGenerator room in _generatedRooms) {
            room.GenerateRoom();
        }
    }

    [ContextMenu("Spawn corridors")]
    public void SpawnAllCorridors() {
        foreach (var corridor in _generatedCorridors) {
            corridor.GenerateCorridor();
        }
    }

    [ContextMenu("KillAllCHildren")]
    public void KillAllChildren() {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        _generatedRooms.Clear();
    }

    [ContextMenu("Load dependencies")]
    void LoadDependencies() { SetupDependencies(); }

#endif
    public void LoadData(FloorData data) {
        FloorGenerationData floorData = data.GetFloorData();
        _baseStats = floorData.baseStats;
        _floorPool = floorData.floorPool;
        _floorModifiers = floorData.floorModifiers;
        _lootRoomSettings = floorData.lootRoomSettings;
        _guardedRoomSettings = floorData.guardedRoomSettings;
        _roomConnectionSettings = floorData.roomConnectionSettings;

        _roomDataTemplate.LoadPrefabs(_floorPool.floorPrefabs);
    }
    public void ApplyModifiers() {
        _baseStats.difficulty = Mathf.FloorToInt(_baseStats.difficulty * _floorModifiers.difficultyModifier);
        _lootRoomSettings.lootRoomChance *= _floorModifiers.lootModifier;
        _guardedRoomSettings.guardedRoomChance *= _floorModifiers.guardModifier;

        _lootRoomSettings.Validate();
        _guardedRoomSettings.Validate();
        _roomConnectionSettings.Validate();
    }

    public void SetupDependencies() {
        _roomPlacer.LoadData(_floorDataTemplate);
        _corridorPlacer.LoadData(_roomDataTemplate, _roomConnectionSettings);
    }

    public void GenerateFloor() {
        _generatedRooms.Clear();
        _generatedNodes.Clear();
        _generatedCorridors.Clear();

        _generatedRooms = _roomPlacer.GenerateRooms(_baseStats.numberOfRooms);
        GenerateLootRooms();
        GenerateGuardedRooms();
        _generatedCorridors = _corridorPlacer.PlaceCorridors(_generatedRooms, _corridorTemplate);
    }

    void GenerateLootRooms() {
        if (_lootRoomSettings.maxLootRooms > 0) {
        }
    }

    void GenerateGuardedRooms() {
        if (_guardedRoomSettings.maxGuardedRooms > 0) {
        }
    }
}
