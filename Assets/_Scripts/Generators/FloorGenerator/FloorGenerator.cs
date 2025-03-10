using System.Collections.Generic;
using System.Linq;
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
        ForceDoors();
    }
    void GenerateRooms() {
        _generatedRooms.Clear(); // Clear previous rooms before generating
        Vector3 startPos = transform.position;

        RoomGenerator firstRoom = InstantiateRoom(startPos, transform, _roomDataTemplate);
        GenerateRoomRecursively(firstRoom.transform.position, startPos, _baseStats.numberOfRooms - 1);

    }

    void ForceDoors() {
        for (int i = _generatedRooms.Count - 1; i > 0; i--) {
            RoomGenerator currentRoom = _generatedRooms[i - 1];
            RoomGenerator prevRoom = _generatedRooms[i];

            Vector3 prevPos = prevRoom.transform.position;
            Vector3 currPos = currentRoom.transform.position;

            var currentRoomDirections = RoomHelpers.GetRoomDirections(currentRoom.RoomStats.sides);
            var prevRoomDirections = RoomHelpers.GetRoomDirections(prevRoom.RoomStats.sides);

            Vector3 backwardDirection = (currPos - prevPos).normalized;
            Vector3 nextRoomDirection = (prevPos - currPos).normalized;

            int backwardDirectionIndex = prevRoomDirections.FindIndex(direction => direction == backwardDirection);
            int nextRoomDirectionIndex = currentRoomDirections.FindIndex(direction => direction == nextRoomDirection);

            Debug.DrawLine(prevPos, currPos, Color.green, 10f);

            if (backwardDirectionIndex != -1) {
                prevRoom.RoomStats.doors[backwardDirectionIndex] = true;
            }
            if (nextRoomDirectionIndex != -1) {
                currentRoom.RoomStats.doors[nextRoomDirectionIndex] = true;
            }
        }
    }

    void GenerateRoomRecursively(Vector3 prevPosition, Vector3 currentPosition, int remainingRooms, int tries = 0) {
        if (remainingRooms <= 0 || tries > 200) return;

        RoomGenerator prevRoom = _generatedRooms.Last();
        RoomStats roomStats = RoomHelpers.RandomizeStats(_roomDataTemplate.stats);

        //randomize and validate the data
        int roomSegmentSize = 20;
        int roomWorldSize = RoomGenerator.GetRoomSizeNumber(roomStats.size) * roomSegmentSize;
        float minDistanceToNextRoom = roomWorldSize + prevRoom.GetRoomWorldSize() + _roomConnectionSettings.roomSpacing;

        //get all possible directions
        List<Vector3> directions = RoomHelpers.GetRoomDirections(prevRoom.RoomStats.sides);

        Vector3 prevRoomDirection = (prevPosition - currentPosition).normalized;

        //transform directions to points and fillter backwardDirection out
        List<Vector3> positions = new(directions.Count - 1);
        foreach (var direction in directions) {
            if (direction != prevRoomDirection) {
                positions.Add(direction * minDistanceToNextRoom + currentPosition);
            }
        }

        //check which directions we can fit a room
        List<Vector3> avaiablePositions = RoomHelpers.GetUnoccupiedPositions(positions, roomWorldSize);

        // if no directions go back
        if (avaiablePositions.Count == 0) {
            Debug.LogWarning("No valid placement found for the next room. Backtracking...");
            GenerateRoomRecursively(transform.position, transform.position, remainingRooms, tries + 1);
            return;
        }

        // Pick a random valid direction
        Vector3 newRoomPosition = avaiablePositions[Random.Range(0, avaiablePositions.Count)];

        //correct room with different shapes // 8gon can't connect diagonally with square
        bool roomsHaveDifferentShapes = prevRoom.RoomStats.sides != roomStats.sides;
        if (roomsHaveDifferentShapes) {
            Vector3 directionToCurrentRoom = (currentPosition - newRoomPosition).normalized;
            List<Vector3> newRoomDirections = RoomHelpers.GetRoomDirections(roomStats.sides);

            bool compatibleAxis = newRoomDirections.Exists((direction) => direction == directionToCurrentRoom);
            if (!compatibleAxis) {
                roomStats.sides = prevRoom.RoomStats.sides;
            }
        }

        roomStats.doors = new bool[roomStats.sides];
        _roomDataTemplate.stats = roomStats;

        // Instantiate the new room
        RoomGenerator newRoom = InstantiateRoom(newRoomPosition, transform, _roomDataTemplate);

        // Recursively generate the next room
        GenerateRoomRecursively(currentPosition, newRoomPosition, remainingRooms - 1, tries + 1);
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
            Debug.Log($"Generating loot rooms with chance: {_lootRoomSettings.lootRoomChance}%");
        }
    }

    void GenerateGuardedRooms() {
        if (_guardedRoomSettings.maxGuardedRooms > 0) {
            Debug.Log($"Generating guarded rooms with chance: {_guardedRoomSettings.guardedRoomChance}%");
        }
    }
}
