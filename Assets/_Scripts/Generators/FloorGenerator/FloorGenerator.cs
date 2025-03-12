using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour {

    #region variables
    [Header("Prefabs/templates")]
    [SerializeField] GameObject _roomBasePrefab;
    [SerializeField] RoomData _roomDataTemplate;
    [SerializeField] FloorData _floorDataTemplate;

    [Header("Override base template")]
    [SerializeField] BaseFloorStats _baseStats;
    [SerializeField] PrefabPool _floorPool;
    [SerializeField] FloorModifiers _floorModifiers;
    [SerializeField] LootRoomSettings _lootRoomSettings;
    [SerializeField] GuardedRoomSettings _guardedRoomSettings;
    [SerializeField] RoomConnectionSettings _roomConnectionSettings;
    [SerializeField] RoomRestrictionsSO _roomRestrictions;

    [Header("Generation details/beahaviour")]
    [SerializeField] int _maxDepth = 10;
    [SerializeField] int _desiredDepth = 10;
    [SerializeField] int _maxTries = 100;
    [SerializeField] List<RoomSize> _roomSizes = new();

    [Header("Generated rooms")]
    [SerializeField] List<RoomGenerator> _generatedRooms;
    [SerializeField] List<RoomNode> _generatedNodes; //TODO implement nodes 

    int _roomSegmentSize = 20;

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
        int roomNumber = _baseStats.numberOfRooms;

        _maxTries = _baseStats.numberOfRooms * 2;
        _maxDepth = Mathf.Clamp(_maxDepth, _desiredDepth, roomNumber);

        _generatedRooms.Clear();
        Vector3 startPos = transform.position;

        RoomGenerator firstRoom = InstantiateRoom(startPos, transform, _roomDataTemplate);
        GenerateRoomRecursively(firstRoom, null, roomNumber - 1);
    }

    void GenerateRoomRecursively(RoomGenerator previousRoom, RoomGenerator currentRoom,
        int remainingRooms, int tries = 0, int depth = 0) {
        if (remainingRooms <= 0 || tries > _maxTries) return;

        if (currentRoom == null) {
            currentRoom = previousRoom;
        }

        RoomData dataTemple = _roomDataTemplate;
        RoomStats newRoomStats = RoomHelpers.RandomizeStats(dataTemple.stats, _roomSizes, _roomRestrictions);

        Vector3 currentPosition = currentRoom.transform.position;
        Vector3 previousPosition = previousRoom.transform.position;

        int newRoomWorldSize = RoomGenerator.GetRoomSizeNumber(newRoomStats.size) * _roomSegmentSize;

        float roomSpacing = _roomConnectionSettings.roomSpacing;
        float roomToRoomDistance = newRoomWorldSize + currentRoom.GetRoomRadius();
        float minDistanceToNextRoom = roomToRoomDistance + roomSpacing;

        Vector3 prevRoomDirection = (previousPosition - currentPosition).normalized;

        List<Vector3> avaiablePositions = RoomHelpers.GetAvaiablePositions(currentRoom, prevRoomDirection, newRoomWorldSize, minDistanceToNextRoom, roomSpacing);

        // if no directions go back
        bool noAvaiablePositionsFound = avaiablePositions.Count == 0;
        if (noAvaiablePositionsFound) {
            HandleBacktracking(previousRoom, remainingRooms, tries, depth);
            return;
        }

        Vector3 newRoomPosition = avaiablePositions[Random.Range(0, avaiablePositions.Count)];
        Vector3 newRoomDirection = (newRoomPosition - currentPosition).normalized;


        if (!RoomHelpers.AreRoomsConnectable(currentRoom.RoomStats, newRoomStats, newRoomDirection * -1)) {
            HandleUnconnectableRooms(currentRoom.RoomStats, newRoomStats);
        }

        dataTemple.stats = newRoomStats;


        RoomGenerator newRoom = InstantiateRoom(newRoomPosition, transform, dataTemple);
        HandleRoomLinks(currentRoom, newRoomPosition, newRoomDirection, newRoom);

        GenerateRoomRecursively(currentRoom, newRoom, remainingRooms - 1, tries + 1, depth + 1);
    }

    void HandleRoomLinks(RoomGenerator currentRoom, Vector3 newRoomPosition, Vector3 newRoomDirection, RoomGenerator newRoom) {
        newRoom.RoomLinks.Position = newRoomPosition;
        newRoom.RoomLinks.Depth = currentRoom.RoomLinks.Depth + 1;
        newRoom.RoomLinks.SetPrevRoom(currentRoom.RoomLinks, newRoomDirection * -1);
        currentRoom.RoomLinks.AddNextRoom(newRoom.RoomLinks, newRoomDirection);
    }

    void HandleUnconnectableRooms(RoomStats currentRoom, RoomStats newRoom) {
        newRoom.sides = currentRoom.sides;

    }
    void HandleBacktracking(RoomGenerator prevRoom, int remainingRooms, int tries, int depth) {
        RoomLinks backtrackedRoom = prevRoom.RoomLinks.GetPrevNode();
        if (backtrackedRoom == null) {
            Debug.LogError("No Valid place for the room found terminating");
            return;
        }
        GenerateRoomRecursively(backtrackedRoom.Room, prevRoom, remainingRooms, tries + 1, depth - 1);
        Debug.DrawLine(backtrackedRoom.transform.position, prevRoom.transform.position, Color.red, 10f);
    }

    RoomGenerator InstantiateRoom(Vector3 position, Transform transform, RoomData data) {
        RoomGenerator room = Instantiate(_roomBasePrefab, position, Quaternion.identity, transform)
            .GetComponent<RoomGenerator>();
        room.LoadData(data);
        _generatedRooms.Add(room);
        room.gameObject.name = $"Room {data.stats.size}";
        return room;
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
