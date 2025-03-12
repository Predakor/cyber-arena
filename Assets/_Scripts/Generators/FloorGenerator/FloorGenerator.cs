using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour {

    [SerializeField] FloorData _floorDataScriptableObject;

    [SerializeField] BaseFloorStats _baseStats;
    [SerializeField] PrefabPool _floorPool;
    [SerializeField] FloorModifiers _floorModifiers;
    [SerializeField] LootRoomSettings _lootRoomSettings;
    [SerializeField] GuardedRoomSettings _guardedRoomSettings;
    [SerializeField] RoomConnectionSettings _roomConnectionSettings;

    [SerializeField] GameObject roomBasePrefab;
    [SerializeField] RoomData _roomDataTemplate;

    [SerializeField] List<RoomGenerator> _generatedRooms;

    int _maxTries = 100;

#if UNITY_EDITOR

    [ContextMenu("Generate Floor")]
    public void CreateFloor() {
        KillAllChildren();
        ApplyModifiers();
        GenerateFloor();
    }

    [ContextMenu("Load Floor Data")]
    public void LoadData() {
        LoadData(_floorDataScriptableObject);
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

    [ContextMenu("KillAllCHildren")]
    public void KillAllChildren() {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        _generatedRooms.Clear();
    }

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

    public void GenerateFloor() {
        GenerateRooms();
        GenerateLootRooms();
        GenerateGuardedRooms();
    }
    void GenerateRooms() {
        _maxTries = _baseStats.numberOfRooms * 2;

        _generatedRooms.Clear(); // Clear previous rooms before generating
        Vector3 startPos = transform.position;

        RoomGenerator firstRoom = InstantiateRoom(startPos, transform, _roomDataTemplate);
        GenerateRoomRecursively(firstRoom, null, _baseStats.numberOfRooms - 1);

    }

    void GenerateRoomRecursively(RoomGenerator previousRoom, RoomGenerator currentRoom,
        int remainingRooms, int tries = 0, int depth = 0) {
        if (remainingRooms <= 0 || tries > _maxTries) return;

        if (currentRoom == null) {
            currentRoom = previousRoom;
        }

        RoomData dataTemple = _roomDataTemplate;
        RoomStats newRoomStats = RoomHelpers.RandomizeStats(dataTemple.stats);

        Vector3 currentPosition = currentRoom.transform.position;
        Vector3 previousPosition = previousRoom.transform.position;

        //randomize and validate the data
        int roomSegmentSize = 20;
        int roomWorldSize = RoomGenerator.GetRoomSizeNumber(newRoomStats.size) * roomSegmentSize;
        float minDistanceToNextRoom = roomWorldSize + previousRoom.GetRoomWorldSize() + _roomConnectionSettings.roomSpacing;

        //get possitions that can fit our room
        Vector3 prevRoomDirection = (previousPosition - currentPosition).normalized;
        List<Vector3> avaiablePositions = RoomHelpers.GetAvaiablePositions(currentRoom, prevRoomDirection, roomWorldSize, minDistanceToNextRoom);

        // if no directions go back
        bool noAvaiablePositionsFound = avaiablePositions.Count == 0;
        if (noAvaiablePositionsFound) {
            HandleBacktracking(previousRoom, remainingRooms, tries, depth);
            return;
        }

        Vector3 newRoomPosition = avaiablePositions[Random.Range(0, avaiablePositions.Count)];
        Vector3 newRoomDirection = (newRoomPosition - currentPosition).normalized;

        if (!RoomHelpers.AreRoomsConnectable(currentRoom.RoomStats, newRoomStats, newRoomDirection * -1)) {
            newRoomStats.sides = previousRoom.RoomStats.sides;
            newRoomStats.doors = new bool[previousRoom.RoomStats.sides];
        }

        newRoomStats.doors = new bool[newRoomStats.sides];
        dataTemple.stats = newRoomStats;

        RoomGenerator newRoom = InstantiateRoom(newRoomPosition, transform, dataTemple);
        newRoom.RoomLinks.Depth = depth;
        newRoom.RoomLinks.Position = newRoomPosition;
        newRoom.RoomLinks.SetPrevRoom(currentRoom.RoomLinks, newRoomDirection * -1);
        currentRoom.RoomLinks.AddNextRoom(newRoom.RoomLinks, newRoomDirection);

        GenerateRoomRecursively(currentRoom, newRoom, remainingRooms - 1, tries + 1, depth + 1);
    }

    void HandleBacktracking(RoomGenerator prevRoom, int remainingRooms, int tries, int depth) {
        Debug.LogWarning("No valid placement found for the next room. Backtracking...", prevRoom);
        RoomLinks backtrackedRoom = prevRoom.RoomLinks.GetPrevNode();
        if (backtrackedRoom != null) {
            GenerateRoomRecursively(backtrackedRoom.Room, prevRoom, remainingRooms, tries + 1, depth - 1);
            Debug.DrawLine(backtrackedRoom.transform.position, prevRoom.transform.position, Color.red, 10f);
            return;
        }
        Debug.LogError("No Valid place for the room found terminating");
    }

    RoomGenerator InstantiateRoom(Vector3 position, Transform transform, RoomData data) {
        RoomGenerator generatedRoom = Instantiate(roomBasePrefab, position, Quaternion.identity, transform)
            .GetComponent<RoomGenerator>();
        generatedRoom.LoadData(data);
        _generatedRooms.Add(generatedRoom);
        generatedRoom.gameObject.name = $"Room {data.stats.size}";
        return generatedRoom;
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
