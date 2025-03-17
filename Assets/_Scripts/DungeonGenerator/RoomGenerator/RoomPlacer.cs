using Helpers.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlacer : MonoBehaviour {
    [Header("Prefabs/templates")]
    [SerializeField] GameObject _roomBasePrefab;
    [SerializeField] RoomRestrictionsSO _roomRestrictions;
    [SerializeField] RoomData _roomDataTemplate;

    [Header("Generation details/beahaviour")]
    [SerializeField] int _roomCount;
    [SerializeField] float _roomSpacing;
    [SerializeField] int _maxDepth = 20;
    [SerializeField] int _desiredDepth = 10;
    [SerializeField] int _maxTries = 100;
    [SerializeField] List<RoomSize> _roomSizes = new(); //probbably use SO

    /// <summary>new room| current room|new room direction</summary>
    public event Action<RoomGenerator, RoomGenerator, Vector3> OnRoomCreated;
    public event Action<RoomGenerator> OnFirstRoomCreated;

    public void Init(TemplatesHolderData data, PlacerData placerData) {
        _roomSpacing = placerData.minRoomDistance;
        _maxDepth = placerData.maxDepth;
        _desiredDepth = placerData.desiredDepth;
        _maxTries = placerData.maxTries;
        _roomSizes = placerData.roomSizes;
        _roomCount = placerData.numberOfRooms;

        _roomBasePrefab = data.RoomTemplatePrefab;
        _roomDataTemplate = data.RoomDataTemplate;
        _roomRestrictions = data.RoomRestrictions;
    }

    [ContextMenu("Generate rooms")]
    public void GenerateRooms() => GenerateRooms(_roomCount);
    public void GenerateRooms(int roomsToGenerate) {

        _maxTries = roomsToGenerate * 2;
        _maxDepth = Mathf.Clamp(_maxDepth, _desiredDepth, roomsToGenerate);

        RoomGenerator firstRoom = InstantiateFirstRoom();
        GenerateRoomRecursively(firstRoom, null, roomsToGenerate - 1);

    }

    RoomGenerator InstantiateFirstRoom() {
        RoomGenerator firstRoom = InstantiateRoom(transform.position, transform, _roomDataTemplate);
        OnFirstRoomCreated?.Invoke(firstRoom);
        return firstRoom;
    }

    void GenerateRoomRecursively(RoomGenerator previousRoom, RoomGenerator currentRoom,
       int remainingRooms, int tries = 0, int depth = 0) {
        if (remainingRooms <= 0 || tries > _maxTries) return;

        if (depth >= _maxDepth) {
            HandleBacktracking(previousRoom, remainingRooms, tries + 1);
            return;
        }

        if (currentRoom == null) {
            currentRoom = previousRoom;
        }

        RandomiseStats(out RoomData dataTemple, out RoomStats newRoomStats);

        Vector3 currentPosition = currentRoom.transform.position;
        Vector3 previousPosition = previousRoom.transform.position;

        int newRoomWorldSize = RoomGenerator.GetRoomWorldSize(newRoomStats);

        float roomToRoomDistance = newRoomWorldSize + currentRoom.GetRoomRadius();
        float minDistanceToNextRoom = roomToRoomDistance + _roomSpacing;

        Vector3 prevRoomDirection = (previousPosition - currentPosition).normalized;

        List<Vector3> avaiablePositions = RoomHelpers.GetAvaiablePositions(currentRoom, prevRoomDirection, newRoomWorldSize, minDistanceToNextRoom, _roomSpacing / 2);

        bool noAvaiablePositionsFound = avaiablePositions.Count == 0;
        if (noAvaiablePositionsFound) {
            HandleBacktracking(previousRoom, remainingRooms, tries);
            return;
        }

        Vector3 newRoomPosition = CollectionHelpers.RandomElement(avaiablePositions);
        Vector3 currentRoomDirection = (currentPosition - newRoomPosition).normalized;


        bool unconnectable = !RoomHelpers.AreConnectable(currentRoom.RoomStats, newRoomStats, currentRoomDirection);
        if (unconnectable) {
            HandleUnconnectableRooms(currentRoom.RoomStats, ref newRoomStats);
        }

        dataTemple.stats = newRoomStats;

        RoomGenerator newRoom = InstantiateRoom(newRoomPosition, transform, dataTemple);
        OnRoomCreated?.Invoke(newRoom, currentRoom, currentRoomDirection * -1);

        GenerateRoomRecursively(currentRoom, newRoom, remainingRooms - 1, tries + 1, depth + 1);
    }

    void RandomiseStats(out RoomData dataTemple, out RoomStats newRoomStats) {
        dataTemple = _roomDataTemplate;
        newRoomStats = RoomHelpers.RandomizeStats(dataTemple.stats, _roomSizes, _roomRestrictions);
    }

    static void HandleUnconnectableRooms(RoomStats currentRoom, ref RoomStats newRoom) {
        newRoom.sides = currentRoom.sides;
    }

    void HandleBacktracking(RoomGenerator prevRoom, int remainingRooms, int tries) {
        RoomLinks backtrackedRoom = prevRoom.RoomLinks.GetPrevNode();
        if (backtrackedRoom == null) {
            Debug.LogError("No Valid place for the room found terminating");
            return;
        }
        int depth = backtrackedRoom.Depth;
        GenerateRoomRecursively(backtrackedRoom.Room, prevRoom, remainingRooms, tries + 1, depth);
        Debug.DrawLine(backtrackedRoom.transform.position, prevRoom.transform.position, Color.red, 10f);
    }

    RoomGenerator InstantiateRoom(Vector3 position, Transform transform, RoomData data) {
        RoomGenerator room = Instantiate(_roomBasePrefab, position, Quaternion.identity, transform)
            .GetComponent<RoomGenerator>();
        room.gameObject.name = $"Room {data.stats.size} {data.stats.sides}gon";
        room.LoadData(data);
        return room;
    }
}
public struct InitData {
    public GameObject roomPrefab;
    public RoomRestrictionsSO roomRestrictions;
    public RoomData roomDataTemplate;
}

